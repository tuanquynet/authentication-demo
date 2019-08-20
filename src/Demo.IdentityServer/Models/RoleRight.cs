using System.Collections.Generic;

namespace Demo.IdentityServer.Models
{
    public class RoleRight
    {
        public string Role { get; set; }
        public IEnumerable<string> Rights { get; set; }
    }
}
