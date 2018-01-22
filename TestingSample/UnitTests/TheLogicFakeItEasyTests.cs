using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeItEasy;
using MyLogic;

namespace UnitTests
{
    [TestClass]
    public class TheLogicFakeItEasyTests
    {
        private IRepository _repository = A.Fake<IRepository>();
        private ILogger _logger = A.Fake<ILogger>();

        [TestInitialize]
        public void Setup()
        {
            //_repositoryMock.Setup(m => m.LoadFactor("A"))
            A.CallTo(() => _repository.LoadFactor(A<string>.Ignored))
                           .Returns(2);
            A.CallTo(() => _repository.LoadFactorAsync(A<string>.Ignored))
                           .Returns(Task.FromResult(2.0));
        }

        [TestMethod]
        public void Logic_With30_Test()
        {
            // arrange
            var logic = new TheLogic(_repository, _logger);

            // act
            double result = logic.Calc(30);

            // assert
            Assert.AreEqual(60, result, "fail to check 30");
            A.CallTo(() => _logger.Log(SeverityLevel.Info, A<string>.Ignored))
                                        .MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => _logger.Log(SeverityLevel.Error, A<string>.Ignored))
                                        .MustHaveHappened(Repeated.Never);
        }

        [TestMethod]
        public async Task LogicAsync_With30_Test()
        {
            // arrange
            var logic = new TheLogic(_repository, _logger);

            // act
            double result = await logic.CalcAsync(30).ConfigureAwait(false);

            // assert
            Assert.AreEqual(60, result, "fail to check 30");
            A.CallTo(() => _logger.Log(SeverityLevel.Info, A<string>.Ignored))
                                        .MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => _logger.Log(SeverityLevel.Error, A<string>.Ignored))
                                        .MustHaveHappened(Repeated.Never);
        }

        [TestMethod]
        //[ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Logic_Throw_Test()
        {
            // arrange
            var logic = new TheLogic(_repository, _logger);
            A.CallTo(() => _logger.Log(SeverityLevel.Info, A<string>.Ignored))
                        .Throws<ArgumentOutOfRangeException>();

            try
            {
                double result = logic.Calc(30);
                throw new Exception();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                // exception
            }
            // act

            // assert
            A.CallTo(() => _logger.Log(SeverityLevel.Info, A<string>.Ignored))
                                        .MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => _logger.Log(SeverityLevel.Error, A<string>.Ignored))
                                        .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
