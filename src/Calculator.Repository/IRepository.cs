using System;
using Calculator.BaseRepository;
using Calculator.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Calculator.Repository
{
    /// <summary>
    /// More fine-grained repository that exposes the data context. Used by
    /// the <see cref="UnitOfWork{TContext, TEntity}"/> implementation.
    /// </summary>
    /// <typeparam name="TEntity">The <see cref="TEntity"/> for the repository.</typeparam>
    /// <typeparam name="TContext">The <see cref="DbContext"/> type.</typeparam>
    public interface IRepository<TEntity, TContext> : IDisposable, IBasicRepository<TEntity>
        where TContext : DbContext, ISupportUser
    {
        /// <summary>
        /// The <see cref="DbContext"/> instance.
        /// </summary>
        TContext PersistedContext { get; set; }
    }
}
