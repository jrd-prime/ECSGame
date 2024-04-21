﻿using Sources.Scripts.Core.Config.Enum;
using UnityEngine;

namespace Sources.Scripts.Core.Config
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig", order = 1)]
    public class GameConfigScriptable : ScriptableObject
    {
        public ServiceFactoryEnum _serviceFactory;
        public DataBaseEnum _dataBase;
    }

}