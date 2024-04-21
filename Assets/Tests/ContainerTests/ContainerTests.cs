using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Sources.Scripts.Core;
using Sources.Scripts.DI;

namespace Tests.ContainerTests
{
    [TestFixture]
    public class ContainerTests
    {
        public static readonly Container Container = Container.I;
        public static readonly TestObject TestObjectInstance = new();
        public static readonly Mock<IBindableConfig> BindableConfigWithRecord = new();
        public static readonly Mock<IBindableConfig> BindableConfigWithEmptyDict = new();

        [SetUp] // Before tests
        public void SetUp()
        {
            BindableConfigWithRecord
                .Setup(x => x.GetBindingsList())
                .Returns(new Dictionary<Type, Type> { { typeof(TestObject), typeof(TestObject) } });
            
            BindableConfigWithEmptyDict
                .Setup(x => x.GetBindingsList())
                .Returns(new Dictionary<Type, Type>());
        }

        [Test]
        public void BindOverloads_NullArgument_ThrowException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                () => Container.BindAsync<TestObject>(instance: null),
                message: "Bind with instance");
            Assert.ThrowsAsync<ArgumentNullException>(
                () => Container.BindAsync(null),
                message: "Bind config");
        }

        [Test]
        public void BindOverloads_NotNullArgument_DoesNotThrowException()
        {
            Assert.DoesNotThrowAsync(
                () => Container.BindAsync(TestObjectInstance),
                message: "Bind with instance");

            Assert.DoesNotThrowAsync(
                () => Container.BindAsync(BindableConfigWithRecord.Object),
                message: "Bind config");
        }

        [Test]
        public void BindOverloads_ExpectedReturnResults()
        {
            var genericOne = Container.BindAsync<TestObject>().Result.GetType();
            var genericTwo = Container.BindAsync<TestObject, TestObject>().Result.GetType();
            var genericPlusInstance = Container.BindAsync(TestObjectInstance).Result.GetType();
            var bindWithConfig = Container.BindAsync(BindableConfigWithRecord.Object);

            Assert.True(genericOne == typeof(TestObject), message: "generic 1");
            Assert.True(genericTwo == typeof(TestObject), message: "generic 2");
            Assert.True(genericPlusInstance == typeof(TestObject), message: "generic 1 + instance");
            Assert.True(bindWithConfig.IsCompleted, message: "dictionary");
        }

        [Test]
        public async Task GetService_ExpectedReturnResults()
        {
            // Arrange
            await Container.BindAsync<TestObject>();

            // Act
            var genericOne = Container.GetServiceAsync<TestObject>().Result;

            // Assert
            Assert.True(genericOne.GetType() == typeof(TestObject));
        }
    }
}