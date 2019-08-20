using Demo.IdentityServer.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Demo.IdentityServer.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> users = new List<User>();
        private readonly Dictionary<string, List<string>> rights = new Dictionary<string, List<string>>();
        public UserRepository(IConfiguration configuration)
        {
            var apiSection = configuration.GetSection("Users");
            apiSection.Bind(users);
            var apiRightSection = configuration.GetSection("Rights");
            apiRightSection.Bind(rights);
        }
        public User AddUser(User user)
        {
            var u = users.FirstOrDefault(x => x.Id == user.Id);
            if(u == null)
            {
                var id = users.Select(x => x.Id).Max();
                user.Id = ++id;
                users.Add(user);
            }
            return user;
        }

        public IEnumerable<User> GetAllUser()
        {
            return users;
        }

        public User GetUserByEmail(string email)
        {
            return users.FirstOrDefault(x => x.Email == email);
        }

        public User GetUserById(int id)
        {
            return users.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<string> GetUserRoles(int userId)
        {
            var u = users.FirstOrDefault(x => x.Id == userId);
            if(u != null)
            {
                return u.Roles;
            }
            return new List<string>();
        }
        public IEnumerable<string> GetRights(string roleName)
        {
            return rights[roleName];
        }

        public bool ValidateCredentials(string userName, string password)
        {
            var user = users.FirstOrDefault(x => x.Email == userName);
            return user != null && user.Password == password;
        }
    }
}
