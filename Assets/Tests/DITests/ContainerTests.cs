using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Sources.Scripts.Core;
using Sources.Scripts.DI;

namespace Tests.DITests
{
    [TestFixture] // Test suit
    public class ContainerTests
    {
        private static readonly Container Container = new();
        private readonly TestObject _testObject = new();
        private readonly Mock<IBindableConfig> _bindableConfig = new();

        private class TestObject
        {
        }

        [SetUp] // Before tests
        public void SetUp()
        {
            _bindableConfig
                .Setup(x => x.GetBindingsList())
                .Returns(new Dictionary<Type, Type>());
        }

        [Test]
        public void Negative_BindOverloads_NullArgument_ThrowException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                () => Container.Bind<TestObject>(null),
                message: "Bind with instance");

            Assert.ThrowsAsync<ArgumentNullException>(
                () => Container.Bind(null),
                message: "Bind config");
        }

        [Test]
        public void Positive_BindOverloads_NotNullArgument_DoesNotThrowException()
        {
            Assert.DoesNotThrowAsync(
                () => Container.Bind<TestObject>(_testObject),
                message: "Bind with instance");

            Assert.DoesNotThrowAsync(
                () => Container.Bind(_bindableConfig.Object),
                message: "Bind config");
        }
    }
}