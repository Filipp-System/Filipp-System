using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calculator.BaseRepository;
using Calculator.Models;
using Calculator.Models.DatabaseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Calculator.Server.Controllers
{
    /// <summary>
    /// Controller for queries of <see cref="Employee"/>.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QueryController : ControllerBase
    {
        private readonly IBasicRepository<Employee> _repository;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Creates a new instance of the <see cref="QueryController"/>.
        /// </summary>
        /// <param name="repository">The <see cref="IBasicRepository{Employee}"/> repository to use.</param>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> for dependency resolution.</param>
        public QueryController(IBasicRepository<Employee> repository, IServiceProvider serviceProvider)
        {
            _repository = repository;
            _serviceProvider = serviceProvider;
        }
    }
}
