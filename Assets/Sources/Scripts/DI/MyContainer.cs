using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Sources.Scripts.Annotation;
using Sources.Scripts.Core.Config;
using UnityEngine;

namespace Sources.Scripts.DI
{
    public class MyContainer : IMyContainer
    {
        [JManualInject] private readonly ContainerBinder _binder;
        [JManualInject] private readonly ContainerCache _cache;
        [JManualInject] private readonly ContainerInjector _injector;

        public async Task<T> GetServiceAsync<T>() where T : class => await _cache.Get<T>() as T;

        // public async Task BindAsync(Type type, object instance)
        // {
        //     if (type == null || instance == null) throw new ArgumentNullException();
        //
        //     Debug.LogWarning("bi " + _binder.GetBinds().Count);
        //     _binder.Bind(type, instance.GetType());
        //     Debug.LogWarning("bi " + _binder.GetBinds().Count);
        //     _cache.Add(type, in instance);
        //     await Task.CompletedTask;
        // }

        public async Task<T> BindAsync<T>() where T : class
        {
            _binder.Bind<T>();
            _cache.Add<T>();
            return await GetServiceAsync<T>();
        }

        public async Task<TBase> BindAsync<TBase, TImpl>() where TBase : class where TImpl : TBase
        {
            _binder.Bind<TBase, TImpl>();
            _cache.Add<TBase, TImpl>();
            return await GetServiceAsync<TBase>();
        }

        public async Task<T> BindAsync<T>(object instanceImpl) where T : class
        {
            if (instanceImpl == null) throw new ArgumentNullException();

            _binder.Bind<T>();
            _cache.Add<T>(in instanceImpl);
            return await GetServiceAsync<T>();
        }

        public async Task<TBase> BindAsync<TBase, TImpl>(TImpl instanceImpl) where TBase : class where TImpl : TBase
        {
            if (instanceImpl == null) throw new ArgumentNullException();

            object instance = instanceImpl;

            _binder.Bind<TBase>(instanceImpl.GetType());
            _cache.Add<TBase>(in instance);
            return await GetServiceAsync<TBase>();
        }

        public async Task BindConfigAsync(IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException();

            Dictionary<Type, Type> bindingsList = configuration.GetBindings();

            _binder.Bind(bindingsList);
            await _cache.Add(bindingsList);
        }

        public void AddToCache(Type type, in object instance)
        {
            if (type == null || instance == null) throw new ArgumentNullException();
            _cache.Add(type, in instance);
        }

        public async Task InjectServicesAsync(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException();
            await _injector.InjectDependenciesAsync(assembly);
        }
    }
}