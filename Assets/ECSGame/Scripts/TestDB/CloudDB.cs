using System;
using Cysharp.Threading.Tasks;
using ECSGame.Scripts.State.Loading;

namespace ECSGame.Scripts.TestDB
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

        public string Description { get; }
        public UniTask Load(Action<ILoadable> action)
        {
            throw new NotImplementedException();
        }
    }
}