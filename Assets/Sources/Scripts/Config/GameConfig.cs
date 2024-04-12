using System;
using System.Collections.Generic;
using Sources.Scripts.ServiceConfig;

namespace Sources.Scripts.Config
{
    public class GameConfig : IConfiguration
    {
        public string Name => GetType().Name;

        public Dictionary<Type, object> GetImpl()
        {
            return new Dictionary<Type, object>
            {
                { typeof(IPresenters), typeof(GamePresenter) }
            };
        }
    }
}