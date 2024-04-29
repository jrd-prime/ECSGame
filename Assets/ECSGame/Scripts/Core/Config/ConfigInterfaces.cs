using System;
using System.Collections.Generic;

namespace ECSGame.Scripts.Core.Config
{
    public interface IConfiguration
    {
    }

    public interface IBindableConfiguration : IConfiguration
    {
        public Dictionary<Type, Type> GetBindings();
    }
}