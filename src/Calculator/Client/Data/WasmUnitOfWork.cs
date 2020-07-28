using System;
using System.Security.Claims;
using Calculator.BaseRepository;
using Calculator.Models;
using Calculator.Models.DatabaseModels;
using Task = System.Threading.Tasks.Task;

namespace Calculator.Client.Data
{
    public class WasmUnitOfWork : IUnitOfWork<Employee>
    {
        /// <summary>
        /// The <see cref="Calculator.Models.DatabaseModels.Calculation"/> being edited.
        /// </summary>
        public Employee OriginalCalculation => _repository.OriginalCalculation;

        /// <summary>
        /// The <see cref="Calculator.Models.DatabaseModels.Calculation"/> that is in the database.
        /// </summary>
        public Employee DatabaseCalculation => _repository.DatabaseCalculation;

        /// <summary>
        /// True if there is a conflict (only exists if that happens).
        /// </summary>
        public bool HasConcurrencyConflict => _repository.DatabaseCalculation != null;

        /// <summary>
        /// The version of the last read <see cref="Calculator.Models.DatabaseModels.Calculation"/>.
        /// </summary>
        public byte[] RowVersion { get; set; }

        /// <summary>
        /// Expose the <see cref="IBasicRepository{Calculation}"/> interface.
        /// </summary>
        public IBasicRepository<Employee> Repository => _repository;

        /// <summary>
        /// Repository instance.
        /// </summary>
        private readonly WasmRepository _repository;

        /// <summary>
        /// Creates a new instance of the <see cref="WasmUnitOfWork"/> class.
        /// </summary>
        /// <param name="repository">The <see cref="IBasicRepository{Calculation}"/> implementation.</param>
        public WasmUnitOfWork(IBasicRepository<Employee> repository)
        {
            _repository = repository as WasmRepository;
        }

        /// <summary>
        /// Time to commit.
        /// </summary>
        /// <returns>A <see cref="Task"/>.</returns>
        public Task CommitAsync()
        {
            return Repository.UpdateAsync(OriginalCalculation, null);
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="user"></param>
        public void SetUser(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// dispose the reference
        /// </summary>
        public void Dispose()
        {
        }
    }
}
