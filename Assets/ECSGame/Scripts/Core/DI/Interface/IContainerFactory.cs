using System;
using ECSGame.Scripts.State.Loading;

namespace ECSGame.Scripts.Core.DI.Interface
{
    public interface IContainerFactory : ILoadable
    {
        string ILoadable.Description => "Container Factory";

        public T GetInstance<T>(Type type) where T : class ;
    }
}