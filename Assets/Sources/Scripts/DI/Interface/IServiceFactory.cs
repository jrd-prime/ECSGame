using System;

namespace Sources.Scripts.DI.Interface
{
    /// <summary>
    /// Create services
    /// </summary>
    public interface IServiceFactory
    {
        public object CreateService(Type type);
        public T CreateService<T>() where T : class;
    }
}