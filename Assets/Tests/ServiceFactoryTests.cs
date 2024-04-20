using System;
using NUnit.Framework;
using Sources.Scripts.Factory;

namespace Tests
{
    [TestFixture]
    public class ServiceFactoryTests
    {
        private readonly ServiceFactory _serviceFactory = new();

        [Test]
        public void GetService_WithNullArgument_ThrowException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                () => _serviceFactory.CreateServiceAsync(null),
                message: "With type argument");
        }

        [Test]
        public void GetService_WithArgument_ReturnExpected()
        {
            // Act
            var instance = _serviceFactory.CreateServiceAsync(typeof(TestObject)).Result;

            // Assert
            Assert.True(instance.GetType() == typeof(TestObject));
        }
    }
}