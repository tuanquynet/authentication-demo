using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Demo.WebClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                 .UseKestrel(options =>
                 {
                     options.Listen(IPAddress.Loopback, 10000, listenOptions =>
                     {
                         listenOptions.UseHttps(new X509Certificate2(System.IO.Path.Combine("AppData", "server.pfx"), "123456", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.DefaultKeySet));
                     });
                     options.Listen(IPAddress.Loopback, 10001, listenOptions =>
                     {
                         listenOptions.UseHttps(new X509Certificate2(System.IO.Path.Combine("AppData", "server.pfx"), "123456", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.DefaultKeySet));
                     });
                     options.Listen(IPAddress.Loopback, 10002, listenOptions =>
                     {
                         listenOptions.UseHttps(new X509Certificate2(System.IO.Path.Combine("AppData", "server.pfx"), "123456", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.DefaultKeySet));
                     });
                 })
                .Build();
    }
}
