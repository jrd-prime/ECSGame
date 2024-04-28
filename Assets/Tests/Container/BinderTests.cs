using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Sources.Scripts.DI;

namespace Tests.Container
{
    // TODO LOOK check if bind not assignable types

    [TestFixture] // Test suit
    public class ContainerBinderTests
    {
        private Binder _binder;

        [SetUp]
        public void SetUp()
        {
            _binder = new Binder();
        }

        // [Test]
        // public void BindOverloads_WithNullArgument_1Arg_ThrowException()
        // {
        //     Assert.Throws<ArgumentNullException>(
        //         () => _binder.Bind(type: null), message: "Bind with type argument");
        //
        //     Assert.Throws<ArgumentNullException>(
        //         () => _binder.Bind(dictionary: null), message: "Bind with dictionary");
        // }

        // [TestCase(null, null, TestName = "Both arguments is null")]
        // [TestCase(null, typeof(TestObject), TestName = "First argument is null")]
        // [TestCase(typeof(TestObject), null, TestName = "Second argument is null")]
        // public void BindOverloads_WithNullArguments_2Arg_ThrowException(Type t1, Type t2)
        // {
        //     Assert.Throws<ArgumentNullException>(() => _binder.BindConfig(t1, t2));
        // }

        // [Test]
        // public void BindOverloads_WithCorrectArgument_1Arg_DoesNotThrowException()
        // {
        //     Assert.DoesNotThrow(
        //         () => _binder.Bind(type: typeof(TestObject)), message: "Bind with type argument");
        //
        //     Assert.DoesNotThrow(
        //         () => _binder.Bind(dictionary: new Dictionary<Type, Type>()), message: "Bind with dictionary");
        // }

        // [Test]
        // public void BindOverloads_WithCorrectArguments_2Arg_DoesNotThrowException()
        // {
        //     Assert.DoesNotThrow(() => _binder.BindConfig(typeof(TestObject), typeof(TestObject)));
        // }
    }
}