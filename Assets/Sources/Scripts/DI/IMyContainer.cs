using System;
using System.Reflection;
using System.Threading.Tasks;
using Sources.Scripts.Core.Config;

namespace Sources.Scripts.DI
{
    public interface IMyContainer
    {
        // GET
        public Task<T> GetServiceAsync<T>() where T : class;

        // BIND
        public Task<T> BindAsync<T>() where T : class;
        public Task<TBase> BindAsync<TBase, TImpl>() where TBase : class where TImpl : TBase;
        public Task<T> BindAsync<T>(object instanceImpl) where T : class;
        public Task<TBase> BindAsync<TBase, TImpl>(TImpl instanceImpl) where TBase : class where TImpl : TBase;
        public Task BindConfigAsync(IConfiguration configuration);

        // CACHE
        public void AddToCache(Type type, in object instance);

        // INJECT
        public Task InjectServicesAsync(Assembly assembly);
    }
}