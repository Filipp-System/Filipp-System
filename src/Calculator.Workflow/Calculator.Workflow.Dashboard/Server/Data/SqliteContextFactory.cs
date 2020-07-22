using System;
using Elsa.Persistence.EntityFrameworkCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Calculator.Workflow.Dashboard.Server.Data
{
    public class SqliteContextFactory : IDesignTimeDbContextFactory<SqliteContext>
    {
        public SqliteContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SqliteContext>();
            var migrationAssembly = typeof(SqliteContext).Assembly.FullName;
            var connectionString = Environment.GetEnvironmentVariable("EF_CONNECTIONSTRING") ??
                                   @"Data Source=workflow.db;Cache=Shared";

            optionsBuilder.UseSqlite(connectionString,
                x => x.MigrationsAssembly(migrationAssembly));
            
            return new SqliteContext(optionsBuilder.Options);
        }
    }
}