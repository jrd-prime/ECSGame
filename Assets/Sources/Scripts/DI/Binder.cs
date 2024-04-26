using System;
using System.Collections.Generic;
using Sources.Scripts.DI.Interface;
using Sources.Scripts.Utils;

namespace Sources.Scripts.DI
{
    

    /// <summary>
    /// Binds cache
    /// </summary>
    public class Binder : IBinder
    {
        public Dictionary<Type, Type> GetBinds() => Binds;

        private static readonly Dictionary<Type, Type> Binds = new();

        private static void BindMain(Type baseType, Type implType)
        {
            if (baseType == null || implType == null) throw new ArgumentNullException();

            JLog.Msg($"Bind: {Helper.TypeNameCutter(baseType)} <- {Helper.TypeNameCutter(implType)}");

            if (Binds.ContainsKey(baseType)) return;
            
            Binds.TryAdd(baseType, implType);
        }

        public void Bind(Type type) => BindMain(type, type);
        public void Bind(Type baseType, Type implType) => BindMain(baseType, implType);
        public void Bind<T>() where T : class => BindMain(typeof(T), typeof(T));

        public void Bind<TBase, TImpl>() where TBase : class where TImpl : TBase =>
            BindMain(typeof(TBase), typeof(TImpl));

        public void Bind(Dictionary<Type, Type> dictionary)
        {
            if (dictionary == null) throw new ArgumentNullException();
            if (dictionary.Count == 0) return;

            foreach (var bind in dictionary)
            {
                BindMain(bind.Key, bind.Value);
            }
        }

        public void Bind<T>(Type type) where T : class
        {
            if (type == null) throw new ArgumentNullException();
            BindMain(typeof(T), type);
        }
    }
}