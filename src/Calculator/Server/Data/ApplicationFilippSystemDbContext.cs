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
    public class ApplicationFilippSystemDbContext : ApplicationDbContext
    {
        private readonly FilippSystemAdapter _adapter = new FilippSystemAdapter();

        public ApplicationFilippSystemDbContext(DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
        }

        public DbSet<UserFilippSystem> FilippSystemUsers { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            _adapter.Snap(this);
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
