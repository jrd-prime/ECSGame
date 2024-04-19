using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sources.Scripts.Core;
using Sources.Scripts.Utils;
using UnityEngine;

namespace Sources.Scripts.DI
{
    public class Container
    {
        public ContainerBinder Binder { get; }
        public ContainerCache Cache { get; }
        public ContainerInjector Injector { get; }

        public Container()
        {
            Binder = new ContainerBinder();
            Cache = new ContainerCache();
            Injector = new ContainerInjector();
        }

        public async Task InitServicesAsync(Dictionary<Type, Type> services)
        {
            JLog.Msg($"({services.Count}) Services initialization STARTED...");

            foreach (var service in services)
            {
                Binder.Bind(service.Key, service.Value);

                // fake delay //TODO remove
                await Task.Delay(2000);
            }

            JLog.Msg($"({services.Count}) Services initialization FINISHED...");
        }

        public async Task<T> GetService<T>() where T : class => await Cache.Get<T>();

        #region Binds

        public async Task<T> Bind<T>() where T : class
        {
            Binder.Bind<T>();
            Cache.Add<T>();
            return await GetService<T>();
        }

        public async Task<object> Bind<TBase, TImpl>() where TBase : class where TImpl : class
        {
            Binder.Bind<TBase, TImpl>();
            Cache.Add<TBase, TImpl>();
            return await GetService<TBase>();
        }

        public async Task<T> Bind<T>(T instance) where T : class
        {
            if (instance == null) throw new ArgumentNullException();

            Binder.Bind<T>();
            Cache.Add<T>(instance);
            return await GetService<T>();
        }

        public async Task Bind(IBindableConfig config)
        {
            if (config == null) throw new ArgumentNullException();

            Dictionary<Type, Type> bindingsList = config.GetBindingsList();

            if (bindingsList.Count == 0) return;

            Binder.Bind(bindingsList);
            await Cache.Add(bindingsList);
        }

        #endregion
    }
}