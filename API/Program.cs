using GreenLeaves.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API {
    public class Program {
        public static void Main( string [] args ) {
            IWebHost host = BuildWebHost( args );
            SeedDb( host );
            CreateWebHostBuilder( args ).Build().Run();
        }

        private static void SeedDb( IWebHost host ) {
            var scopeFactory =
                host.Services
                .GetService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope()) {
                var seeder = scope.ServiceProvider
                                .GetService<GreenLeaveSeeder>();
                seeder.SeedAsync().Wait();
            }

        }
        private static IWebHost BuildWebHost( string [] args ) {
            return WebHost.CreateDefaultBuilder( args )
                 .ConfigureAppConfiguration( SetupConfiguration )
                 .UseStartup<Startup>()
                 .Build();
        }
        public static void SetupConfiguration( WebHostBuilderContext ctx, IConfigurationBuilder builder ) {
            // Removing the default configuration options
            builder.Sources.Clear();
            builder.AddJsonFile( "appsettings.json", false, true )
                .AddEnvironmentVariables();
        }

        public static IWebHostBuilder CreateWebHostBuilder( string [] args ) =>
            WebHost.CreateDefaultBuilder( args )
                .UseStartup<Startup>();
    }
}
