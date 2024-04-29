using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ECSGame.Scripts.State.Loading
{
    public class LoadingScreen : MonoBehaviour
    {
        public async UniTask Load(Queue<ILoadable> loadingOperations)
        {
            foreach (var operation in loadingOperations)
            {
                await operation.Load();
            }
        }
    }
}