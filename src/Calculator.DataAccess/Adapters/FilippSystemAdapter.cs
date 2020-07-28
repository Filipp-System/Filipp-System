//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Text.Json;
//using System.Threading.Tasks;
//using Calculator.Models.DatabaseModels;
//using Microsoft.EntityFrameworkCore;

//namespace Calculator.DataAccess.Adapters
//{
//    public class FilippSystemAdapter
//    {
//        private static readonly string Unknown = nameof(Unknown);

//        public async Task<int> ProcessFilippSystemChangesAsync(
//            ClaimsPrincipal currentUser,
//            FilippSystemContext context,
//            Func<Task<int>> saveChangesAsync)
//        {
//            var user = Unknown;

//            // grab user identity
//            if (currentUser != null)
//            {
//                var name = currentUser.Claims.FirstOrDefault(
//                    c => c.Type == ClaimTypes.NameIdentifier);

//                if (name != null)
//                {
//                    user = name.Value;
//                }
//                else if (!string.IsNullOrWhiteSpace(currentUser.Identity.Name))
//                {
//                    user = currentUser.Identity.Name;
//                }
//            }

//            var employees = new List<Calculation>();

//            // FilippSystem employees

//            foreach (var item in context.ChangeTracker.Entries<Calculation>())
//            {
//                if (item.State == EntityState.Modified || 
//                    item.State == EntityState.Added || 
//                    item.State == EntityState.Deleted)
//                {
//                    // set created information for new item
//                    if (item.State == EntityState.Added)
//                    {
//                        item.Property<string>(FilippSystemContext.CreatedBy).CurrentValue =
//                            user;
//                        item.Property<DateTimeOffset>(FilippSystemContext.CreatedOn).CurrentValue =
//                            DateTimeOffset.UtcNow;
//                    }

//                    Calculation dbVal = null;

//                    // set modified information for modified item
//                    if (item.State == EntityState.Modified)
//                    {
//                        var db = await item.GetDatabaseValuesAsync();
//                        dbVal = db.ToObject() as Calculation;
//                        item.Property<string>(FilippSystemContext.ModifiedBy).CurrentValue =
//                            user;
//                        item.Property<DateTimeOffset>(FilippSystemContext.ModifiedOn).CurrentValue =
//                            DateTimeOffset.UtcNow;
//                    }

//                    // parse the changes
//                    var changes = new PropertyChanges<Calculation>(item, dbVal);
//                    var employee = new Calculation()
//                    {
//                        EmployeeID = item.Entity.EmployeeID,
//                        Action = item.State.ToString(),
//                        User = user,
//                        Changes = JsonSerializer.Serialize(changes),
//                        EmployeeReference = item.Entity
//                    };
                    
//                    employees.Add(employee);
//                }
//            }

//            if (employees.Count > 0)
//            {
//                // save
//                await context.FilippSystemEmployees.AddRangeAsync(employees);
//            }

//            var result = await saveChangesAsync();

//            // need a second round to update newly generated keys
//            var secondSave = false;

//            // attach ids for add operations
//            foreach (var employee in employees.Where(a => a.EmployeeID == 0).ToList())
//            {
//                secondSave = true;
//                employee.EmployeeID = employee.EmployeeReference.EmployeeID;
//                context.Entry(employee).State = EntityState.Modified;
//            }

//            if (secondSave)
//            {
//                await saveChangesAsync();
//            }

//            return result;
//        }
//    }

//}
