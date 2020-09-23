using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Business.Calculator.Server.Data
{
    public class CalculatorDbContext : ApplicationDbContext
    {
        private readonly ILogger<CalculatorDbContext> _logger;
        private readonly CalculatorAdapter _calculatorAdapter = new CalculatorAdapter();
        public CalculatorDbContext(DbContextOptions options, 
            IOptions<OperationalStoreOptions> operationalStoreOptions, ILogger<CalculatorDbContext> logger) 
            : base(options, operationalStoreOptions)
        {
            _logger = logger;
        }
    }
}
