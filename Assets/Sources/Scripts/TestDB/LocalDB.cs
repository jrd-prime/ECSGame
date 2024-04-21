using UnityEngine;

namespace Sources.Scripts.TestDB
{
    public class LocalDB : IDataBase
    {
        private static LocalDB _prefsDB;
        public static LocalDB I => _prefsDB ??= new LocalDB();

        private LocalDB()
        {
        }

        public void ShowInfo()
        {
            throw new System.NotImplementedException();
        }
    }
}