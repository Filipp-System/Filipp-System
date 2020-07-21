using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Calculator.DataAccess;
using Calculator.Model;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Calculator.Repository
{
    public class EmployeeRepository : IRepository<Employee, EmployeeContext>
    {
        /// <summary>
        /// Factory to create contexts.
        /// </summary>
        private readonly DbContextFactory<EmployeeContext> _factory;
        private bool disposedValue;

        /// <summary>
        /// For longer tracked work.
        /// </summary>
        public EmployeeContext PersistedContext { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="EmployeeRepository"/> class.
        /// </summary>
        /// <param name="factory">
        /// The <see cref="DbContextFactory{EmployeeContext}"/> to use.
        /// </param>
        public EmployeeRepository(DbContextFactory<EmployeeContext> factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Performs some work, either using the persisted context or
        /// by generating a new context for the operation.
        /// </summary>
        /// <param name="work">The work to perform (passed a <see cref="EmployeeContext"/>).</param>
        /// <param name="user">The current <see cref="ClaimsPrincipal"/>.</param>
        /// <param name="saveChanges"><c>True</c> to save changes when done.</param>
        /// <returns></returns>
        private async Task WorkInContextAsync(
            Func<EmployeeContext, Task> work,
            ClaimsPrincipal user,
            bool saveChanges = false)
        {
            if (PersistedContext != null)
            {
                if (user != null)
                {
                    PersistedContext.User = user;
                }

                // do some work. saveChanges flag is ignored because this will be committed later.
                await work(PersistedContext);
            }
            else
            {
                using (var context = _factory.CreateDbContext())
                {
                    context.User = user;
                    await work(context);
                    if (saveChanges)
                    {
                        await context.SaveChangesAsync();
                    }
                }
            }
        }

        /// <summary>
        /// Attaches an item to the <see cref="EmployeeContext"/>
        /// </summary>
        /// <param name="item">The instance of the <see cref="Employee"/>.</param>
        public void Attach(Employee item)
        {
            if (PersistedContext == null)
            {
                throw new InvalidOperationException("Only valid in a unit of work");
            }

            PersistedContext.Attach(item);
        }

        /// <summary>
        /// Add a new <see cref="Employee"/>
        /// </summary>
        /// <param name="item">The <see cref="Employee"/> to add.</param>
        /// <param name="user">The logged in <see cref="ClaimsPrincipal"/>.</param>
        /// <returns>The <see cref="Employee"/> with id set.</returns>
        public async Task<Employee> AddAsync(Employee item, ClaimsPrincipal user)
        {
            await WorkInContextAsync(context =>
            {
                context.Employees.Add(item);
                return Task.CompletedTask;
            }, user, true);
            return item;
        }

        /// <summary>
        /// Delete a <see cref="Employee"/>.
        /// </summary>
        /// <param name="id">Id of the <see cref="Employee"/> to delete.</param>
        /// <param name="user">The logged in <see cref="ClaimsPrincipal"/>.</param>
        /// <returns><c>True</c> when found and deleted.</returns>
        public async Task<bool> DeleteAsync(int id, ClaimsPrincipal user)
        {
            bool? result = null;
            await WorkInContextAsync(async context =>
            {
                var item = await context.Employees.SingleOrDefaultAsync(c => c.EmployeeID == id);
                if (item == null)
                {
                    result = false;
                }
                else
                {
                    context.Employees.Remove(item);
                }
            }, user, true);
            if (!result.HasValue)
            {
                result = true;
            }

            return result.Value;
        }

        /// <summary>
        /// Not implemented here (see Blazor WebAssembly client).
        /// </summary>
        /// <returns></returns>
        public Task<ICollection<Employee>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Load a <see cref="Employee"/>.
        /// </summary>
        /// <param name="id">The id of the <see cref="Employee"/> to load.</param>
        /// <param name="user">The logged in <see cref="ClaimsPrincipal"/>.</param>
        /// <param name="forUpdate"><c>True</c> to keep tracking on.</param>
        /// <returns>The <see cref="Employee"/>.</returns>
        public async Task<Employee> LoadAsync(int id, ClaimsPrincipal user, bool forUpdate = false)
        {
            Employee employee = null;
            await WorkInContextAsync(async context =>
            {
                var employeeReference = context.Employees;
                if (forUpdate)
                {
                    employeeReference.AsNoTracking();
                }

                employee = await employeeReference.SingleOrDefaultAsync(c => c.EmployeeID == id);
            }, user);
            return employee;
        }

        /// <summary>
        /// Query the <see cref="Employee"/> database.
        /// </summary>
        /// <param name="query">
        /// A delegate that provides an <see cref="IQueryable{Employee}"/>
        /// to build on.
        /// </param>
        /// <returns>A <see cref="Task"/></returns>
        public async Task QueryAsync(Func<IQueryable<Employee>, Task> query)
        {
            await WorkInContextAsync(async context =>
            {
                await query(context.Employees.AsNoTracking().AsQueryable());
            }, null);
        }

        /// <summary>
        /// Update the <see cref="Employee"/> (without a unit of work).
        /// </summary>
        /// <param name="item">The <see cref="Employee"/> to update.</param>
        /// <param name="user">The logged in <see cref="ClaimsPrincipal"/>.</param>
        /// <returns>The updated <see cref="Employee"/>.</returns>
        public async Task<Employee> UpdateAsync(Employee item, ClaimsPrincipal user)
        {
            await WorkInContextAsync(context =>
            {
                context.Employees.Attach(item);
                return Task.CompletedTask;
            }, user, true);
            return item;
        }

        /// <summary>
        /// Grabs the value of a property. Useful for shadow properties.
        /// </summary>
        /// <typeparam name="TPropertyType">The type of the property.</typeparam>
        /// <param name="item">The <see cref="Employee"/> the property is on.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The value of the property.</returns>
        public async Task<TPropertyType> GetPropertyValueAsync<TPropertyType>(Employee item, string propertyName)
        {
            TPropertyType value = default;
            await WorkInContextAsync(context =>
            {
                value = context.Entry(item).Property<TPropertyType>(propertyName).CurrentValue;
                return Task.CompletedTask;
            }, null);
            return value;
        }

        /// <summary>
        /// Sets original value. This is useful to check concurrency if you have
        /// disconnected entities and are re-attaching to update.
        /// </summary>
        /// <typeparam name="TPropertyType">The type of the property.</typeparam>
        /// <param name="item">The <see cref="Employee"/> being tracked.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public async Task SetOriginalValueForConcurrencyAsync<TPropertyType>(Employee item, string propertyName, TPropertyType value)
        {
            await WorkInContextAsync(context =>
            {
                var tracked = context.Entry(item);
                // tell EFCore which version was loaded
                tracked.Property<TPropertyType>(propertyName).OriginalValue = value;
                // tell EFCore to modify entity
                tracked.State = EntityState.Modified;
                return Task.CompletedTask;
            }, null);
        }

        /// <summary>
        /// Implements the dispose pattern.
        /// </summary>
        /// <param name="disposing"><c>True</c> when disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    PersistedContext?.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Implement <see cref="IDisposable"/>.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
