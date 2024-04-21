using System;
using System.Threading.Tasks;

namespace Sources.Scripts.Factory
{
    
    public class CloudServiceFactory : IServiceFactory
    {
        public Task<object> CreateServiceAsync(Type type)
        {
            throw new NotImplementedException();
        }

        public Task<T> CreateServiceAsync<T>() where T : class
        {
            throw new NotImplementedException();
        }
    }
}