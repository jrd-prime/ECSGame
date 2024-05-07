using System;
using Cysharp.Threading.Tasks;
using ECSGame.Scripts.Core.DI.Interface;
using ECSGame.Scripts.State.Loading;

namespace ECSGame.Scripts.Core.DI.ServicesFactory
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

        public UniTask Load(Action<ILoadable> action)
        {
            action.Invoke(this);
            return UniTask.CompletedTask;
        }
        
        public T GetInstance<T>(Type type) where T : class => CreateInstance(type) as T;
        private object CreateInstance(Type type) => Activator.CreateInstance(type);
    }
}