using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Sources.Scripts.DI;

namespace Tests.ContainerTests
{
    [TestFixture]
    public class ContainerCacheTests
    {
        private readonly ContainerCache _cache = new Mock<ContainerCache>().Object;
        private static readonly TestObject TestObject = new();
        private static readonly Dictionary<Type, Type> EmptyDict = new();

        private static readonly Dictionary<Type, Type> OneRecordDict = new()
            { { typeof(TestObject), TestObject.GetType() } };

        [Test]
        public void Add_WithNullArgument_1Arg_ThrowException()
        {
            Assert.Throws<ArgumentNullException>(
                () => _cache.Add<TestObject>(null), message: "With instance");

            Assert.ThrowsAsync<ArgumentNullException>(
                () => _cache.Add(dictionary: null), message: "With dict");
        }


        [Test]
        public void Add_WithNullArgument_2Arg_ThrowException()
        {
            Assert.Throws<ArgumentNullException>(
                () => _cache.Add(null, null), message: $"Type and instance null");

            Assert.Throws<ArgumentNullException>(
                () => _cache.Add(null, TestObject), message: $"Type null");

            Assert.Throws<ArgumentNullException>(
                () => _cache.Add(TestObject.GetType(), null), message: $"Instance null");
        }

        [Test]
        public void Add_WithCorrectArgument_1And2Arg_DoesNotThrowException()
        {
            Assert.DoesNotThrow(
                () => _cache.Add<TestObject>(TestObject), message: $"Instance arg");

            Assert.DoesNotThrowAsync(
                () => _cache.Add(EmptyDict), message: $"Dictionary arg");

            Assert.DoesNotThrow(
                () => _cache.Add(typeof(TestObject), TestObject), message: $"Type and instance arg");

            Assert.DoesNotThrow(
                () => _cache.Add(typeof(TestObject), TestObject), message: $"Type and instance arg");
        }
    }
}