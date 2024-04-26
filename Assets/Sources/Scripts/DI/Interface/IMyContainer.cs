using System;
using System.Reflection;
using System.Threading.Tasks;
using Sources.Scripts.Core.Config;

namespace Sources.Scripts.DI.Interface
{
    public interface IMyContainer: IFieldsInjectable
    {
        public void AddToCache(Type type, in object instance);
        public Task InjectServicesAsync(Assembly assembly);
        public Task<T> GetServiceAsync<T>() where T : class;
        public Task<T> BindAsync<T>() where T : class;
        public Task<TBase> BindAsync<TBase, TImpl>() where TBase : class where TImpl : TBase;
        public Task<T> BindAsync<T>(object instanceImpl) where T : class;
        public Task<TBase> BindAsync<TBase, TImpl>(TImpl instanceImpl) where TBase : class where TImpl : TBase;
        public Task BindConfigAsync(IBindableConfiguration configuration);
    }
}