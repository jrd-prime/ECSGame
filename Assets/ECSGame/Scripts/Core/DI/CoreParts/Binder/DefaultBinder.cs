using System;
using System.Collections.Generic;
using ECSGame.Scripts.Core.Config.Interface;
using ECSGame.Scripts.Core.DI.Interface;
using ECSGame.Scripts.Utils;

namespace ECSGame.Scripts.Core.DI.CoreParts.Binder
{
    /// <summary>
    /// Binds cache
    /// </summary>
    public class DefaultBinder : IBinder
    {
        public Dictionary<Type, Type> GetBinds() => Binds;

        private static readonly Dictionary<Type, Type> Binds = new();

        private static bool BindMain(Type baseType, Type implType)
        {
            JLog.Msg($"Bind: {Helper.TypeNameCutter(baseType)} <- {Helper.TypeNameCutter(implType)}");
            Binds.TryAdd(baseType, implType);
            return Binds.ContainsKey(baseType);
        }

        public void Bind(Type baseType, Type implType, Action<Type, Type> callback = null)
        {
            if (baseType == null || implType == null) throw new ArgumentNullException();

            if (BindMain(baseType, implType)) callback?.Invoke(baseType, implType);
        }

        public void BindConfig(in IBindableConfiguration config, Action callback = null)
        {
            if (config == null) throw new ArgumentNullException();

            foreach (var binding in config.GetBindings())
            {
                BindMain(binding.Key, binding.Value);
            }

            callback?.Invoke();
        }
    }
}