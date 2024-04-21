using UnityEngine;

namespace Sources.Scripts.TestDB
{
    public class CloudDB : IDataBase
    {
       
        private static CloudDB _prefsDB;
        public static CloudDB I => _prefsDB ??= new CloudDB();

        private CloudDB()
        {
        }

        public void ShowInfo()
        {
            throw new System.NotImplementedException();
        }
    }
}