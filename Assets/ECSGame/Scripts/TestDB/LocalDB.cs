using System;
using Cysharp.Threading.Tasks;
using ECSGame.Scripts.State.Loading;

namespace ECSGame.Scripts.TestDB
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

        public string Description { get; }
        public UniTask Load(Action<ILoadable> action)
        {
            throw new NotImplementedException();
        }
    }
}