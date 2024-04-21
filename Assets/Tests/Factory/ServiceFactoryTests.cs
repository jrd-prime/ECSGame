using System;
using NUnit.Framework;
using Sources.Scripts.Factory;

namespace Tests.Factory
{
    [TestFixture]
    public class ServiceFactoryTests
    {
        private readonly StandardServiceFactory _standardServiceFactory = new();

        [Test]
        public void GetService_WithNullArgument_ThrowException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                () => _standardServiceFactory.CreateServiceAsync(null),
                message: "With type argument");
        }

        [Test]
        public void GetService_WithArgument_ReturnExpected()
        {
            // Act
            var instance = _standardServiceFactory.CreateServiceAsync(typeof(TestObject)).Result;

            // Assert
            Assert.True(instance.GetType() == typeof(TestObject));
        }
    }
}