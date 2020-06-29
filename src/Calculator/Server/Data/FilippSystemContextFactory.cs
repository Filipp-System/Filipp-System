using System;
using System.Reflection;
using Calculator.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Calculator.Server.Data
{
    /// <summary>
    /// Factory to enable running migrations from the command line
    /// </summary>
    public class FilippSystemContextFactory : IDesignTimeDbContextFactory<EmployeeContext>
    {
        public EmployeeContext CreateDbContext(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("Hosting:Environment")
                                  ?? "Development";
            var basePath = AppContext.BaseDirectory;

            // grab connection string
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();

            var config = builder.Build();
            var connectionString = config.GetConnectionString(EmployeeContext.BlazorFilippSystemDb);
            var optionsBuilder = new DbContextOptionsBuilder<EmployeeContext>();

            // use SQLite to place migrations in this assembly
            optionsBuilder.UseSqlite(connectionString, builder =>
                builder.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
            return new EmployeeContext(optionsBuilder.Options);
        }
    }
}
