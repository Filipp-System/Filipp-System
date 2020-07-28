using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Calculator.BaseRepository
{
    /// <summary>
    /// A unit of work. Implies a context will persist beyond a single operation.
    /// </summary>
    public interface IUnitOfWork<TEntity> : IDisposable 
    {
        /// <summary>
        /// The repository related to the unit of work.
        /// </summary>
        IBasicRepository<TEntity> Repository { get; }

        /// <summary>
        /// Sets the user for the filipp system
        /// </summary>
        /// <param name="user">The <see cref="ClaimsPrincipal"/> for filipp system.</param>
        void SetUser(ClaimsPrincipal user);

        /// <summary>
        /// Commit the work.
        /// </summary>
        /// <returns>An asynchronous <see cref="Task"/>.</returns>
        Task CommitAsync();
    }
}
