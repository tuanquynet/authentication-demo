using Demo.IdentityServer.Repository;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
namespace Demo.IdentityServer.Service
{
    public class ProfileService : IProfileService
    {
        private readonly IUserRepository userRepository;
        public ProfileService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var externalUser = context.Subject;
            var claimValue = externalUser.FindFirst(JwtClaimTypes.Email) ??
                             externalUser.FindFirst(ClaimTypes.Email)??
                             externalUser.FindFirst(JwtClaimTypes.Name) ??
                             externalUser.FindFirst(ClaimTypes.Name);

            if (claimValue != null)
            {
                // get user account and role
                var user = userRepository.GetUserByEmail(claimValue.Value);
                if (user != null)
                {

                    context.IssuedClaims.Add(new Claim(JwtClaimTypes.Id, user.Id.ToString()));
                    context.IssuedClaims.Add(new Claim(JwtClaimTypes.Email, user.Email));
                    var roles = userRepository.GetUserRoles(user.Id);

                    if (roles.Any())
                    {
                        foreach (var role in roles)
                        {
                            context.IssuedClaims.Add(new Claim(ClaimTypes.Role, role));
                            context.IssuedClaims.Add(new Claim(JwtClaimTypes.Role, role));
                            var rights = userRepository.GetRights(role);
                            if (rights.Any())
                            {
                                var claims = rights.Select(x => new Claim("right", x.Trim()));
                                context.IssuedClaims.AddRange(claims);
                            }
                        }
                    }
                }
            }
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }
    }
}
