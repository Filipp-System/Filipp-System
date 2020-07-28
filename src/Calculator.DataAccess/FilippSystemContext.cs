using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Calculator.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace Calculator.DataAccess
{
    public class FilippSystemContext : DbContext, ISupportUser
    {
        /// <summary>
        /// Tracking lifetime of context
        /// </summary>
        private readonly Guid _id;

        /// <summary>
        /// For calculator info
        /// </summary>
        //private readonly FilippSystemAdapter _adapter = new FilippSystemAdapter();

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
        /// <param name="options">The <see cref="DbContextOptions{FilippSystemContext}"/>
        /// for the context.
        /// </param>
        public FilippSystemContext(DbContextOptions<FilippSystemContext> options)
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
        //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    return await _adapter.ProcessFilippSystemChangesAsync(
        //        User, this, async () => await base.SaveChangesAsync(cancellationToken));
        //}

        /// <summary>
        /// List of <see cref="Calculator.Models.DatabaseModels.Calculation"/>
        /// </summary>
        public DbSet<Calculation> Calculations { get; set; }

        /// <summary>
        /// List of <see cref="CalculationTask"/>
        /// </summary>
        public DbSet<CalculationTask> CalculationTasks { get; set; }

        /// <summary>
        /// List of <see cref="Employees"/>
        /// </summary>
        public DbSet<Employee> Employees { get; set; }

        /// <summary>
        /// List of <see cref="Machine"/>
        /// </summary>
        public DbSet<Machine> Machines { get; set; }

        /// <summary>
        /// List of <see cref="Material"/>
        /// </summary>
        public DbSet<Material> Materials { get; set; }

        public DbSet<FilippSystemApplicationInformation> FilippSystemApplicationInformation { get; set; }

        

        /// <summary>
        /// Define the model.
        /// </summary>
        /// <param name="modelBuilder">The <see cref="ModelBuilder"/>.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var filippSystemApplicationInformation = modelBuilder.Entity<FilippSystemApplicationInformation>();

            // this property isn't on the c# class
            // so we set it up as a "shadow" property and use it for concurrency
            filippSystemApplicationInformation.Property<byte[]>(RowVersion).IsRowVersion();

            // FilippSystem fields
            filippSystemApplicationInformation.Property<string>(ModifiedBy);
            filippSystemApplicationInformation.Property<DateTimeOffset>(ModifiedOn);
            filippSystemApplicationInformation.Property<string>(CreatedBy);
            filippSystemApplicationInformation.Property<DateTimeOffset>(CreatedOn);

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
