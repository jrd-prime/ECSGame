using System;
using System.Collections.Generic;

namespace Sources.Scripts.DI.Interface
{
    public interface IContainerBinder
    {
        Dictionary<Type, Type> GetBinds();
        void Bind(Type type);
        void Bind(Type baseType, Type implType);
        void Bind<T>() where T : class;
        void Bind<TBase, TImpl>() where TBase : class where TImpl : TBase;
        void Bind(Dictionary<Type, Type> dictionary);
        void Bind<T>(Type type) where T : class;
    }
}