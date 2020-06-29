using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Calculator.DataAccess
{
    /// <summary>
    /// Defines a factory for creating <see cref="DbContext"/> instances.
    /// A service of this type is registered in the DI container by the
    /// <see cref="M:EntityFrameworkServiceCollectionExtension.AddDbContextPool"/> methods.
    /// </summary>
    /// <typeparam name="TContext">The <see cref="DbContext"/> type to create.</typeparam>
    public interface IDbContextFactory<out TContext> where TContext : DbContext
    {
        TContext CreateDbContext();
    }
}
