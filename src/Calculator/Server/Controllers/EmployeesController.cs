using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calculator.BaseRepository;
using Calculator.Client.Data;
using Calculator.DataAccess;
using Calculator.Models;
using Calculator.Models.DatabaseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IBasicRepository<Employee> _repository;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Creates a new instance of the <see cref="EmployeesController"/>.
        /// </summary>
        /// <param name="repository">The <see cref="IBasicRepository{Employee}"/> repository to work with.</param>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> for dependency resolution.</param>
        public EmployeesController(IBasicRepository<Employee> repository, IServiceProvider serviceProvider)
        {
            _repository = repository;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Get a <see cref="Employee"/>.
        /// </summary>
        /// <example>GET /api/employees/1?forUpdate=true</example>
        /// <param name="id">The id of the <see cref="Employee"/>.</param>
        /// <param name="forUpdate"><c>True</c> to fetch additional concurrency info.</param>
        /// <returns>An <see cref="IActionResult"/>.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, [FromQuery] bool forUpdate = false)
        {
            if (id < 1)
            {
                return new NotFoundResult();
            }

            if (forUpdate)
            {
                var unitOfWork = _serviceProvider.GetService<IUnitOfWork<Employee>>();
                HttpContext.Response.RegisterForDispose(unitOfWork);
                var result = await unitOfWork.Repository.LoadAsync(id, User, true);

                // return version for tracking on client. It's not part of the C# class so it is tracked as a shadow property
                var concurrencyResult = new EmployeeConcurrencyResolver
                {
                    OriginalEmployee = result,
                    RowVersion = result == null
                        ? null
                        : await unitOfWork.Repository.GetPropertyValueAsync<byte[]>(result, FilippSystemContext.RowVersion)
                };
                return new OkObjectResult(concurrencyResult);
            }
            else
            {
                var result = await _repository.LoadAsync(id, User);
                return result == null ? (IActionResult) new NotFoundResult() : new OkObjectResult(result);
            }
        }

        /// <summary>
        /// Add a new <see cref="Employee"/>.
        /// </summary>
        /// <example>POST /api/employees</example>
        /// <param name="employee"></param>
        /// <returns>The <see cref="Employee"/> with id.</returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Employee employee)
        {
            return employee == null
                ? new BadRequestResult()
                : ModelState.IsValid
                    ? new OkObjectResult(await _repository.AddAsync(employee, User))
                    : (IActionResult) new BadRequestObjectResult(ModelState);
        }

        /// <summary>
        /// Update a <see cref="Employee"/>.
        /// </summary>
        /// <example>PUT /api/employees/1</example>
        /// <param name="id">The id of the <see cref="Employee"/>.</param>
        /// <param name="value">The <see cref="EmployeeConcurrencyResolver"/> payload.</param>
        /// <returns>An <see cref="IActionResult"/> of OK or Conflict.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] EmployeeConcurrencyResolver value)
        {
            if (value == null || value.OriginalEmployee == null || value.OriginalEmployee.EmployeeID != id)
            {
                return new BadRequestResult();
            }

            if (ModelState.IsValid)
            {
                var unitOfWork = _serviceProvider.GetService<IUnitOfWork<Employee>>();
                HttpContext.Response.RegisterForDispose(unitOfWork);
                unitOfWork.SetUser(User);
                // get the employee on the board for EF Core
                unitOfWork.Repository.Attach(value.OriginalEmployee);
                await unitOfWork.Repository.SetOriginalValueForConcurrencyAsync(
                    value.OriginalEmployee, FilippSystemContext.RowVersion, value.RowVersion);
                try
                {
                    await unitOfWork.CommitAsync();
                    return new OkResult();
                }
                catch (RepositoryConcurrencyException<Employee> dbException)
                {
                    // it has been updated, send back the database version
                    // and the new RowVersion in case the user wants to override
                    value.DatabaseEmployee = dbException.DbEntity;
                    value.RowVersion = dbException.RowVersion;
                    return new ConflictObjectResult(value);
                }
            }

            return new BadRequestObjectResult(ModelState);
        }

        /// <summary>
        /// Delete a <see cref="Employee"/>.
        /// </summary>
        /// <param name="id">The id of the <see cref="Employee"/> to delete.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var result = await _repository.DeleteAsync(id, User);
                return result
                    ? new OkResult()
                    : (IActionResult) new NotFoundResult();
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception);
            }
        }
    }
}
