using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model.Setup
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public long MobileNo {  get; set; }  
        public string Salt { get; set; }
        public int PermissionId { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }


    }

     public class LoginModel
    {
        public string Username { get; set; }
        public string  Password { get; set; }
        public string salt { get; set; }
    }

    public class ForgetPassword
    {
        public string Email { get; set; }

    }
    public class ChangePassword
    {
        public string CurrentPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
    public class UserPasswordChange
    {
        public int Id { get; set; }
        public string NewPassword { get; set; }
    }
    public class ResetPassword
    {
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }

    public class PasswordResetLog 
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public string Browser { get; set; }
        public string Device { get; set; }
        [NotMapped]
        public DateTime Date { get; set; }
        [NotMapped]
        public DateTime? UserDate { get; set; }
    }


}
