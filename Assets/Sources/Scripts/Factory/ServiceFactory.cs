using System;
using System.Threading.Tasks;

namespace Sources.Scripts.Factory
{
    public class ServiceFactory
    {
        public async Task<object> CreateServiceAsync(Type type)
        {
            if (type == null) throw new ArgumentNullException();

            return await Task.FromResult(Activator.CreateInstance(type));
        }

        public async Task<T> CreateServiceAsync<T>() where T : class
        {
            return await Task.FromResult(Activator.CreateInstance<T>());
        }
    }
}