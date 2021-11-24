using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace WebApplicationData.EF
{
    public class WebApplicationDbContextFactory : IDesignTimeDbContextFactory<WebApplicationContext>
    {
        public WebApplicationContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("WebSolutionDb");
            var optionsBuilder = new DbContextOptionsBuilder<WebApplicationContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new WebApplicationContext(optionsBuilder.Options);
            
        }
    }
}
