using DataAccess.InterFaces;
using DataAccess.Model.setup;
using Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserRepository: IUserRepository, IDisposable
    {
        private readonly DatabaseContext db;
        public UserRepository(DatabaseContext context)
        {
            this.db = context;
        }

        public async Task<DataTable> Login(LoginModel model)
        {
            var p=  model.PrepareSQLParameters();
            var query = @"SELECT u.Id, u.DepartmentId, t.Name as Department, u.Name, u.Username, u.Password, u.Salt, u.Email, up.PermissionId AS PermissionId, d.ValidDate
                            FROM dbo.[User] AS u
                                INNER JOIN dbo.UserPermission as up ON u.Id = up.UserId
                                LEFT JOIN Setup.Designer as d ON u.Id = d.UserId and d.IsDeleted = 0
                                OUTER APPLY 
								(
										Select Stuff((Select ',' + Name from Setup.Department
										where Id In (Select value from OPENJSON(u.DepartmentId))
											for xml Path('')), 1, 1, '') as Name
								) as t
                            WHERE (u.Username= @username OR u.Email = @username) AND u.Password = @password AND u.IsActive=1";

            var data = await db.ExecuteDataTableAsync(CommandType.Text, query, p);
            return data;
        }







        public async Task<DataTable> GetUserByUserNameAsync(string userName)
        {
            var p = db.SqlParameters.AddMore("username", userName);
            var query = @"SELECT u.Id, u.DepartmentId, t.Name as Department, u.Name, u.Username, u.Password, u.Salt, u.Email, up.PermissionId AS PermissionId
                            FROM dbo.[User] AS u
                                INNER JOIN dbo.UserPermission as up ON u.Id = up.UserId
                                OUTER APPLY 
								(
										Select Stuff((Select ',' + Name from Setup.Department
										where Id In (Select value from OPENJSON(u.DepartmentId))
											for xml Path('')), 1, 1, '') as Name
								) as t
                            WHERE (u.Username= @username OR u.Email = @username) AND u.IsActive=1";
            var data = await db.ExecuteDataTableAsync(CommandType.Text, query, p);
            return data;

        }














        public void Dispose()
        {
            db.Dispose();
        }
    }
}
