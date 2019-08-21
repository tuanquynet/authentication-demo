using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using ThumbprintExtension;
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
                        //listenOptions.UseHttps(X509.GetCertificate("BDEE91B4AD15C5C8A8164AFBB15C0292E1C5E94C"));
                        listenOptions.UseHttps(new X509Certificate2(System.IO.Path.Combine("AppData", "server.pfx"), "123456", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.DefaultKeySet));
                    });
                })
                .Build();
    }
}
