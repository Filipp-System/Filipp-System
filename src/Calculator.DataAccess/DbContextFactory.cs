using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.DataAccess
{
    /// <summary>
    /// Factory to generate instance of <see cref="TContext"/>.
    /// </summary>
    /// <typeparam name="TContext">The <see cref="DbContext"/> type to provide.</typeparam>
    public class DbContextFactory<TContext> : IDbContextFactory<TContext> where TContext : DbContext 
    {
        /// <summary>
        /// Service provider.
        /// </summary>
        private readonly IServiceProvider _provider;

        /// <summary>
        /// Inject the service provider.
        /// </summary>
        /// <param name="provider">The <see cref="IServiceProvider"/> instance.</param>
        public DbContextFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Create a newly created context.
        /// </summary>
        /// <returns>A new instance of <see cref="TContext"/>.</returns>
        public TContext CreateDbContext()
        {
            if (_provider == null)
            {
                throw new InvalidOperationException($"An instance of IServiceProvider must be configured.");
            }

            // satisfies any dependencies via the provider 
            return ActivatorUtilities.CreateInstance<TContext>(_provider);
        }
    }
}
