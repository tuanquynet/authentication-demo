using Demo.IdentityServer.Repository;
using Demo.IdentityServer.Service;
using Demo.IdentityServer.Services;
using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSingleton<IUserRepository, UserRepository>();

            // Identity server
            services.AddIdentityServer()
                // add credentials
                .AddDeveloperSigningCredential()

                // api resource
                .AddInMemoryApiResources(Config.GetApiResources())

                // users
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()

                // identity resource
                .AddInMemoryIdentityResources(Config.GetIdentityResources())

                // clients
                .AddInMemoryClients(Config.GetClients())

                .AddProfileService<ProfileService>();

            // data format
            services.AddOidcStateDataFormatterCache();

            services.AddAuthentication()
                .AddCookie()
                .AddOpenIdConnect("aad", "Azure AD", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                    options.ClientId = "6a242062-c85f-4cea-9262-97324399949e";
                    options.Authority = "https://login.microsoftonline.com/31102c06-cfad-4ba9-a40f-5457807e2881";
                    options.ClientSecret = "DERATRJ0zqzCybvQuZgyF3Kw70e7vPw9u8ASqGyTELE=";
                    options.ResponseType = "code";
                    options.RequireHttpsMetadata = true;
                    options.CallbackPath = "/signin-aad";
                    options.SignedOutCallbackPath = "/signout-callback-aad";
                    options.RemoteSignOutPath = "/signout-aad";
                })
               .AddGoogle("Google", "Google Account", options =>
               {
                   var section = Configuration.GetSection("ExternalProvider:Google");
                   section.Bind(options);
               });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();
            app.UseCors(option =>
            {
                option.AllowAnyOrigin();
                option.AllowAnyHeader();
            });
            app.UseAuthorization();
            app.UseIdentityServer();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
           {
               endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}");
           });
        }
    }
}
