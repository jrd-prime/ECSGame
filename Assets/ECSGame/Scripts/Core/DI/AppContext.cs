using ECSGame.Scripts.Core.Config;
using ECSGame.Scripts.Core.DI.Interface;
using UnityEngine;

namespace ECSGame.Scripts.Core.DI
{
    /// <summary>
    /// Init services and inject dependencies
    /// </summary>
    public class AppContext : MonoBehaviour, IAppContext
    {
        [SerializeField] public GameConfigScriptable _gameConfig;

        public static AppContext Instance { get; private set; }

        public IMyContainer Container { get; private set; }
        public IContainerProvider ContainerProvider { get; private set; }
        public ConfigManager ConfigManager { get; private set; }

        private AppContext()
        {
        }

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        public void Init()
        {
            ConfigManager = new ConfigManager(_gameConfig);
            ContainerProvider = new DefaultContainerProvider();
            Container = ContainerProvider.GetContainer();
        }
// #if !UNITY_INCLUDE_TESTS 
//         private void OnValidate()
//         {
//             if (_gameConfig == null) throw new Exception($"Game config not set to {gameObject.name}!");
//         }
// #endif
    }
}