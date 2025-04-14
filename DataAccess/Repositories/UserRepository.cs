using DataAccess.InterFaces;
using DataAccess.Model.Setup;
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
            var query = @"SELECT u.Id, u.DepartmentId as Department, u.Name, u.Username, u.Password, u.Salt, u.Email, up.PermissionId AS PermissionId
                            FROM dbo.[User] AS u
                                INNER JOIN dbo.UserPermission as up ON u.Id = up.UserId
                              WHERE (u.Username= @username OR u.Email = @username) AND u.Password = @password AND u.IsActive=1";

            var data = await db.ExecuteDataTableAsync(CommandType.Text, query, p);
            return data;
        }



        public async Task<User> GetUserByUserNameAsync(LoginModel model)
        {
            var p = model.PrepareSQLParameters();
            var query = @"SELECT Id, DepartmentId, Name, Username, Password, Salt, Email, IsActive FROM
                dbo.[User] WHERE (Username= @username OR Email = @username)";
            var data = await db.ExecuteDataTableAsync(CommandType.Text, query, p);
            return data.TransformToObject<User>();
        }



        public async Task<DataTable> GetUserByUsernameAsync(string userName)
        {
            var p = db.SqlParameters.AddMore("username", userName);
            var query = @"SELECT u.Id, u.DepartmentId as Department, u.Name, u.Username, u.Password, u.Salt, u.Email, up.PermissionId AS PermissionId
                            FROM dbo.[User] AS u
                                INNER JOIN dbo.UserPermission as up ON u.Id = up.UserId
                              WHERE (u.Username= @username OR u.Email = @username) AND u.Password = @password AND u.IsActive=1";
            var data = await db.ExecuteDataTableAsync(CommandType.Text, query, p);
            return data;

        }

        public async Task<string> GetUserSaltByUsernameAsync(string userName)
        {
            var p = db.SqlParameters.AddMore("username", userName);
            var querry = @"SELECT Salt FROM dbo.[User] WHERE (Username = @username OR Email =  @username)";
            var data = await db.ExecuteScalarAsync(CommandType.Text, querry, p);
            return data.Response.ToString();



        }
















        public void Dispose()
        {
            db.Dispose();
        }
    }
}
