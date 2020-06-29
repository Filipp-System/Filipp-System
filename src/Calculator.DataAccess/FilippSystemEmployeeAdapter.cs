using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Calculator.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Task = Calculator.Model.Task;

namespace Calculator.DataAccess
{
    public class FilippSystemEmployeeAdapter
    {
        private static readonly string Unknown = nameof(Unknown);

        public async Task<int> ProcessEmployeeChangesAsync(
            ClaimsPrincipal currentUser,
            EmployeeContext context,
            Func<Task<int>> saveChangesAsync)
        {
            var user = Unknown;

            // grab user identity
            if (currentUser != null)
            {
                var name = currentUser.Claims.FirstOrDefault(
                    c => c.Type == ClaimTypes.NameIdentifier);

                if (name != null)
                {
                    user = name.Value;
                }
                else if (!string.IsNullOrWhiteSpace(currentUser.Identity.Name))
                {
                    user = currentUser.Identity.Name;
                }
            }

            var filippSystemEmployees = new List<FilippSystemEmployee>();

            // FilippSystem employees

            foreach (var item in context.ChangeTracker.Entries<Employee>())
            {
                if (item.State == EntityState.Modified || 
                    item.State == EntityState.Added || 
                    item.State == EntityState.Deleted)
                {
                    // set created information for new item
                    if (item.State == EntityState.Added)
                    {
                        item.Property<string>(EmployeeContext.CreatedBy).CurrentValue =
                            user;
                        item.Property<DateTimeOffset>(EmployeeContext.CreatedOn).CurrentValue =
                            DateTimeOffset.UtcNow;
                    }

                    Employee dbVal = null;

                    // set modified information for modified item
                    if (item.State == EntityState.Modified)
                    {
                        var db = await item.GetDatabaseValuesAsync();
                        dbVal = db.ToObject() as Employee;
                        item.Property<string>(EmployeeContext.ModifiedBy).CurrentValue =
                            user;
                        item.Property<DateTimeOffset>(EmployeeContext.ModifiedOn).CurrentValue =
                            DateTimeOffset.UtcNow;
                    }

                    // parse the changes
                    var changes = new PropertyChanges<Employee>(item, dbVal);
                    var filippSystemEmployee = new FilippSystemEmployee()
                    {
                        EmployeeID = item.Entity.EmployeeID,
                        Action = item.State.ToString(),
                        User = user,
                        Changes = JsonSerializer.Serialize(changes),
                        EmployeeReference = item.Entity
                    };
                    
                    filippSystemEmployees.Add(filippSystemEmployee);
                }
            }

            if (filippSystemEmployees.Count > 0)
            {
                // save
                await context.FilippSystemEmployees.AddRangeAsync(filippSystemEmployees);
            }

            var result = await saveChangesAsync();

            // need a second round to update newly generated keys
            var secondSave = false;

            // attach ids for add operations
            foreach (var filippSystemEmployee in filippSystemEmployees.Where(a => a.EmployeeID == 0).ToList())
            {
                secondSave = true;
                filippSystemEmployee.EmployeeID = filippSystemEmployee.EmployeeReference.EmployeeID;
                context.Entry(filippSystemEmployee).State = EntityState.Modified;
            }

            if (secondSave)
            {
                await saveChangesAsync();
            }

            return result;
        }
    }

}
