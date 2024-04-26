using System;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using Sources.Scripts.Annotation;
using Sources.Scripts.Core.Config;
using Sources.Scripts.DI.Interface;
using Sources.Scripts.Factory;
using UnityEngine;
using Exception = System.Exception;

namespace Sources.Scripts.DI
{
    public class DefaultContainerProvider : IContainerProvider
    {
        private IMyContainer _container;
        private IBinder _binder;
        private ICache _cache;
        private IInjector _injector;
        private IServiceFactory _serviceFactory;

        private readonly IContainerConfig _config = ConfigManager.I.GetConfiguration<IContainerConfig>();
        private readonly IMyContainerFactory _factory = new DefaultContainerFactory();
        private readonly Dictionary<Type, object> _tempCache = new();

        public async UniTask<IMyContainer> GetContainer() => await InitializeContainer();

        private async UniTask<IMyContainer> InitializeContainer()
        {
            await CheckConfigForCriticalImplementations();
            await CreateInstances();
            await SetTemporaryCache(new List<object>() { _container, _binder, _cache, _injector, _serviceFactory });
            await InjectDependencies();

            return await UniTask.FromResult(_container);
        }

        private async UniTask InjectDependencies()
        {
            await UniTask
                .WhenAll(
                    InjectFor(_container),
                    InjectFor(_cache),
                    InjectFor(_injector)
                );
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

            Debug.LogWarning("delay");
            await UniTask.Delay(2000);

            // Check after inject
            Assert.True(target.IsFieldsInjected());
        }

        private async UniTask CheckConfigForCriticalImplementations()
        {
            if (_config == null) throw new ArgumentNullException(Messages.ConfigNullErrMsg);

            Assert.True(
                _config.Impl.ContainsKey(typeof(IMyContainer)),
                $"{Messages.ContainerImplErrMsg} {typeof(IMyContainer)}");
            Assert.True(
                _config.Impl.ContainsKey(typeof(IBinder)),
                $"{Messages.ContainerImplErrMsg} {typeof(IBinder)}");
            Assert.True(
                _config.Impl.ContainsKey(typeof(ICache)),
                $"{Messages.ContainerImplErrMsg} {typeof(ICache)}");
            Assert.True(
                _config.Impl.ContainsKey(typeof(IInjector)),
                $"{Messages.ContainerImplErrMsg} {typeof(IInjector)}");
            Assert.True(
                _config.Impl.ContainsKey(typeof(IServiceFactory)),
                $"{Messages.ContainerImplErrMsg} {typeof(IServiceFactory)}");

            await UniTask.CompletedTask;
        }
    }
}