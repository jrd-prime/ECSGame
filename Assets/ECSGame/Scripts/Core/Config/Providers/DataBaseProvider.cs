﻿using System;
using System.Collections.Generic;
using ECSGame.Scripts.Core.DataBase;

namespace ECSGame.Scripts.Core.Config.Providers
{
    public class DataBaseProvider : CustomProvider
    {
        protected override Dictionary<Enum, Type> GetDictWithImpl()
        {
            return new Dictionary<Enum, Type>
            {
                { DataBaseSelect.Prefs, typeof(PrefsDB) }
            };
        }
    }

    public enum DataBaseSelect
    {
        Prefs,
        Local,
        Cloud
    }
}