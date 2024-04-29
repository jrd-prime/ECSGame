using System;
using System.Collections.Generic;
using ECSGame.Scripts.TestDB;
using ECSGame.Scripts.TestViews;

namespace ECSGame.Scripts.TestConfig
{
    public class LocalService : IServiceConfig
    {
        public Dictionary<Type, Type> GetServicesList()
        {
            return new Dictionary<Type, Type>()
            {
                { typeof(IDataBase), typeof(LocalDB) },
                { typeof(IViews), typeof(ModernView) }
            };
        }
    }
}