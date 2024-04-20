using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Sources.Scripts.Core;
using Sources.Scripts.DI;

namespace Tests.DITests
{
    [TestFixture] // Test suit
    public class ContainerUnitTests
    {
        private readonly Container _container = ContainerTests.Container;
        private readonly TestObject _testObjectInstance = ContainerTests.TestObjectInstance;
        private readonly Mock<IBindableConfig> _bindableConfigWithRecord = ContainerTests.BindableConfigWithRecord;

        private readonly Mock<IBindableConfig>
            _bindableConfigWithEmptyDict = ContainerTests.BindableConfigWithEmptyDict;


        [SetUp] // Before tests
        public void SetUp()
        {
            _bindableConfigWithRecord
                .Setup(x => x.GetBindingsList())
                .Returns(new Dictionary<Type, Type> { { typeof(TestObject), typeof(TestObject) } });
            _bindableConfigWithEmptyDict
                .Setup(x => x.GetBindingsList())
                .Returns(new Dictionary<Type, Type>());
        }


        [Test]
        public void BindOverloads_NullArgument_ThrowException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                () => _container.BindAsync<TestObject>(instance: null),
                message: "Bind with instance");
            Assert.ThrowsAsync<ArgumentNullException>(
                () => _container.BindAsync(null),
                message: "Bind config");
        }

        [Test]
        public void BindOverloads_NotNullArgument_DoesNotThrowException()
        {
            Assert.DoesNotThrowAsync(
                () => _container.BindAsync(_testObjectInstance),
                message: "Bind with instance");

            Assert.DoesNotThrowAsync(
                () => _container.BindAsync(_bindableConfigWithRecord.Object),
                message: "Bind config");
        }
    }
}