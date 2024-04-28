using System;
using Sources.Scripts.DI.Interface;

namespace Sources.Scripts.DI.Factory
{
    public class DefaultServiceFactory : IServiceFactory
    {
        public object CreateService(Type type)
        {
            if (type == null) throw new ArgumentNullException();

            return Activator.CreateInstance(type);
        }

        public T CreateService<T>() where T : class
        {
            return Activator.CreateInstance<T>();
        }
    }
}