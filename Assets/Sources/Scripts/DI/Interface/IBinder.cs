using System;
using System.Collections.Generic;
using Sources.Scripts.Core.Config;

namespace Sources.Scripts.DI.Interface
{
    public interface IBinder
    {
        Dictionary<Type, Type> GetBinds();
        void Bind(Type baseType, Type implType, Action<Type, Type> callback);
        void BindConfig(in IBindableConfiguration config, Action callback);
    }
}