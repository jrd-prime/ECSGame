using System;

namespace Sources.Scripts.DI.Interface
{
    public interface IMyContainerFactory
    {
        public T GetInstance<T>(Type type) where T : class ;
    }
}