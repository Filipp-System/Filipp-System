using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Calculator.Model;
using Microsoft.EntityFrameworkCore;

namespace Calculator.DataAccess
{
    public class EmployeeContext : DbContext, ISupportUser
    {
        /// <summary>
        /// Tracking lifetime of context
        /// </summary>
        private readonly Guid _id;

        /// <summary>
        /// For calculator info
        /// </summary>
        private readonly FilippSystemEmployeeAdapter _adapter = new FilippSystemEmployeeAdapter();

        /// <summary>
        /// The logged in <see cref="ClaimsPrincipal"/>
        /// </summary>
        public ClaimsPrincipal User { get; set; }

        /// <summary>
        /// Magic string
        /// </summary>
        public static readonly string RowVersion = nameof(RowVersion);

        /// <summary>
        /// Who created it?
        /// </summary>
        public static readonly string CreatedBy = nameof(CreatedBy);

        /// <summary>
        /// When was it created?
        /// </summary>
        public static readonly string CreatedOn = nameof(CreatedOn);

        /// <summary>
        /// Who last modified it?
        /// </summary>
        public static readonly string ModifiedBy = nameof(ModifiedBy);

        /// <summary>
        /// When was it last modified?
        /// </summary>
        public static readonly string ModifiedOn = nameof(ModifiedOn);

        /// <summary>
        /// Magic string
        /// </summary>
        public static readonly string BlazorFilippSystemDb =
            nameof(BlazorFilippSystemDb).ToLower();

        /// <summary>
        /// Inject options
        /// </summary>
        /// <param name="options">The <see cref="DbContextOptions{EmployeeContext}"/>
        /// for the context.
        /// </param>
        public EmployeeContext(DbContextOptions<EmployeeContext> options) 
            : base(options)
        {
            _id = Guid.NewGuid();
            Debug.WriteLine($"{_id} context created.");
        }

        /// <summary>
        /// Override the save operation to generate FilippSystem information.
        /// </summary>
        /// <param name="cancellationToken">The <seealso cref="CancellationToken"/>.</param>
        /// <returns>The result.</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _adapter.ProcessEmployeeChangesAsync(
                User, this, async () => await base.SaveChangesAsync(cancellationToken));
        }

        /// <summary>
        /// List of <see cref="Employee"/>
        /// </summary>
        public DbSet<Employee> Employees { get; set; }

        public DbSet<FilippSystemEmployee> FilippSystemEmployees { get; set; }

        /// <summary>
        /// Define the model.
        /// </summary>
        /// <param name="modelBuilder">The <see cref="ModelBuilder"/>.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var employee = modelBuilder.Entity<Employee>();

            // this property isn't on the c# class
            // so we set it up as a "shadow" property and use it for concurrency
            employee.Property<byte[]>(RowVersion).IsRowVersion();

            // FilippSystem fields
            employee.Property<string>(ModifiedBy);
            employee.Property<DateTimeOffset>(ModifiedOn);
            employee.Property<string>(CreatedBy);
            employee.Property<DateTimeOffset>(CreatedOn);

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Dispose pattern.
        /// </summary>
        public override void Dispose()
        {
            Debug.WriteLine($"{_id} context disposed.");
            base.Dispose();
        }

        /// <summary>
        /// Dispose pattern.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/>.</returns>
        public override ValueTask DisposeAsync()
        {
            Debug.WriteLine($"{_id} context disposed async.");
            return base.DisposeAsync();
        }
    }
}
