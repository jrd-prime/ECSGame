using System;
using System.Collections.Generic;

namespace Sources.Scripts.Core
{
    public interface IBindableConfig : IConfig
    {
        public Dictionary<Type, Type> GetBindingsList();
    }
}