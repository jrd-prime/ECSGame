using System;
using ECSGame.Scripts.Core.DI.Interface;

namespace ECSGame.Scripts.Core.DI.Factory
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