using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Calculator.BaseRepository;
using Calculator.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Calculator.Repository
{
    public class UnitOfWork<TContext, TEntity> : IUnitOfWork<TEntity>
        where TContext: DbContext, ISupportUser
    {
        /// <summary>
        /// The repository to work with.
        /// </summary>
        private IRepository<TEntity, TContext> _repository;

        /// <summary>
        /// Simple repository reference.
        /// </summary>
        public IBasicRepository<TEntity> Repository => _repository;

        /// <summary>
        /// Inject the context.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="factory"></param>
        public UnitOfWork(IRepository<TEntity, TContext> repository, DbContextFactory<TContext> factory)
        {
            repository.PersistedContext = factory.CreateDbContext();
            _repository = repository;
        }

        /// <summary>
        /// Commit changes.
        /// </summary>
        /// <returns>A <see cref="Task"/>.</returns>
        public async Task CommitAsync()
        {
            try
            {
                await _repository.PersistedContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                // build the helper exception from the exception data
                var newException = new RepositoryConcurrencyException<TEntity>(
                    (TEntity)exception.Entries[0].Entity, exception);
                var dbValues = await exception.Entries[0].GetDatabaseValuesAsync(); // TODO: test if async is working correctly here

                // was deleted
                if (dbValues == null)
                {
                    newException.DbEntity = default;
                }
                else
                {
                    // update the new row version
                    newException.RowVersion = dbValues.GetValue<byte[]>(FilippSystemContext.RowVersion);
                    // grab the database version
                    newException.DbEntity = (TEntity) dbValues.ToObject();
                    // move to original so second submit works (unless there is another concurrent edit)
                    exception.Entries[0].OriginalValues.SetValues(dbValues);
                }

                throw newException;
            }
        }

        /// <summary>
        /// Sets the <see cref="ClaimsPrincipal"/> for filipp system.
        /// </summary>
        /// <param name="user">The logged in <see cref="ClaimsPrincipal"/>.</param>
        public void SetUser(ClaimsPrincipal user)
        {
            if (_repository.PersistedContext != null)
            {
                _repository.PersistedContext.User = user;
            }
        }
        
        /// <summary>
        /// Unit of work is done.
        /// </summary>
        public void Dispose()
        {
            if (_repository != null)
            {
                _repository.Dispose();
                _repository = null;
            }
        }
    }
}
