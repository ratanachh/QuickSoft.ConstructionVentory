using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace QuickSoft.ConstructionVentory
{
    public static class Program
    {
        private static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }
        
        private static IWebHostBuilder CreateHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                // .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("appsettings.Development.json", false, true)
                .AddEnvironmentVariables()
                .Build();

            return  new WebHostBuilder()
                .UseConfiguration(config)
                .UseKestrel()
                .UseStartup<Startup>();
        }
    }
}