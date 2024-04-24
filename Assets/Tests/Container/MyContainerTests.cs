using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;
using Sources.Scripts.Core.Config;
using Sources.Scripts.DI;
using Sources.Scripts.Factory;
using Sources.Scripts.Utils;
using Tests.Container.TestAssembly;
using UnityEngine;

namespace Tests.Container
{
    [TestFixture]
    [Category(nameof(MyContainerTests))]
    public class MyContainerTests
    {
        #region Vars

        private MyContainer _myContainer;

        private ContainerBinder _binder;
        private ContainerCache _cache;
        private ContainerInjector _injector;
        private IConfiguration _configuration;

        public static readonly TestObject TestObjectInstance = new();

        private TestObject _testObject;
        private TestImpl _testImpl;
        private InjectResult _stubInjectResult;

        private IMyContainer _stubVerifyBindMethods;
        private IMyContainer _stubInjectMethods;

        #endregion

        [SetUp]
        public async void SetUp()
        {
            _testObject = new TestObject();
            _testImpl = new TestImpl();
            _configuration = new TestConfigWithRecord();

            _myContainer = new MyContainer();

            Dictionary<Type, object> instances = await ReflectionUtils.ManualInjectAsync(_myContainer);

            _binder = instances[typeof(ContainerBinder)] as ContainerBinder;
            _cache = instances[typeof(ContainerCache)] as ContainerCache;
            _injector = instances[typeof(ContainerInjector)] as ContainerInjector;

            var serviceFactory = ServiceFactoryManager.I.GetCurrentFactory();

            await ReflectionUtils.ManualInjectWithInstanceAsync(instances[typeof(ContainerCache)], serviceFactory);
            await ReflectionUtils.ManualInjectWithInstanceAsync(instances[typeof(ContainerInjector)], _myContainer);
            await ReflectionUtils.ManualInjectWithInstanceAsync(instances[typeof(ContainerInjector)], _cache);
        }

        #region GET

        [Test]
        public void GetService_PositiveCases_ReturnCorrectObjects()
        {
            // Act
            var oneGeneric = _myContainer.GetServiceAsync<TestObject>();

            // Assert
            Assert.IsInstanceOf<TestObject>(oneGeneric.Result);
        }

        #endregion

        #region BIND

        [Test]
        public void Bind_PositiveCases_ReturnCorrectObjects()
        {
            // Act
            var oneGeneric = _myContainer.BindAsync<TestObject>();
            var twoGeneric = _myContainer.BindAsync<TestBase, TestImpl>();
            var oneGenericWithInstance = _myContainer.BindAsync<TestObject>(_testObject);
            var twoGenericWithInstance = _myContainer.BindAsync<TestBase, TestImpl>(_testImpl);
            var withConfig = _myContainer.BindConfigAsync(_configuration);

            // Assert
            Assert.IsInstanceOf<TestObject>(oneGeneric.Result);

            Assert.IsInstanceOf<TestBase>(twoGeneric.Result);
            Assert.IsInstanceOf<TestImpl>(twoGeneric.Result);

            Assert.IsInstanceOf(_testObject.GetType(), oneGenericWithInstance.Result);

            Assert.IsInstanceOf<TestBase>(twoGenericWithInstance.Result);
            Assert.IsInstanceOf<TestImpl>(twoGenericWithInstance.Result);
            Assert.IsInstanceOf(_testImpl.GetType(), twoGenericWithInstance.Result);

            Assert.DoesNotThrowAsync(() => withConfig);
        }

        [Test]
        public void Bind_NullArgument_ThrowException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                () => _myContainer.BindAsync<TestObject>(null),
                message: "Bind with instance");

            Assert.ThrowsAsync<ArgumentNullException>(
                () => _myContainer.BindAsync<TestBase, TestImpl>(null),
                message: "Bind with instanceToImpl");

            Assert.ThrowsAsync<ArgumentNullException>(
                () => _myContainer.BindConfigAsync(null),
                message: "Bind config");
        }

        #endregion

        #region CACHE

        [Test]
        public void Cache_PositiveCases_ObjectAddedToCache()
        {
            // Arrange
            var testObject = _testObject as object;
            _cache.Clear();

            // Act
            _myContainer.AddToCache(typeof(TestObject), in testObject);
            _cache.GetCache().TryGetValue(typeof(TestObject), out var cachedObject);

            // Assert
            Assert.IsTrue(cachedObject?.Equals(testObject));
        }


        [Test]
        public void Cache_AddToCache_NullArgument_ThrowException()
        {
            Assert.Throws<ArgumentNullException>(
                () => _myContainer.AddToCache(null, null),
                "Both null");

            Assert.Throws<ArgumentNullException>(
                () => _myContainer.AddToCache(null, _testObject),
                "Type null");

            Assert.Throws<ArgumentNullException>(
                () => _myContainer.AddToCache(typeof(TestObject), null),
                "Instance null");
        }

        #endregion

        #region INJECT

        [Test]
        public void Inject_NullArgument_ThrowException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                () => _myContainer.InjectServicesAsync(null),
                "Assembly null");
        }

        // [Test]
        // public void Inject_CorrectArgument_DoesNotThrowException()
        // {
        //     var assembly = Assembly.GetAssembly(typeof(TestWithInjectField));
        //
        //     Assert.DoesNotThrowAsync(
        //         () => _myContainer.InjectServicesAsync(assembly),
        //         "Correct assembly");
        // }

        [Test]
        public async Task Inject_CorrectAssembly_InstanceInjected()
        {
            // Arrange
            // Bind container, add to cache
            await _myContainer.BindAsync<MyContainer>(_myContainer);

            // Bind test class, create instance and aadd to cache
            await _myContainer.BindAsync<TestWithInjectField>();

            // Get special test assembly from class with field with inject attr
            var assembly = Assembly.GetAssembly(typeof(TestWithInjectField));

            // Act
            // Get service before inject (injected field - private Container)
            var beforeInject = _myContainer.GetServiceAsync<TestWithInjectField>().Result.GetContainer() == null;

            // Inject in assembly
            await _myContainer.InjectServicesAsync(assembly);

            // Get service after inject (injected field - private Container)
            var afterInject = _myContainer.GetServiceAsync<TestWithInjectField>().Result.GetContainer() == null;

            // Assert
            // Before container null
            Assert.True(beforeInject);

            // After container is no null
            Assert.False(afterInject);
        }

        #endregion
    }
}