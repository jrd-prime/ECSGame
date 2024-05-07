using System;
using System.Collections.Generic;

namespace ECSGame.Scripts.Core.Config.Interface
{
    public interface IBindableConfiguration : IConfiguration
    {
        public Dictionary<Type, Type> GetBindings();
    }
}