using System;
using System.Reflection;
using System.Threading.Tasks;
using Sources.Scripts.Core.Config;
using Sources.Scripts.Factory;

namespace Sources.Scripts.DI
{
    internal interface IMyContainer
    {
        // GET
        public Task<T> GetServiceAsync<T>() where T : class;

        // BIND
        public Task<object> BindAsync<T>() where T : class;
        public Task<object> BindAsync<TBase, TImpl>() where TBase : class where TImpl : class;
        public Task<object> BindAsync<T>(T instance) where T : class;
        public Task<object> BindAsync<T>(object instance) where T : class;
        public Task BindConfigAsync(IConfiguration configuration);

        // CACHE
        public void AddToCache(Type type, object instance);

        // INJECT
        public Task<InjectResult> InjectServicesAsync(Assembly assembly);
    }
}