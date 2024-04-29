using System;

namespace ECSGame.Scripts.Core.DI.Interface
{
    public interface IMyContainerFactory
    {
        public T GetInstance<T>(Type type) where T : class ;
    }
}