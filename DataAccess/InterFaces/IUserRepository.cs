using DataAccess.Model.setup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.InterFaces
{
    public interface IUserRepository : IDisposable
    {
        Task<DataTable> Login(LoginModel model);
        //Task<DataTable> GetUserById(int id);
        //Task<User> GetUserByUserNameAsync(LoginModel model);
        Task<DataTable> GetUserByUserNameAsync(string userName);





    }
}
