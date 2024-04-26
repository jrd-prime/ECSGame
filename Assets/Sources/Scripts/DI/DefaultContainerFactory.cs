using System;
using System.Dynamic;
using Sources.Scripts.DI.Interface;

namespace Sources.Scripts.DI
{
    public class DefaultContainerFactory : IMyContainerFactory
    {
        public T GetInstance<T>(Type type) where T : class => CreateInstance(type) as T;
        private object CreateInstance(Type type) => Activator.CreateInstance(type);
    }
}