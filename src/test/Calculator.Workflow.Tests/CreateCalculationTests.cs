using Calculator.BaseRepository;
using Calculator.DataAccess;
using Calculator.Models.DatabaseModels;
using Calculator.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Calculator.Workflow.Tests
{
    public class CreateCalculationTests
    {
        private FilippSystemContext _filippSystemContext;
        private readonly UnitOfWork<FilippSystemContext, Calculation> _unitOfWork;
        private readonly DbContextOptions<FilippSystemContext> _contextOptions;
        public CreateCalculationTests(IBasicRepository<Calculation> calculationRepository, UnitOfWork<FilippSystemContext, Calculation> unitOfWork)
        {
            _contextOptions = new DbContextOptionsBuilder<FilippSystemContext>()
                .UseSqlite("Filename=Calculation-Test.db").Options;
            _unitOfWork = unitOfWork;
        }
        
        [Fact]
        public void Can_Request_Single_Calculation()
        {
            // Arrange
            var testCalculationId = 1;
            _filippSystemContext = new FilippSystemContext(_contextOptions);

            // Act
            var calculation = _unitOfWork.Repository.LoadAsync(testCalculationId, _filippSystemContext.User);

            // Assert
            Assert.Equal(testCalculationId, calculation.Result.CalculationID);
        }
    }
}
