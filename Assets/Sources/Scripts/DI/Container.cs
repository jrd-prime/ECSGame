using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sources.Scripts.Core;

namespace Sources.Scripts.DI
{
    public sealed class Container
    {
        public ContainerBinder Binder;
        public ContainerCache Cache;
        public ContainerInjector Injector { get; }

        private static Container _instance;

        private Container()
        {
            Binder =  ContainerBinder.I;
            Cache =  ContainerCache.I;
            Injector = new ContainerInjector();
        }

        public static Container I => _instance ??= new Container();

        public async Task<T> GetServiceAsync<T>() where T : class => await Cache.Get<T>();


        public async Task<object> BindAsync<T>() where T : class
        {
            Binder.Bind<T>();
            Cache.Add<T>();
            return await GetServiceAsync<T>();
        }

        public async Task<object> BindAsync<TBase, TImpl>() where TBase : class where TImpl : class
        {
            Binder.Bind<TBase, TImpl>();
            Cache.Add<TBase, TImpl>();
            return await GetServiceAsync<TBase>();
        }

        public async Task<object> BindAsync<T>(T instance) where T : class
        {
            if (instance == null) throw new ArgumentNullException();

            Binder.Bind<T>();
            Cache.Add<T>(instance);
            return await GetServiceAsync<T>();
        }

        public async Task BindAsync(IBindableConfig config)
        {
            if (config == null) throw new ArgumentNullException();

            Dictionary<Type, Type> bindingsList = config.GetBindingsList();

            Binder.Bind(bindingsList);
            await Cache.Add(bindingsList);
        }
    }
}