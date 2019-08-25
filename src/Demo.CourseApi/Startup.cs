using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;

namespace Demo.CourseApi
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
            services.AddMvc();
            services.AddAuthentication("Bearer")
                  .AddIdentityServerAuthentication(options =>
                  {
                      options.Authority = "https://localhost:5000";
                      options.RequireHttpsMetadata = false;
                      options.ApiName = "course-api";
                  });

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Course API", Version = "v1" });
                var section = Configuration.GetSection("Identity:Swagger");
                var oauthScheme = new OAuth2Scheme();
                oauthScheme.AuthorizationUrl = $"{section["Authority"]}/connect/authorize";
                oauthScheme.TokenUrl = $"{section["Authority"]}/connect/token";
                oauthScheme.Flow = section["Flow"];
                var scopes = new Dictionary<string, string>();
                section.GetSection("Scopes").Bind(scopes);
                oauthScheme.Scopes = scopes;
                c.AddSecurityDefinition("Bearer", oauthScheme);
            });
            services.AddAuthorization(options =>
            {
                var section = Configuration.GetSection("Identity:Authorization");
                Dictionary<string, string[]> policies = new Dictionary<string, string[]>();
                section.Bind(policies);
                foreach (var policy in policies)
                {
                    options.AddPolicy(policy.Key, p =>
                    {
                        p.RequireRole(policy.Value);
                    });
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Course v1 api");
                var section = Configuration.GetSection("Identity:Swagger");
                c.ConfigureOAuth2(section["ClientId"], section["ClientSecret"], "course-api", "Swagger");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
