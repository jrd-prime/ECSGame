using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Scripts.DI
{
    /// <summary>
    /// Binds cache
    /// </summary>
    public sealed class ContainerBinder
    {
        public Dictionary<Type, Type> GetBinds() => Binds;

        private static readonly Dictionary<Type, Type> Binds = new();
        
        private static void BindMain(Type baseType, Type implType) => Binds.TryAdd(baseType, implType);

        #region Overloads

        public void Bind(Type type) => BindMain(type, type);
        public void Bind(Type baseType, Type implType) => BindMain(baseType, implType);
        public void Bind<T>(GameObject gameObject) where T : class => BindMain(typeof(T), typeof(T));
        public void Bind<T>() => BindMain(typeof(T), typeof(T));

        public void Bind<TBase, TImpl>() where TBase : class where TImpl : class =>
            BindMain(typeof(TBase), typeof(TImpl));

        public void Bind(Dictionary<Type, Type> dictionary)
        {
            foreach (var bind in dictionary)
            {
                BindMain(bind.Key, bind.Value);
            }
        }

        #endregion
    }
}