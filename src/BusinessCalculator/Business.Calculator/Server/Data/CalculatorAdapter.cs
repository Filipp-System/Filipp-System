using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Business.Calculator.Server.Data
{
    public class CalculatorAdapter
    {
        private readonly ILogger<CalculatorAdapter> _logger;

        public CalculatorAdapter()
        {
        }

        public void LogChanges(CalculatorDbContext context)
        {
            var tracker = context.ChangeTracker;

            foreach (var trackerEntry in tracker.Entries())
            {
                if (trackerEntry.State == EntityState.Unchanged)
                    return;

                _logger.LogInformation($"Database: {context.Database}. Entity tracked: {trackerEntry.Entity}." +
                                       $" Action: {trackerEntry.State} Original value: {trackerEntry.OriginalValues}." +
                                       $" New value: {trackerEntry.CurrentValues}. Metadata: {trackerEntry.Metadata}");

            }
        }
    }
}
