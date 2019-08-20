using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Demo.IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel(options =>
                {
                    options.Listen(IPAddress.Loopback, 5000, listenOptions =>
                    {
                        listenOptions.UseHttps(new X509Certificate2(System.IO.Path.Combine("AppData", "server.pfx"), "123456", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.DefaultKeySet));
                    });
                })
                .Build();
    }
}
