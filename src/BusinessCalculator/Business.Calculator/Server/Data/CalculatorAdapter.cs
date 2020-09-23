using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Business.Calculator.Server.Data
{
    public class CalculatorAdapter
    {
        public void LogChanges(CalculatorDbContext context)
        {
            var tracker = context.ChangeTracker;

            foreach (var trackerEntry in tracker.Entries())
            {
                if (trackerEntry.State == EntityState.Unchanged)
                    return;

                Log.Information($"Database: {context.Database}. Entity tracked: {trackerEntry.Entity}." +
                                       $" Action: {trackerEntry.State} Original value: {trackerEntry.OriginalValues}." +
                                       $" New value: {trackerEntry.CurrentValues}. Metadata: {trackerEntry.Metadata}");
            }
        }

    }
}
