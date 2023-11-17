using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IUserService
    {
        LoginResult AuthenticateUser(string userid, string password, ref User user);
        void AddUser(UserViewModel model, string username);
        List<User> GetUsersDisplay();
        User GetUser(int id);
        bool DeleteUser(UserViewModel model);
        bool UpdateUser(UserViewModel model, string username);
        Task<User> GetUserByEmail(string email);
        Task<bool> UpdateUserPasswordByEmail(string email, string newPassword);
    }
}
