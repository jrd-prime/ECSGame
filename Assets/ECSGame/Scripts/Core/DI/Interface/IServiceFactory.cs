using System;
using ECSGame.Scripts.State.Loading;

namespace ECSGame.Scripts.Core.DI.Interface
{
    /// <summary>
    /// Create services
    /// </summary>
    public interface IServiceFactory : ILoadable
    {
        string ILoadable.Description => "Service Factory";

        public object CreateService(Type type);
        public T CreateService<T>() where T : class;
        public T GetInstance<T>(Type type) where T : class;
    }
}