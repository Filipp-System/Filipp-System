using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Calculator.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;

namespace Calculator.DataAccess
{
    public class FilippSystemContext : DbContext, ISupportUser
    {

        #region DbSets

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

        #endregion

        /// <summary>
        /// Tracking lifetime of context
        /// </summary>
        private readonly Guid _id;

        /// <summary>
        /// The Logger Instance for this Class
        /// </summary>
        private readonly ILogger<FilippSystemContext> _logger;
        
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
        /// <param name="logger">The logger instance for ElasticSearch logs</param>
        public FilippSystemContext(DbContextOptions<FilippSystemContext> options, ILogger<FilippSystemContext> logger)
            : base(options)
        {
            _logger = logger;
            _id = Guid.NewGuid();
            _logger.Log(LogLevel.Debug,$"{_id} context created.");
        }

        /// <summary>
        /// Override the save operation to generate FilippSystem information.
        /// </summary>
        /// <param name = "cancellationToken" > The < seealso cref= "CancellationToken" />.</ param >
        /// < returns > The result.</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                _logger.Log(LogLevel.Critical, dbUpdateConcurrencyException,
                    $"Error {nameof(DbUpdateConcurrencyException)} occured while performing {nameof(SaveChangesAsync)}: {dbUpdateConcurrencyException.Message}; User: {User}");
                throw;
            }
            catch (DbUpdateException dbUpdateException)
            {
                _logger.Log(LogLevel.Critical, dbUpdateException,
                    $"Error {nameof(DbUpdateException)} occured while performing {nameof(SaveChangesAsync)}: {dbUpdateException.Message}; User: {User}");
                throw;
            }
            finally
            {
                _logger.Log(LogLevel.Information, $"User: {User}, TransactionId: {Database.CurrentTransaction.TransactionId}, RowVersion: {RowVersion}");
            }

        }

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

            _logger.Log(LogLevel.Information, filippSystemApplicationInformation.ToJson(new JsonWriterSettings(){ OutputMode = JsonOutputMode.Strict}));

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Dispose pattern.
        /// </summary>
        public override void Dispose()
        {
            var logMessage = $"{_id} context disposed.";

            _logger.Log(LogLevel.Debug, logMessage);
            Debug.WriteLine(logMessage);
            base.Dispose();
        }

        /// <summary>
        /// Dispose pattern.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/>.</returns>
        public override ValueTask DisposeAsync()
        {
            var logMessage = $"{_id} context disposed async.";

            _logger.Log(LogLevel.Debug, logMessage);
            Debug.WriteLine(logMessage);
            return base.DisposeAsync();
        }
    }
}
