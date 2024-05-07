using System;
using System.Collections.Generic;
using ECSGame.Scripts.Core.Config.Interface;
using ECSGame.Scripts.Core.DataBase;

namespace ECSGame.Scripts.Core.DI.Config
{
    public class DefaultBinds : IBindsConfiguration
    {
        public Dictionary<Type, Type> GetBindings()
        {
            return new Dictionary<Type, Type>
            {
                { typeof(IDataBase), typeof(PrefsDB) }
            };
        }
    }
}