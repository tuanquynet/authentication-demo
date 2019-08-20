using System.Collections.Generic;

namespace Demo.IdentityServer.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public string Project { get; set; }
        public string Email { get; set; }
    }
}
