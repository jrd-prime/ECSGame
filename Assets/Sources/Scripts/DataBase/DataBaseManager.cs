﻿using System;
using Sources.Scripts.Core.Config.Enum;
using Sources.Scripts.TestDB;

namespace Sources.Scripts.DataBase
{
    public class DataBaseManager
    {
        private static DataBaseManager _dataBaseManager;
        public DataBaseEnum CurrentDB { get; private set; }
        public static DataBaseManager I => _dataBaseManager ??= new DataBaseManager();

        private DataBaseManager()
        {
        }

        public IDataBase GetDB(DataBaseEnum dataBase)
        {
            CurrentDB = dataBase;
            
            return dataBase switch
            {
                DataBaseEnum.Prefs => PrefsDB.I,
                DataBaseEnum.Local => LocalDB.I,
                DataBaseEnum.Cloud => CloudDB.I,
                _ => throw new ArgumentOutOfRangeException(nameof(dataBase), dataBase, null)
            };
        }
    }
}