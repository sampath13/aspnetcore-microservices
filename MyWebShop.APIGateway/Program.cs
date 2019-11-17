using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MyWebShop.APIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config)
                =>
           {
               config.AddJsonFile(Path.Combine("configuration", "configuration.json"), optional: false, reloadOnChange: true);
               config.AddJsonFile(Path.Combine("configuration", $"configuration.{hostingContext.HostingEnvironment.EnvironmentName}.json"));
           }
                )
                .UseStartup<Startup>();
    }
}
