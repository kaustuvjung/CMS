using Helper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Claims;

namespace tset.Models
{
    public class SessionData
    {
        public static LoginUser CurrentUser 
            {
                get
            {
                    var context = DI.Instance.Resolve<IHttpContextAccessor>().HttpContext;
                    if (context.User.Identity.IsAuthenticated)
                    {
                        int.TryParse(context.User.Claims.FirstOrDefault(x => x.Type == "Id").Value, out int id);
                        return id > 0 ? new LoginUser()
                        {
                            Id = id,
                            PermissionId = int.Parse(context.User.Claims.FirstOrDefault(x => x.Type == "Permission")?.Value),
                            Name = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value,
                            Username = context.User.Claims.FirstOrDefault(x => x.Type == "Username")?.Value,
                            Department = context.User.Claims.FirstOrDefault(x => x.Type == "Department")?.Value,
                            DepartmentId = JsonConvert.DeserializeObject<int[]>(context.User.Claims.FirstOrDefault(x => x.Type == "DepartmentId")?.Value),
                        } : null;
                    }
                    else
                    {
                        return new LoginUser
                        {
                            Id = 5
                        };
                    }

                }
            } 
        
    
    public class LoginUser
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Name { get; set; }
            public int PermissionId { get; set; }
            public bool IsAdmin { get; set; }
            public string Department {  get; set; }
            public int[] DepartmentId { get; set; }

        }
 

    }
}
