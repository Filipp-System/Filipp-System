using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calculator.BaseRepository;
using Calculator.Models.DatabaseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Calculator.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculationController : ControllerBase
    {
        private readonly IBasicRepository<Calculation> _repository;
        private readonly IServiceProvider _serviceProvider;

        public CalculationController(IBasicRepository<Calculation> repository, IServiceProvider serviceProvider)
        {
            _repository = repository;
            _serviceProvider = serviceProvider;
        }

        public async Task<IActionResult> GetCalculation(int calculationId)
        {
            var result = await _repository.LoadAsync(calculationId, User);
            
            return Accepted(result);
        }
    }
}
