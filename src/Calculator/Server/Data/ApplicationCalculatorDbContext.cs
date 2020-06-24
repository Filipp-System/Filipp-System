using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Calculator.Server.Data
{
    public class ApplicationCalculatorDbContext : ApplicationDbContext
    {
        private readonly CalculatorAdapter _adapter = new CalculatorAdapter();

        public ApplicationCalculatorDbContext(DbContextOptions options, 
            IOptions<OperationalStoreOptions> operationalStoreOptions) 
            : base(options, operationalStoreOptions)
        {
        }

        public DbSet<UserCalculator> UserCalculators { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken token = default)
        {
            _adapter.SnapContext(this);
            return base.SaveChangesAsync(token);
        }
    }
}
