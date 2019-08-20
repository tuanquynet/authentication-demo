using Demo.IdentityServer.Repository;
using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Demo.IdentityServer.Services
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserRepository userRepository;
        private readonly ISystemClock clock;
        public ResourceOwnerPasswordValidator(IUserRepository userRepository, ISystemClock clock)
        {
            this.clock = clock;
            this.userRepository = userRepository;
        }
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (userRepository.ValidateCredentials(context.UserName, context.Password))
            {
                var user = userRepository.GetUserByEmail(context.UserName);
                var roles = userRepository.GetUserRoles(user.Id);
                var claims = new List<Claim>();
                if (roles.Any())
                {
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(JwtClaimTypes.Role, role));
                        claims.Add(new Claim(ClaimTypes.Role, role));
                        var rights = userRepository.GetRights(role);
                        if (rights.Any())
                        {
                            claims.AddRange(rights.Select(x => new Claim("right", x.Trim())));
                        }
                    }
                }
                context.Result = new GrantValidationResult(
                    user.Email ?? throw new ArgumentException("Subject ID not set", nameof(user.Email)),
                    OidcConstants.AuthenticationMethods.Password, clock.UtcNow.DateTime, claims);

            }

            return Task.CompletedTask;
        }
    }
}
