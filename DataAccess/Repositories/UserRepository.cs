using DataAccess.InterFaces;
using DataAccess.Model.Setup;
using Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

        public async Task<DataTable> GetUser()
        {
            var q = QueryBuilder.GetCommandText["Setup_user_get"];
            var data = await db.ExecuteDataTableAsync(CommandType.Text, q);
            return data;
        }

        public async Task<DbResponse> SaveUser(User model)
        {
            model.CreatedBy = 1;
            model.ModifiedBy = 1;

            var p = db.SqlParameters.AddMore("name", model.Name)
                                     .AddMore("@email", model.Email)
                                     .AddMore("@mobilenumber", model.MobileNumber)
                                     .AddMore("@username", model.UserName)
                                     .AddMore("@password", model.Password, true)
                                     .AddMore("@salt", model.Salt, true)
                                     .AddMore("@departmentId", model.DepartmentId.ConverToJSON(), true)
                                     .AddMore("@wardId", model.WardId.ConverToJSON(), true)
                                     .AddMore("@createdBy", model.CreatedBy)
                                     .AddMore("@permissionId", model.PermissionId)
                                     .AddMore("@modifiedBy", model.ModifiedBy, true)
                                     .AddMore("@Id", model.Id);
            var q = QueryBuilder.GetCommandText["Setup_user_save"];
            var dbResponse = await db.ExecuteScalarAsync(CommandType.Text, q, p);

            if (!dbResponse.HasError)
                model.Id = (int)dbResponse.Response;
            return dbResponse;

        }
        public async Task<DbResponse> DeleteUser(int id)
        {
            var p = new List<SqlParameter>
            {
                new SqlParameter("id", id)
            };
            var q = QueryBuilder.GetCommandText["Setup_user_delete"];
            var dbResponse = await db.ExecuteNonQueryAsync(CommandType.Text, q, p);
            return dbResponse;
        }

        #region API USER
        public async Task<DataTable> API_GetUsers()
        {
            var q = "SELECT Id as userid, Username as username, Name from [dbo].[User] where IsActive =1 and IsStatic =  0";
            var data = await db.ExecuteDataTableAsync(CommandType.Text, q);
            return data;
        }
        public async Task<string> API_MapUser(int userId, string token)
        {
            var p = db.SqlParameters.AddMore("@userId", userId)
                                       .AddMore("@token", token);
            var q = @"Update dbo.[User] set Token = @token, TokenLastUpdated =  GETDATE() where Id =  @userId 
                       Select top 1 Token  as [auth_token] from dbo.[User] where Id =  @userId and IsActive=1";
             var data = await db.ExecuteScalarAsync(CommandType.Text, q, p);
            return (string)data?.Response;
        }
        public async Task<DataTable> API_AuthorizeUser(string auth_token)
        {
            var p = db.SqlParameters.AddMore("@auth", auth_token, true);
            var q = @"SELECT u.Id, u.DepartmentId, u.Name, u.Username, u.Password, u.Salt, u.Email, up.PermissionId AS PermissionId, d.ValidDate
                            FROM dbo.[User] AS u
                                INNER JOIN dbo.UserPermission as up ON u.Id = up.UserId
                                LEFT JOIN Setup.Designer as d ON u.Id = d.UserId and d.IsDeleted = 0
                                 WHERE Token = @auth_token";
            var data = await db.ExecuteDataTableAsync(CommandType.Text, q, p);
            return data;

        }
        #endregion































        public void Dispose()
        {
            db.Dispose();
        }
    }
}
