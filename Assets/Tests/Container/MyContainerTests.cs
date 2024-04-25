//
//
// using System;
// using System.Collections.Generic;
// using System.Reflection;
// using System.Threading.Tasks;
// using Moq;
// using NUnit.Framework;
// using Sources.Scripts.Core.Config;
// using Sources.Scripts.DI;
// using Sources.Scripts.DI.Interface;
// using Sources.Scripts.Factory;
// using Sources.Scripts.Utils;
// using Tests.Container.TestAssembly;
//
// namespace Tests.Container
// {
//     [TestFixture]
//     [Category(nameof(MyContainerTests))]
//     public class MyContainerTests
//     {
//         #region Vars
//
//         private IMyContainer _container;
//
//         private ContainerBinder _binder;
//         private ContainerCache _cache;
//         private ContainerInjector _injector;
//         private IConfiguration _configuration;
//
//         public static readonly TestObject TestObjectInstance = new();
//
//         private TestObject _testObject;
//         private TestImpl _testImpl;
//         private InjectResult _stubInjectResult;
//
//         private IMyContainer _stubVerifyBindMethods;
//         private IMyContainer _stubInjectMethods;
//
//         #endregion
//
//         [SetUp]
//         public async void SetUp()
//         {
//             _testObject = new TestObject();
//             _testImpl = new TestImpl();
//             _configuration = new TestConfigWithRecord();
//
//             // Container init
//             {
//                 _container = new MyContainer();
//
//                 Dictionary<Type, object> instances = await ReflectionUtils.ManualInjectAsync(_container);
//
//                 _binder = instances[typeof(ContainerBinder)] as ContainerBinder;
//                 _cache = instances[typeof(ContainerCache)] as ContainerCache;
//                 _injector = instances[typeof(ContainerInjector)] as ContainerInjector;
//
//                 var serviceFactory = ServiceFactoryManager.I.GetCurrentFactory();
//
//                 await ReflectionUtils.ManualInjectWithInstanceAsync(instances[typeof(ContainerCache)], serviceFactory);
//                 await ReflectionUtils.ManualInjectWithInstanceAsync(instances[typeof(ContainerInjector)], _container);
//                 await ReflectionUtils.ManualInjectWithInstanceAsync(instances[typeof(ContainerInjector)], _cache);
//             }
//         }
//
//         #region GET
//
//         [Test]
//         public void GetService_PositiveCases_ReturnCorrectObjects()
//         {
//             var oneGeneric = _container.GetServiceAsync<TestObject>();
//
//             Assert.IsInstanceOf<TestObject>(oneGeneric.Result);
//         }
//
//         #endregion
//
//         #region BIND
//
//         [Test]
//         public void Bind_PositiveCases_ReturnCorrectObjects()
//         {
//             // Act
//             var oneGeneric = _container.BindAsync<TestObject>();
//             var twoGeneric = _container.BindAsync<TestBase, TestImpl>();
//             var oneGenericWithInstance = _container.BindAsync<TestObject>(_testObject);
//             var twoGenericWithInstance = _container.BindAsync<TestBase, TestImpl>(_testImpl);
//             var withConfig = _container.BindConfigAsync(_configuration);
//
//             // Assert
//             Assert.IsInstanceOf<TestObject>(oneGeneric.Result);
//
//             Assert.IsInstanceOf<TestBase>(twoGeneric.Result);
//             Assert.IsInstanceOf<TestImpl>(twoGeneric.Result);
//
//             Assert.IsInstanceOf(_testObject.GetType(), oneGenericWithInstance.Result);
//
//             Assert.IsInstanceOf<TestBase>(twoGenericWithInstance.Result);
//             Assert.IsInstanceOf<TestImpl>(twoGenericWithInstance.Result);
//             Assert.IsInstanceOf(_testImpl.GetType(), twoGenericWithInstance.Result);
//
//             Assert.DoesNotThrowAsync(() => withConfig);
//         }
//
//         [Test]
//         public void Bind_NullArgument_ThrowException()
//         {
//             Assert.ThrowsAsync<ArgumentNullException>(
//                 () => _container.BindAsync<TestObject>(null),
//                 message: "Bind with instance");
//
//             Assert.ThrowsAsync<ArgumentNullException>(
//                 () => _container.BindAsync<TestBase, TestImpl>(null),
//                 message: "Bind with instanceToImpl");
//
//             Assert.ThrowsAsync<ArgumentNullException>(
//                 () => _container.BindConfigAsync(null),
//                 message: "Bind config");
//         }
//
//
//         [Test]
//         public async Task Bind_BindRecordAddedToBindsCache()
//         {
//             // Arrange
//             var testObject1 = new TestObject1();
//             var testImpl1 = new TestImpl1();
//             var config = Mock.Of<IConfiguration>(x => x.GetBindings() == new Dictionary<Type, Type>
//             {
//                 { typeof(TestImpl), typeof(TestImpl) }
//             });
//
//             // Act
//             _binder.GetBinds().Clear();
//
//             var bindsCountBefore = _binder.GetBinds().Count;
//
//             // await _myContainer.BindAsync<TestObject>();
//             // await _myContainer.BindAsync<TestBase, TestImpl>();
//             // await _myContainer.BindAsync<TestObject1>(testObject1);
//             // await _myContainer.BindAsync<TestBase1, TestImpl1>(testImpl1);
//             // await _myContainer.BindConfigAsync(config);
//
//             await Task.WhenAll(
//                 _container.BindAsync<TestObject>(),
//                 _container.BindAsync<TestBase, TestImpl>(),
//                 _container.BindAsync<TestObject1>(testObject1),
//                 _container.BindAsync<TestBase1, TestImpl1>(testImpl1),
//                 _container.BindConfigAsync(config)
//             );
//
//             var bindsCountAfter = _binder.GetBinds().Count;
//
//             // Assert
//             Assert.True(_binder.GetBinds().TryGetValue(typeof(TestObject), out _));
//             Assert.True(_binder.GetBinds().TryGetValue(typeof(TestBase), out _));
//             Assert.True(_binder.GetBinds().TryGetValue(typeof(TestObject1), out _));
//             Assert.True(_binder.GetBinds().TryGetValue(typeof(TestBase1), out _));
//             Assert.True(bindsCountBefore + 5 == bindsCountAfter);
//         }
//
//         #endregion
//
//         #region CACHE
//
//         [Test]
//         public void Cache_PositiveCases_ObjectAddedToCache()
//         {
//             // Arrange
//             var testObject = _testObject as object;
//             _cache.Clear();
//
//             // Act
//             _container.AddToCache(typeof(TestObject), in testObject);
//             _cache.GetCache().TryGetValue(typeof(TestObject), out var cachedObject);
//
//             // Assert
//             Assert.IsTrue(cachedObject?.Equals(testObject));
//         }
//
//
//         [Test]
//         public void Cache_AddToCache_NullArgument_ThrowException()
//         {
//             Assert.Throws<ArgumentNullException>(
//                 () => _container.AddToCache(null, null),
//                 "Both null");
//
//             Assert.Throws<ArgumentNullException>(
//                 () => _container.AddToCache(null, _testObject),
//                 "Type null");
//
//             Assert.Throws<ArgumentNullException>(
//                 () => _container.AddToCache(typeof(TestObject), null),
//                 "Instance null");
//         }
//
//         #endregion
//
//         #region INJECT
//
//         [Test]
//         public void Inject_NullArgument_ThrowException()
//         {
//             Assert.ThrowsAsync<ArgumentNullException>(
//                 () => _container.InjectServicesAsync(null),
//                 "Assembly null");
//         }
//
//         [Test]
//         public void Inject_CorrectArgument_DoesNotThrowException()
//         {
//             var mockAssembly = new Mock<Assembly>();
//             var stub = Mock.Of<IMyContainer>(x => x.InjectServicesAsync(mockAssembly.Object) == Task.CompletedTask);
//
//             Assert.DoesNotThrowAsync(
//                 () => stub.InjectServicesAsync(mockAssembly.Object),
//                 "Correct assembly");
//         }
//
//         [Test]
//         public async Task Inject_CorrectAssembly_InstanceInjected()
//         {
//             // Arrange
//             // Bind container, add to cache
//             await _container.BindAsync<MyContainer>(_container);
//
//             // Bind test class, create instance and aadd to cache
//             await _container.BindAsync<TestWithInjectField>();
//
//             // Get special test assembly from class with field with inject attr
//             var assembly = Assembly.GetAssembly(typeof(TestWithInjectField));
//
//             // Act
//             // Get service before inject (injected field - private Container)
//             var beforeInject = _container.GetServiceAsync<TestWithInjectField>().Result.GetContainer() == null;
//
//             // Inject in assembly
//             await _container.InjectServicesAsync(assembly);
//
//             // Get service after inject (injected field - private Container)
//             var afterInject = _container.GetServiceAsync<TestWithInjectField>().Result.GetContainer() == null;
//
//             // Assert
//             // Before container null
//             Assert.True(beforeInject as bool?);
//
//             // After container is no null
//             Assert.False(afterInject as bool?);
//         }
//
//         #endregion
//     }
// }