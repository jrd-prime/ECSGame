using System;
using System.Collections;
using NUnit.Framework;
using Sources.Scripts.DI;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.DITests
{
    [TestFixture] // Test suit
    public class ContainerTests
    {
        private Container _container = new();
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
        public void Container_Bind_OverloadWithInstance_Null_Argument()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _container.Bind<TestObject>(null));
        }


        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator ContainerTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}