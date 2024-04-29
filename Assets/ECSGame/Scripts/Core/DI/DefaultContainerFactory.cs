using System;
using ECSGame.Scripts.Core.DI.Interface;

namespace ECSGame.Scripts.Core.DI
{
    public class DefaultContainerFactory : IMyContainerFactory
    {
        public T GetInstance<T>(Type type) where T : class => CreateInstance(type) as T;
        private object CreateInstance(Type type) => Activator.CreateInstance(type);
    }
}