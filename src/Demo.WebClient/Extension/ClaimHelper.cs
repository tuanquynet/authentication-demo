using System.Security.Claims;

namespace Demo.WebClient.Extension
{
    public static class ClaimHelper
    {
        public static bool HasRight(string rightName)
        {
            var claims = ClaimsPrincipal.Current.Identity as ClaimsIdentity;
            var validClaim = claims.FindFirst(x => x.Type == "right" && string.Equals(x.Value, rightName));
            return validClaim != null;
        }
    }
}
