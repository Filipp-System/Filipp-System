using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calculator.BaseRepository;
using Calculator.Client.Data;
using Calculator.DataAccess;
using Calculator.Model;
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
        /// <param name="repository">The <see cref="IBasicRepository{Contact}"/> repo to work with.</param>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> for dependency resolution.</param>
        public EmployeesController(IBasicRepository<Employee> repository, IServiceProvider serviceProvider)
        {
            _repository = repository;
            _serviceProvider = serviceProvider;
        }

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
                        : await unitOfWork.Repository.GetPropertyValueAsync<byte[]>(result, EmployeeContext.RowVersion)
                };
                return new OkObjectResult(concurrencyResult);
            }
            else
            {
                var result = await _repository.LoadAsync(id, User);
                return result == null ? (IActionResult) new NotFoundResult() : new OkObjectResult(result);
            }
        }

        // TODO: go on here
    }
}
