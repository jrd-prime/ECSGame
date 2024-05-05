using System;
using Cysharp.Threading.Tasks;
using ECSGame.Scripts.State.Loading;
using ECSGame.Scripts.TestDB;
using UnityEngine;

namespace ECSGame.Scripts.Core.DataBase.DB
{
    public class PrefsDB : IDataBase
    {
        public void ShowInfo()
        {
            throw new System.NotImplementedException();
        }

        public string Description => "Prefs DB";

        public UniTask Load(Action<ILoadable> action)
        {
            action.Invoke(this);
            Debug.LogWarning(Description);
            return UniTask.CompletedTask;
        }
    }
}