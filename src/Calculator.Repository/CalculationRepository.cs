using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Calculator.DataAccess;
using Calculator.Models;
using Calculator.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Calculator.Repository
{
    public class CalculationRepository : IRepository<Calculation, FilippSystemContext>
    {
        /// <summary>
        /// Factory to create contexts.
        /// </summary>
        private readonly DbContextFactory<FilippSystemContext> _factory;
        private bool _disposedValue;

        /// <summary>
        /// For longer tracked work.
        /// </summary>
        public FilippSystemContext PersistedContext { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="CalculationRepository"/> class.
        /// </summary>
        /// <param name="factory">
        /// The <see cref="DbContextFactory{FilippSystemContext}"/> to use.
        /// </param>
        public CalculationRepository(DbContextFactory<FilippSystemContext> factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Performs some work, either using the persisted context or
        /// by generating a new context for the operation.
        /// </summary>
        /// <param name="work">The work to perform (passed a <see cref="Calculator.DataAccess.FilippSystemContext"/>).</param>
        /// <param name="user">The current <see cref="ClaimsPrincipal"/>.</param>
        /// <param name="saveChanges"><c>True</c> to save changes when done.</param>
        /// <returns></returns>
        private async Task WorkInContextAsync(
            Func<FilippSystemContext, Task> work,
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
        /// Attaches an item to the <see cref="Calculator.DataAccess.FilippSystemContext"/>
        /// </summary>
        /// <param name="item">The instance of the <see cref="Calculator.Models.DatabaseModels.Calculation"/>.</param>
        public void Attach(Calculation item)
        {
            if (PersistedContext == null)
            {
                throw new InvalidOperationException("Only valid in a unit of work");
            }

            PersistedContext.Attach(item);
        }

        /// <summary>
        /// Add a new <see cref="Calculator.Models.DatabaseModels.Calculation"/>
        /// </summary>
        /// <param name="item">The <see cref="Calculator.Models.DatabaseModels.Calculation"/> to add.</param>
        /// <param name="user">The logged in <see cref="ClaimsPrincipal"/>.</param>
        /// <returns>The <see cref="Calculator.Models.DatabaseModels.Calculation"/> with id set.</returns>
        public async Task<Calculation> AddAsync(Calculation item, ClaimsPrincipal user)
        {
            await WorkInContextAsync(context =>
            {
                context.Calculations.Add(item);
                return Task.CompletedTask;
            }, user, true);
            return item;
        }

        /// <summary>
        /// Delete a <see cref="Calculator.Models.DatabaseModels.Calculation"/>.
        /// </summary>
        /// <param name="id">Id of the <see cref="Calculator.Models.DatabaseModels.Calculation"/> to delete.</param>
        /// <param name="user">The logged in <see cref="ClaimsPrincipal"/>.</param>
        /// <returns><c>True</c> when found and deleted.</returns>
        public async Task<bool> DeleteAsync(int id, ClaimsPrincipal user)
        {
            bool? result = null;
            await WorkInContextAsync(async context =>
            {
                var item = await context.Calculations.SingleOrDefaultAsync(c => c.CalculationID == id);
                if (item == null)
                {
                    result = false;
                }
                else
                {
                    context.Calculations.Remove(item);
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
        public Task<ICollection<Calculation>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Load a <see cref="Calculator.Models.DatabaseModels.Calculation"/>.
        /// </summary>
        /// <param name="id">The id of the <see cref="Calculator.Models.DatabaseModels.Calculation"/> to load.</param>
        /// <param name="user">The logged in <see cref="ClaimsPrincipal"/>.</param>
        /// <param name="forUpdate"><c>True</c> to keep tracking on.</param>
        /// <returns>The <see cref="Calculator.Models.DatabaseModels.Calculation"/>.</returns>
        public async Task<Calculation> LoadAsync(int id, ClaimsPrincipal user, bool forUpdate = false)
        {
            Calculation calculation = null;
            await WorkInContextAsync(async context =>
            {
                var calculationReference = context.Calculations;
                if (forUpdate)
                {
                    calculationReference.AsNoTracking();
                }

                calculation = await calculationReference.SingleOrDefaultAsync(c => c.CalculationID == id);
            }, user);
            return calculation;
        }

        /// <summary>
        /// Query the <see cref="Calculator.Models.DatabaseModels.Calculation"/> database.
        /// </summary>
        /// <param name="query">
        /// A delegate that provides an <see cref="IQueryable{Calculation}"/>
        /// to build on.
        /// </param>
        /// <returns>A <see cref="Task"/></returns>
        public async Task QueryAsync(Func<IQueryable<Calculation>, Task> query)
        {
            await WorkInContextAsync(async context =>
            {
                await query(context.Calculations.AsNoTracking().AsQueryable());
            }, null);
        }

        /// <summary>
        /// Update the <see cref="Calculator.Models.DatabaseModels.Calculation"/> (without a unit of work).
        /// </summary>
        /// <param name="item">The <see cref="Calculator.Models.DatabaseModels.Calculation"/> to update.</param>
        /// <param name="user">The logged in <see cref="ClaimsPrincipal"/>.</param>
        /// <returns>The updated <see cref="Calculator.Models.DatabaseModels.Calculation"/>.</returns>
        public async Task<Calculation> UpdateAsync(Calculation item, ClaimsPrincipal user)
        {
            await WorkInContextAsync(context =>
            {
                context.Calculations.Attach(item);
                return Task.CompletedTask;
            }, user, true);
            return item;
        }

        /// <summary>
        /// Grabs the value of a property. Useful for shadow properties.
        /// </summary>
        /// <typeparam name="TPropertyType">The type of the property.</typeparam>
        /// <param name="item">The <see cref="Calculator.Models.DatabaseModels.Calculation"/> the property is on.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The value of the property.</returns>
        public async Task<TPropertyType> GetPropertyValueAsync<TPropertyType>(Calculation item, string propertyName)
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
        /// <param name="item">The <see cref="Calculator.Models.DatabaseModels.Calculation"/> being tracked.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public async Task SetOriginalValueForConcurrencyAsync<TPropertyType>(Calculation item, string propertyName, TPropertyType value)
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
            if (!_disposedValue)
            {
                if (disposing)
                {
                    PersistedContext?.Dispose();
                }

                _disposedValue = true;
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
