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
        private readonly UnitOfWork<FilippSystemContext, Calculation> _unitOfWork;
        private DbContextOptions<FilippSystemContext> _contextOptions;
        public CreateCalculationTests(IBasicRepository<Calculation> calculationRepository, UnitOfWork<FilippSystemContext, Calculation> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _contextOptions = new DbContextOptionsBuilder<FilippSystemContext>()
                .UseSqlite("Filename=Calculation-Test.db").Options;
        }
        
        [Fact]
        public void ListSingleCalculationTest()
        {
            // Arrange

            
            var testCalculationId = 1;
            
            // Act
            using (var context = new FilippSystemContext(_contextOptions))
            {
                var testController = new CalculationController(_unitOfWork.Repository);
                var calculation = testController.GetCalculation(testCalculationId);
            }
            //var result = testController.GetCalculation(testCalculationId);

            // Assert
            Assert.Equal(testCalculationId, result);
        }
    }
}
