using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calculator.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Calculator.Server.Data
{
    public class CalculatorAdapter
    {
        public void SnapContext(ApplicationCalculatorDbContext context)
        {
            var calculatorUsers = new List<UserCalculator>();
            var tracker = context.ChangeTracker;

            foreach (var item in tracker.Entries<ApplicationUser>())
            {
                if (item.State == EntityState.Added ||
                    item.State == EntityState.Deleted ||
                    item.State == EntityState.Modified)
                {
                    var calculatorUser = new UserCalculator(item.State.ToString(), item.Entity);
                    if (item.State == EntityState.Modified)
                    {
                        var wasConfirmed =
                            (bool) item.OriginalValues[nameof(ApplicationUser.EmailConfirmed)];
                        if (wasConfirmed == false && item.Entity.EmailConfirmed)
                        {
                            calculatorUser.Action = "Email Confirmed";
                        }

                    }
                    calculatorUsers.Add(calculatorUser);
                }
            }

            if (calculatorUsers.Count > 0)
            {
                context.UserCalculators.AddRange(calculatorUsers);
            }
        }
    }
}
