using System;

namespace Calculator.BaseRepository
{
    /// <summary>
    /// Simplify concurrency issues.
    /// </summary>
    /// <typeparam name="TEntity">The entity type the exception is for.</typeparam>
    public class RepositoryConcurrencyException<TEntity> : Exception
    {
        /// <summary>
        /// The <see cref="TEntity"/> to update.
        /// </summary>
        public TEntity Entity { get; private set; }

        /// <summary>
        /// The changed <see cref="TEntity"/> in the database.
        /// </summary>
        public TEntity DbEntity { get; set; }

        /// <summary>
        /// The database row version.
        /// </summary>
        public byte[] RowVersion { get; set; } = null;

        public RepositoryConcurrencyException(TEntity entity, Exception ex)
            : base("A concurrency issue was detected.", ex)
        {
            Entity = entity;
        }
    }
}
