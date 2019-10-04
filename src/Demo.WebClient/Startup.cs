using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace Demo.WebClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //services.AddAuthorization(options => {
            //    options.AddPolicy("ManagementPolicy", p => p.RequireRole(new string[] { "Admin", "Manager" }));
            //});
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")
            // //Implicit flow
            // .AddOpenIdConnect("oidc", options =>
            // {
            //     options.SignInScheme = "Cookies";
            //     options.Authority = "https://localhost:5000";
            //     options.RequireHttpsMetadata = true;
            //     options.ClientId = "w1";
            //     options.ClientSecret = "123456";
            //     options.SaveTokens = true;
            //     options.ResponseType = "id_token token";
            //     options.Scope.Add("course-api");
            // });

            // Authorization code
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Cookies";
                options.Authority = "https://localhost:5000";
                options.RequireHttpsMetadata = true;
                options.ClientId = "w2";
                options.ClientSecret = "123456";
                options.SaveTokens = true;
                options.ResponseType = "code";
                //options.Scope.Add("course-api");  
                //options.Scope.Add("offline_access");
            });

            // hybrid
            // .AddOpenIdConnect("oidc", options =>
            //  {
            //      options.SignInScheme = "Cookies";
            //      options.Authority = "https://localhost:5000";
            //      options.RequireHttpsMetadata = true;
            //      options.ClientId = "w3";
            //      options.ClientSecret = "123456";
            //      options.SaveTokens = true;
            //      options.ResponseType = "code id_token";
            //      options.Scope.Add("course-api");
            //      options.Scope.Add("offline_access");
            //  });


            //.AddCookie("AzureAD")
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = "AzureAD";
            //    options.DefaultChallengeScheme = "aad";
            //}).AddCookie("AzureAD")
            //.AddOpenIdConnect("aad", "Azure AD", options =>
            // {
            //     options.SignInScheme = "AzureAD";
            //     options.ClientId = "f6fb4506-4826-463f-9206-eb235b9ff950";
            //     options.Authority = "https://login.microsoftonline.com/49a190c0-a772-4234-bdb9-30fcd87ab7e2";
            //     options.ClientSecret = "ki6aPWs0qewCYIVGZi8q8MToenck5831op7CF7QXm24=";
            //     options.ResponseType = "code";
            //     options.RequireHttpsMetadata = true;
            //     options.CallbackPath = "/signin-aad";
            //     options.SignedOutCallbackPath = "/signout-callback-aad";
            //     options.RemoteSignOutPath = "/signout-aad";
            //     options.Events = new Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectEvents()
            //     {
            //         OnTokenResponseReceived = async context =>
            //         {
            //             var request = context.HttpContext.Request;
            //             var currentUri = UriHelper.BuildAbsolute(request.Scheme, request.Host, request.PathBase, request.Path);
            //             var clientCredential = new ClientCredential("f6fb4506-4826-463f-9206-eb235b9ff950", "ki6aPWs0qewCYIVGZi8q8MToenck5831op7CF7QXm24=");
            //             var authContext = new AuthenticationContext("https://login.microsoftonline.com/49a190c0-a772-4234-bdb9-30fcd87ab7e2");
            //             var token = await authContext.AcquireTokenByAuthorizationCodeAsync(context.ProtocolMessage.Code, new Uri(currentUri), clientCredential, "https://datalake.azure.net/");
            //             var aToken = token.AccessToken;
            //             context.HttpContext.Items["adToken"] = aToken;

            //         },
            //         OnTokenValidated = context =>
            //         {
            //             var identity = context.Principal.Identity as ClaimsIdentity;
            //             var claim = new Claim("http://schemas.microsoft.com/identity/claims/adToken", context.HttpContext.Items["adToken"]?.ToString());
            //             identity.AddClaim(claim);
            //             //ClaimTypes.Email
            //             return Task.CompletedTask;
            //         }
            //     };
            // });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
