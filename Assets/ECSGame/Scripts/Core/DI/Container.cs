using System;
using System.Reflection;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using ECSGame.Scripts.Core.Annotation;
using ECSGame.Scripts.Core.Config.Interface;
using ECSGame.Scripts.Core.DI.Interface;
using ECSGame.Scripts.State.Loading;

namespace ECSGame.Scripts.Core.DI
{
    public class Container : IMyContainer, IDisposable
    {
        [JManualInject] private readonly IBinder _binder;
        [JManualInject] private readonly ICache _cache;
        [JManualInject] private readonly IInjector _injector;

        private object _tempImplInstance;
        private IBindableConfiguration _tempConfig;

        private Action<Type, Type> _bindComplete;
        private Action _bindConfigComplete;

        public Container()
        {
            _bindComplete += OnBindFinished;
            _bindConfigComplete += OnBindConfigFinished;
        }

        #region Callbacks

        private void OnBindFinished(Type baseType, Type implType)
        {
            _cache.Add(baseType, implType, in _tempImplInstance);
            _tempImplInstance = null;
        }

        private void OnBindConfigFinished()
        {
            _cache.Add(_tempConfig);
            _tempConfig = null;
        }

        #endregion

        #region Get

        public T GetService<T>() where T : class => _cache.Get<T>();

        #endregion

        #region Bind

        private void BindMain(Type baseType, Type implType)
        {
            if (baseType == null || implType == null) throw new ArgumentNullException();

            _binder.Bind(baseType, implType, _bindComplete);
        }

        public void BindConfig(in IBindableConfiguration configuration)
        {
            _tempConfig = configuration ?? throw new ArgumentNullException();
            if (_tempConfig.GetBindings().Count != 0)
                _binder.BindConfig(in _tempConfig, _bindConfigComplete);

            _tempConfig = null;
        }

        public void Bind<T>() where T : class => BindMain(typeof(T), typeof(T));

        public void Bind<TBase, TImpl>() where TBase : class where TImpl : TBase
            => BindMain(typeof(TBase), typeof(TImpl));

        public void Bind<T>(in object implInstance) where T : class
        {
            _tempImplInstance = implInstance ?? throw new ArgumentNullException();
            BindMain(typeof(T), implInstance.GetType());
        }

        public void Bind<TBase, TImpl>(in TImpl implInstance) where TBase : class where TImpl : TBase
        {
            _tempImplInstance = implInstance ?? throw new ArgumentNullException();
            BindMain(typeof(TBase), typeof(TImpl));
        }

        #endregion

        #region Inject

        //TODO remove direct call
        public async Task InjectServicesAsync(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException();
            await _injector.InjectDependenciesAsync(assembly);
        }

        #endregion

        #region Service

        public bool IsFieldsInjected() => _binder != null && _cache != null && _injector != null;

        public void Dispose()
        {
            _bindComplete -= OnBindFinished;
            _bindConfigComplete -= OnBindConfigFinished;
        }

        #endregion

        public string Description => "Container";

        public UniTask Load(Action<ILoadable> action)
        {
            throw new NotImplementedException();
        }
    }
}