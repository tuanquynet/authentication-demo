using Demo.IdentityServer.Models;
using System.Collections.Generic;

namespace Demo.IdentityServer.Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUser();
        User GetUserById(int id);
        User GetUserByEmail(string email);
        IEnumerable<string> GetUserRoles(int userId);
        User AddUser(User user);
        IEnumerable<string> GetRights(string roleName);
        bool ValidateCredentials(string userName, string password);
    }
}
