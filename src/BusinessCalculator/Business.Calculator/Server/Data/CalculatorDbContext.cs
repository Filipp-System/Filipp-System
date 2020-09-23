using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Business.Calculator.Server.Data
{
    public class CalculatorDbContext : ApplicationDbContext
    {
        private readonly CalculatorAdapter _calculatorAdapter = new CalculatorAdapter();
        public CalculatorDbContext(DbContextOptions options, 
            IOptions<OperationalStoreOptions> operationalStoreOptions) 
            : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
        }
    }
}
