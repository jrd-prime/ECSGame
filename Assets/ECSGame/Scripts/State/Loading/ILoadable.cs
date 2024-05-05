using System;
using Cysharp.Threading.Tasks;

namespace ECSGame.Scripts.State.Loading
{
    public interface ILoadable
    {
        public string Description { get; }
        public UniTask Load(Action<ILoadable> action);
    }
}