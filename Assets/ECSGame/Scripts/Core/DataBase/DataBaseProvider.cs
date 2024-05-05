using System;
using System.Collections.Generic;
using ECSGame.Scripts.Core.DataBase.DB;

namespace ECSGame.Scripts.Core.DataBase
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