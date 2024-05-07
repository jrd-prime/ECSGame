using System;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using ECSGame.Scripts.Core.Annotation;
using ECSGame.Scripts.Core.Config;
using ECSGame.Scripts.Core.DI.Config.Const;
using ECSGame.Scripts.Core.DI.Interface;
using ECSGame.Scripts.State.Loading;
using UnityEngine.Assertions;
using Exception = System.Exception;

namespace ECSGame.Scripts.Core.DI.Initializer
{
    public class DefaultContainerInitializer : IContainerInitializer
    {
        private readonly IMyContainer _container;
        private IBinder _binder;
        private ICache _cache;
        private IInjector _injector;
        private IServiceFactory _serviceFactory;

        private readonly Dictionary<Type, Type> _containerConfig;
        private IContainerFactory _factory;
        private readonly AppContext _context = AppContext.Instance;
        private readonly Dictionary<Type, object> _tempCache = new();

        public DefaultContainerInitializer()
        {
            var config = ConfigManager.Instance.GetConfiguration<IContainerConfig>()
                         ?? throw new NullReferenceException();
            _containerConfig = config.GetConfig()
                               ?? throw new NullReferenceException();
            _container = Activator.CreateInstance(_containerConfig[typeof(IMyContainer)]) as IMyContainer;
        }

        public IMyContainer GetContainer() => _container;

        public async UniTask Load(Action<ILoadable> action)
        {
            action.Invoke(this);

            _factory = _context.providersFactory.GetProvidedInstance<IContainerFactory>();

            await CheckConfigForCriticalImplementations();
            await CreateInstances();
            await SetTemporaryCache(new List<object> { _container, _binder, _cache, _injector, _serviceFactory });
            await InjectDependencies();

            await InitializeDI.Init(_container);
        }

        private async UniTask InjectDependencies()
        {
            await InjectFor(_container);
            await InjectFor(_cache);
            await InjectFor(_injector);
        }

        private async UniTask CreateInstances()
        {
            _binder = _factory.GetInstance<IBinder>(_containerConfig[typeof(IBinder)]);
            _cache = _factory.GetInstance<ICache>(_containerConfig[typeof(ICache)]);
            _injector = _factory.GetInstance<IInjector>(_containerConfig[typeof(IInjector)]);
            _serviceFactory = _factory.GetInstance<IServiceFactory>(_containerConfig[typeof(IServiceFactory)]);
            await UniTask.CompletedTask;
        }

        private async UniTask SetTemporaryCache(List<object> objects)
        {
            if (objects.Count == 0) throw new Exception(Messages.TempCacheNotSetErrMsg);

            foreach (var obj in objects)
            {
                _tempCache.TryAdd(obj.GetType(), obj);
            }

            await UniTask.CompletedTask;
        }

        private async UniTask InjectFor<TTarget>(TTarget target) where TTarget : class, IFieldsInjectable
        {
            var fields = target.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                if (!Attribute.IsDefined(field, typeof(JManualInject))) continue;

                Type implType = _containerConfig[field.FieldType];

                if (implType == _tempCache[implType].GetType())
                    field.SetValue(target, _tempCache[implType]);
            }

            // Check after inject
            Assert.IsTrue(target.IsFieldsInjected());

            await UniTask.CompletedTask;
        }

        private async UniTask CheckConfigForCriticalImplementations()
        {
            if (_containerConfig == null) throw new ArgumentNullException(Messages.ConfigNullErrMsg);

            Assert.IsTrue(
                _containerConfig.ContainsKey(typeof(IMyContainer)),
                $"{Messages.ContainerImplErrMsg} {typeof(IMyContainer)}");
            Assert.IsTrue(
                _containerConfig.ContainsKey(typeof(IBinder)),
                $"{Messages.ContainerImplErrMsg} {typeof(IBinder)}");
            Assert.IsTrue(
                _containerConfig.ContainsKey(typeof(ICache)),
                $"{Messages.ContainerImplErrMsg} {typeof(ICache)}");
            Assert.IsTrue(
                _containerConfig.ContainsKey(typeof(IInjector)),
                $"{Messages.ContainerImplErrMsg} {typeof(IInjector)}");
            Assert.IsTrue(
                _containerConfig.ContainsKey(typeof(IServiceFactory)),
                $"{Messages.ContainerImplErrMsg} {typeof(IServiceFactory)}");

            await UniTask.CompletedTask;
        }
    }
}