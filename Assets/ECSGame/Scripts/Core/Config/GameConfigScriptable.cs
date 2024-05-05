using ECSGame.Scripts.Core.Config.Providers;
using ECSGame.Scripts.Core.DataBase;
using UnityEngine;

namespace ECSGame.Scripts.Core.Config
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig", order = 1)]
    public class GameConfigScriptable : ScriptableObject
    {
        public ContainerInitSelect _containerInit = ContainerInitSelect.Default;
        public ContainerFactorySelect _containerFactory = ContainerFactorySelect.Default;
        public ServiceFactorySelect _serviceFactory = ServiceFactorySelect.Standard;
        public DataBaseSelect _dataBase = DataBaseSelect.Prefs;
        public AssetLoaderSelect _assetLoader = AssetLoaderSelect.Local;
    }
}