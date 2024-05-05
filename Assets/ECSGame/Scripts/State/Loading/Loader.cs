using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions;

namespace ECSGame.Scripts.State.Loading
{
    public class Loader
    {
        public Dictionary<ILoadable, int> LoadingQueueDelay { get; }
        public Queue<ILoadable> LoadingQueue { get; }

        public int FullTimeLoad { get; private set; }

        public Loader()
        {
            LoadingQueueDelay = new Dictionary<ILoadable, int>();
            LoadingQueue = new Queue<ILoadable>();
        }

        public void AddToLoadingQueue(ILoadable operation, int bonusDelay = 0, [CallerFilePath] string pas = "")
        {
            Debug.LogWarning($"Loader add: {operation.GetType()} {bonusDelay}");
            
            Assert.IsNotNull(operation, $"Operation is null! {pas}");
            
            LoadingQueue.Enqueue(operation);
            LoadingQueueDelay.Add(operation, bonusDelay);
            FullTimeLoad += bonusDelay;
        }
    }
}