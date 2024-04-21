using System;
using System.Collections.Generic;
using Sources.Scripts.Utils;

namespace Sources.Scripts.DI
{
    /// <summary>
    /// Binds cache
    /// </summary>
    public sealed class ContainerBinder
    {
        public Dictionary<Type, Type> GetBinds() => Binds;

        private static readonly Dictionary<Type, Type> Binds = new();

        private static ContainerBinder _binder;
        public static ContainerBinder I => _binder ??= new ContainerBinder();

        private ContainerBinder()
        {
        }

        private static void BindMain(Type baseType, Type implType)
        {
            if (baseType == null || implType == null) throw new ArgumentNullException();

            JLog.Msg($"Bind: {Helper.TypeNameCutter(baseType)} <- {Helper.TypeNameCutter(implType)}");

            if (Binds.ContainsKey(baseType)) return;
            if (!Binds.TryAdd(baseType, implType))
                throw new Exception($"{baseType} not in Binds and failed to add");
        }

        public void Bind(Type type) => BindMain(type, type);
        public void Bind(Type baseType, Type implType) => BindMain(baseType, implType);
        public void Bind<T>() where T : class => BindMain(typeof(T), typeof(T));

        public void Bind<TBase, TImpl>() where TBase : class where TImpl : class =>
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
    }
}