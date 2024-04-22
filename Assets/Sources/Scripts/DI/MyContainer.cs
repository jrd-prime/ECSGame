using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Sources.Scripts.Annotation;
using Sources.Scripts.Core.Config;

namespace Sources.Scripts.DI
{
    public sealed class MyContainer : IMyContainer
    {
        [JManualInject] private readonly ContainerBinder _binder;
        [JManualInject] private readonly  ContainerCache _cache;
        [JManualInject] private readonly  ContainerInjector _injector;

        public async Task<T> GetServiceAsync<T>() where T : class => await _cache.Get<T>();

        public async Task<object> BindAsync<T>() where T : class
        {
            _binder.Bind<T>();
            _cache.Add<T>();
            return await GetServiceAsync<T>();
        }

        public async Task<object> BindAsync<TBase, TImpl>() where TBase : class where TImpl : class
        {
            _binder.Bind<TBase, TImpl>();
            _cache.Add<TBase, TImpl>();
            return await GetServiceAsync<TBase>();
        }

        public async Task<object> BindAsync<T>(T instance) where T : class
        {
            if (instance == null) throw new ArgumentNullException();

            _binder.Bind<T>();
            _cache.Add<T>(instance);
            return await GetServiceAsync<T>();
        }

        public async Task<object> BindAsync<T>(object instance) where T : class
        {
            if (instance == null) throw new ArgumentNullException();

            _binder.Bind<T>(instance.GetType());
            _cache.Add<T>(instance);
            return await GetServiceAsync<T>();
        }

        public async Task BindConfigAsync(IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException();

            Dictionary<Type, Type> bindingsList = configuration.GetBindings();

            _binder.Bind(bindingsList);
            await _cache.Add(bindingsList);
        }

        public void AddToCache(Type type, object instance) => _cache.Add(type, instance);

        public async Task<InjectResult> InjectServicesAsync(Assembly assembly)
            => await _injector.InjectDependenciesAsync(assembly);
    }
}