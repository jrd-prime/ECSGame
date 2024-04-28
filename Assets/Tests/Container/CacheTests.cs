using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Sources.Scripts.DI;

namespace Tests.Container
{
    [TestFixture]
    public class ContainerCacheTests
    {
        private Cache _cache;
        private object _testObject;
        private Dictionary<Type, Type> _emptyDict;

        [SetUp]
        public void SetUp()
        {
            _cache = new Mock<Cache>().Object;
            _testObject = new TestObject();
            _emptyDict = new Dictionary<Type, Type>();
        }

        // [Test]
        // public void Add_WithNullArgument_1Arg_ThrowException()
        // {
        //     Assert.Throws<ArgumentNullException>(() => _cache.Add<TestObject>(null),
        //         message: "With instance");
        //
        //     Assert.ThrowsAsync<ArgumentNullException>(() => _cache.Add(dictionary: null),
        //         message: "With dict");
        // }


        // [Test]
        // public void Add_WithNullArgument_2Arg_ThrowException()
        // {
        //     Assert.Throws<ArgumentNullException>(() => _cache.Add(null, null),
        //         message: $"Type and instance null");
        //
        //     Assert.Throws<ArgumentNullException>(() => _cache.Add(null, in _testObject),
        //         message: $"Type null");
        //
        //     Assert.Throws<ArgumentNullException>(() => _cache.Add(_testObject.GetType(), null),
        //         message: $"Instance null");
        // }

        // [Test]
        // public void Add_WithCorrectArgument_1And2Arg_DoesNotThrowException()
        // {
        //     Assert.DoesNotThrow(() => _cache.Add<TestObject>(_testObject),
        //         message: $"Instance arg");
        //
        //     Assert.DoesNotThrowAsync(() => _cache.Add(_emptyDict),
        //         message: $"Dictionary arg");
        //
        //     Assert.DoesNotThrow(() => _cache.Add(typeof(TestObject), _testObject),
        //         message: $"Type and instance arg");
        //
        //     Assert.DoesNotThrow(() => _cache.Add(typeof(TestObject), _testObject),
        //         message: $"Type and instance arg");
        // }
    }
}