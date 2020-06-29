using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calculator.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Calculator.Server.Data
{
    public class FilippSystemAdapter
    {
        public void Snap(ApplicationFilippSystemDbContext context)
        {
            var filippSystemUsers = new List<UserFilippSystem>();
            var tracker = context.ChangeTracker;
            foreach (var item in tracker.Entries<ApplicationUser>())
            {
                if (item.State == EntityState.Added ||
                    item.State == EntityState.Deleted ||
                    item.State == EntityState.Modified)
                {
                    var filippSystemUser = new UserFilippSystem(item.State.ToString(), item.Entity);
                    if (item.State == EntityState.Modified)
                    {
                        var wasConfirmed =
                            (bool) item.OriginalValues[nameof(ApplicationUser.EmailConfirmed)];
                        if (wasConfirmed == false && item.Entity.EmailConfirmed)
                        {
                            filippSystemUser.Action = "Email Confirmed.";
                        }
                    }
                    filippSystemUsers.Add(filippSystemUser);
                }
            }

            if (filippSystemUsers.Count > 0)
            {
                context.FilippSystemUsers.AddRange(filippSystemUsers);
            }
        }
    }
}
