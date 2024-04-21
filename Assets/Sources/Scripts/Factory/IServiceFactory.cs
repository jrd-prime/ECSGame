using System;
using System.Threading.Tasks;

namespace Sources.Scripts.Factory
{
    /// <summary>
    /// Create services
    /// </summary>
    public interface IServiceFactory
    {
        public Task<object> CreateServiceAsync(Type type);
        public Task<T> CreateServiceAsync<T>() where T : class;
    }
}