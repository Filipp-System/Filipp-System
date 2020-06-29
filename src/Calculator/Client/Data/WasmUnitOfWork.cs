using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Calculator.BaseRepository;
using Calculator.Model;
using Task = System.Threading.Tasks.Task;

namespace Calculator.Client.Data
{
    public class WasmUnitOfWork : IUnitOfWork<Employee>
    {
        /// <summary>
        /// The <see cref="Employee"/> being edited.
        /// </summary>
        public Employee OriginalEmployee => _repository.OriginalEmployee;

        /// <summary>
        /// The <see cref="Employee"/> that is in the database.
        /// </summary>
        public Employee DatabaseEmployee => _repository.DatabaseEmployee;

        /// <summary>
        /// True if there is a conflict (only exists if that happens).
        /// </summary>
        public bool HasConcurrencyConflict => _repository.DatabaseEmployee != null;

        /// <summary>
        /// The version of the last read <see cref="Employee"/>.
        /// </summary>
        public byte[] RowVersion { get; set; }

        /// <summary>
        /// Expose the <see cref="IBasicRepository{Employee}"/> interface.
        /// </summary>
        public IBasicRepository<Employee> Repository => _repository;

        /// <summary>
        /// Repository instance.
        /// </summary>
        private readonly WasmRepository _repository;

        /// <summary>
        /// Creates a new instance of the <see cref="WasmUnitOfWork"/> class.
        /// </summary>
        /// <param name="repository">The <see cref="IBasicRepository{Employee}"/> implementation.</param>
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
            return Repository.UpdateAsync(OriginalEmployee, null);
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
