using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using alintaApi.Data;

namespace alintaApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            // Find the service layer within our scope.
            using (var scope = host.Services.CreateScope())
            {
                // Get the instance of CustomersDBContext in our services layer
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<CustomersDbContext>();

                // Call the DataGenerator to create sample data
                DataGenerator.Initialize(services);
            }

            // Continue to run the application
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
