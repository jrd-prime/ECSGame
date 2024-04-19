using System;
using NUnit.Framework;
using Sources.Scripts.DI;

namespace Tests.DITests
{
    [TestFixture] // Test suit
    public class ContainerBinderTests
    {
        private static ContainerBinder _binder = new();
        private TestObject _testObject = new();

        private class TestObject
        {
        }

        [SetUp] // Before tests
        public void SetUp()
        {
        }

        [TearDown] // After tests
        public void TearDown()
        {
        }

        [Test]
        public void Negative_BindOverloads_WithNullArgument_ThrowException()
        {
            Assert.Throws<ArgumentNullException>(
                () => _binder.Bind(type: null),
                message: "Bind with type argument");

            Assert.Throws<ArgumentNullException>(
                () => _binder.Bind(dictionary: null),
                message: "Bind with dictionary");
        }


        [TestCase(null, null, TestName = "Both arguments is null")]
        [TestCase(null, typeof(TestObject), TestName = "First argument is null")]
        [TestCase(typeof(TestObject), null, TestName = "Second argument is null")]
        public void Negative_BindOverloads_WithNullArguments_ThrowException(Type type1, Type type2)
        {
            Assert.Throws<ArgumentNullException>(() => _binder.Bind(type1, type2));
        }
    }
}