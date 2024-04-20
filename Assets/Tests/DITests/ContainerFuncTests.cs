using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Sources.Scripts.Core;
using Sources.Scripts.DI;

namespace Tests.DITests
{
    public class ContainerFuncTests
    {
        private readonly Container _container = ContainerTests.Container;
        private readonly TestObject _testObjectInstance = ContainerTests.TestObjectInstance;
        private readonly Mock<IBindableConfig> _bindableConfigWithRecord = ContainerTests.BindableConfigWithRecord;

        [Test]
        public void BindOverloads_ExpectedReturnResults()
        {
            var genericOne = _container.BindAsync<TestObject>().Result.GetType();
            var genericTwo = _container.BindAsync<TestObject, TestObject>().Result.GetType();
            var genericPlusInstance = _container.BindAsync(_testObjectInstance).Result.GetType();
            var bindWithConfig = _container.BindAsync(_bindableConfigWithRecord.Object);

            Assert.True(genericOne == typeof(TestObject));
            Assert.True(genericTwo == typeof(TestObject));
            Assert.True(genericPlusInstance == typeof(TestObject));
            Assert.True(bindWithConfig.IsCompleted);
        }

        [Test]
        public async Task GetService_ExpectedReturnResults()
        {
            // Arrange
            await _container.BindAsync<TestObject>();

            // Act
            var genericOne = _container.GetServiceAsync<TestObject>().Result;

            // Assert
            Assert.True(genericOne.GetType() == typeof(TestObject));
        }
    }
}