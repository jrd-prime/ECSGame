using System;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using ECSGame.Scripts.Core.Annotation;
using ECSGame.Scripts.Core.DI.Interface;

using UnityEngine;
using UnityEngine.Assertions;
using Exception = System.Exception;
using Random = System.Random;

namespace ECSGame.Scripts.Core.DI
{
    public class DefaultContainerProvider : IContainerProvider
    {
        private IMyContainer _container;
        private IBinder _binder;
        private ICache _cache;
        private IInjector _injector;
        private IServiceFactory _serviceFactory;

        private IContainerConfig _config;
        private IMyContainerFactory _factory;

        private readonly AppContext _context = AppContext.Instance;
        private readonly Dictionary<Type, object> _tempCache = new();

        public IMyContainer GetContainer() => _container;

        public async UniTask Load()
        {
            _config = _context.ConfigManager.GetConfiguration<IContainerConfig>()
                      ?? throw new NullReferenceException();

            var factoryImplType = _config.Impl[typeof(IMyContainerFactory)]
                                  ?? throw new KeyNotFoundException();

            _factory = Activator.CreateInstance(factoryImplType) as IMyContainerFactory;

            await CheckConfigForCriticalImplementations();
            await CreateInstances();
            await SetTemporaryCache(new List<object> { _container, _binder, _cache, _injector, _serviceFactory });
            await InjectDependencies();
        }
        
        private async UniTask InjectDependencies()
        {
            await InjectFor(_container);
            await InjectFor(_cache);
            await InjectFor(_injector);
        }

        private async UniTask CreateInstances()
        {
            _container = _factory.GetInstance<IMyContainer>(_config.Impl[typeof(IMyContainer)]);
            _binder = _factory.GetInstance<IBinder>(_config.Impl[typeof(IBinder)]);
            _cache = _factory.GetInstance<ICache>(_config.Impl[typeof(ICache)]);
            _injector = _factory.GetInstance<IInjector>(_config.Impl[typeof(IInjector)]);
            _serviceFactory = _factory.GetInstance<IServiceFactory>(_config.Impl[typeof(IServiceFactory)]);
            await UniTask.CompletedTask;
        }

        private async UniTask SetTemporaryCache(List<object> objects)
        {
            if (objects.Count == 0) throw new Exception(Messages.TempCacheNotSetErrMsg);

            foreach (var obj in objects) _tempCache.TryAdd(obj.GetType(), obj);
            await UniTask.CompletedTask;
        }

        private async UniTask InjectFor<TTarget>(TTarget target) where TTarget : class, IFieldsInjectable
        {
            var fields = target.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                if (!Attribute.IsDefined(field, typeof(JManualInject))) continue;

                Type implType = _config.Impl[field.FieldType];

                if (implType == _tempCache[implType].GetType())
                    field.SetValue(target, _tempCache[implType]);
            }

            var r = new Random();
            var t = r.Next(500, 1000);
            Debug.LogWarning($"FakeDelay for {target.GetType()}. Sec: {t}");
            await UniTask.Delay(t);

            // Check after inject
            Assert.IsTrue(target.IsFieldsInjected());
        }

        private async UniTask CheckConfigForCriticalImplementations()
        {
            if (_config == null) throw new ArgumentNullException(Messages.ConfigNullErrMsg);

            Assert.IsTrue(
                _config.Impl.ContainsKey(typeof(IMyContainer)),
                $"{Messages.ContainerImplErrMsg} {typeof(IMyContainer)}");
            Assert.IsTrue(
                _config.Impl.ContainsKey(typeof(IBinder)),
                $"{Messages.ContainerImplErrMsg} {typeof(IBinder)}");
            Assert.IsTrue(
                _config.Impl.ContainsKey(typeof(ICache)),
                $"{Messages.ContainerImplErrMsg} {typeof(ICache)}");
            Assert.IsTrue(
                _config.Impl.ContainsKey(typeof(IInjector)),
                $"{Messages.ContainerImplErrMsg} {typeof(IInjector)}");
            Assert.IsTrue(
                _config.Impl.ContainsKey(typeof(IServiceFactory)),
                $"{Messages.ContainerImplErrMsg} {typeof(IServiceFactory)}");

            await UniTask.CompletedTask;
        }
    }
}