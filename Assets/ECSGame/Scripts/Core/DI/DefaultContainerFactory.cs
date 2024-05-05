using System;
using Cysharp.Threading.Tasks;
using ECSGame.Scripts.Core.DI.Interface;
using ECSGame.Scripts.State.Loading;
using UnityEngine;

namespace ECSGame.Scripts.Core.DI
{
    public class DefaultContainerFactory : IMyContainerFactory
    {
        public T GetInstance<T>(Type type) where T : class => CreateInstance(type) as T;
        private object CreateInstance(Type type) => Activator.CreateInstance(type);

        public UniTask Load(Action<ILoadable> action)
        {
            action.Invoke(this);
            return UniTask.CompletedTask;
        }
    }
}