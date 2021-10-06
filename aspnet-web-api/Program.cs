using aspnet_web_api.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Collections.Specialized;

namespace aspnet_web_api
{
    public class Program
    {
     
        public static void Main(string[] args)
        {
            MySqlConnection conn = new MySqlConnection();
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json").Build();
            var section = config.GetSection("password");
            conn.Server = ConfigurationManager.AppSettings.Get("server");
            conn.DatabaseName = ConfigurationManager.AppSettings.Get("database");
            conn.UserName = ConfigurationManager.AppSettings.Get("uid");
            conn.Password = ConfigurationManager.AppSettings.Get("password");
            bool isOpen = conn.Open();
            Console.WriteLine(conn.Password);
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
