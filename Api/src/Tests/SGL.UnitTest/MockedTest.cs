using System;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;

namespace SGL.UnitTest
{
    public abstract class MockedTest<T> : IDisposable
        where T : class
    {
        protected T ClassUnderTest => Mocker.Create<T>();

        protected AutoMock Mocker { get; }

        protected MockedTest()
        {
            Mocker = AutoMock.GetLoose();
        }

        protected Mock<TDepend> GetMock<TDepend>()
            where TDepend : class
        {
            return Mocker.Mock<TDepend>();
        }
    
        public void Dispose()
        {
            Mocker?.Dispose();
        }
    }
}
