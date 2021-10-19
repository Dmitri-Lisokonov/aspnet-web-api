using aspnet_web_api.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;


namespace aspnet_web_api
{
    public class Program
    {
     
        public static void Main(string[] args)
        {
            DatabaseManager manager = new DatabaseManager(@"./Config/DatabaseConfig.json");
            bool isAlive = manager.Connect();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
