using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calculator.BaseRepository;
using Calculator.DataAccess;
using Calculator.Models.DatabaseModels;
using Calculator.Repository;
using Calculator.Server.Controllers;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Calculator.Workflow.Tests
{
    public class CreateCalculationTests
    {
        private readonly FilippSystemContext _filippSystemContext;
        private readonly IBasicRepository<Calculation> _calculationRepository;

        private readonly UnitOfWork<FilippSystemContext, Calculation> _unitOfWork;
        public CreateCalculationTests(IBasicRepository<Calculation> calculationRepository, UnitOfWork<FilippSystemContext, Calculation> unitOfWork)
        {
            _calculationRepository = calculationRepository;
            _unitOfWork = unitOfWork;
            var contextOptions = new DbContextOptionsBuilder<FilippSystemContext>()
                .UseSqlite("Filename=Calculation-Test.db").Options;

            _filippSystemContext = new FilippSystemContext(contextOptions);
            
        }
        
        [Fact]
        public void ListSingleCalculationTest()
        {
            // Arrange

            var testController = new CalculationController(_calculationRepository);
            var testCalculationId = 1;

            var calculation = _unitOfWork.Repository.LoadAsync(testCalculationId, _filippSystemContext.User);
            // Act
            //var result = testController.GetCalculation(testCalculationId);

            // Assert
            Assert.Equal(testCalculationId, calculation.Result.CalculationID);
        }
    }
}
