using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Sources.Scripts.Core;
using Sources.Scripts.Core.Config;
using Sources.Scripts.DI;

namespace Tests.ContainerTests
{
    [TestFixture]
    public class ContainerTests
    {
        public static readonly MyContainer MyContainer = new MyContainer();
        public static readonly TestObject TestObjectInstance = new();
        public static readonly Mock<IBindableConfiguration> BindableConfigWithRecord = new();
        public static readonly Mock<IBindableConfiguration> BindableConfigWithEmptyDict = new();

        [SetUp] // Before tests
        public void SetUp()
        {
            BindableConfigWithRecord
                .Setup(x => x.GetBindings())
                .Returns(new Dictionary<Type, Type> { { typeof(TestObject), typeof(TestObject) } });
            
            BindableConfigWithEmptyDict
                .Setup(x => x.GetBindings())
                .Returns(new Dictionary<Type, Type>());
        }

        [Test]
        public void BindOverloads_NullArgument_ThrowException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                () => MyContainer.BindAsync<TestObject>(instance: null),
                message: "Bind with instance");
            Assert.ThrowsAsync<ArgumentNullException>(
                () => MyContainer.BindConfigAsync(null),
                message: "Bind config");
        }

        [Test]
        public void BindOverloads_NotNullArgument_DoesNotThrowException()
        {
            Assert.DoesNotThrowAsync(
                () => MyContainer.BindAsync(TestObjectInstance),
                message: "Bind with instance");

            Assert.DoesNotThrowAsync(
                () => MyContainer.BindAsync(BindableConfigWithRecord.Object),
                message: "Bind config");
        }

        [Test]
        public void BindOverloads_ExpectedReturnResults()
        {
            var genericOne = MyContainer.BindAsync<TestObject>().Result.GetType();
            var genericTwo = MyContainer.BindAsync<TestObject, TestObject>().Result.GetType();
            var genericPlusInstance = MyContainer.BindAsync(TestObjectInstance).Result.GetType();
            var bindWithConfig = MyContainer.BindAsync(BindableConfigWithRecord.Object);

            Assert.True(genericOne == typeof(TestObject), message: "generic 1");
            Assert.True(genericTwo == typeof(TestObject), message: "generic 2");
            Assert.True(genericPlusInstance == typeof(TestObject), message: "generic 1 + instance");
            Assert.True(bindWithConfig.IsCompleted, message: "dictionary");
        }

        [Test]
        public async Task GetService_ExpectedReturnResults()
        {
            // Arrange
            await MyContainer.BindAsync<TestObject>();

            // Act
            var genericOne = MyContainer.GetServiceAsync<TestObject>().Result;

            // Assert
            Assert.True(genericOne.GetType() == typeof(TestObject));
        }
    }
}