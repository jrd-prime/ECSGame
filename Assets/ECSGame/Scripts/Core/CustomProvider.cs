using System;
using System.Collections.Generic;
using ECSGame.Scripts.Core.DataBase.Const;
using ECSGame.Scripts.State.Loading;
using UnityEngine;
using UnityEngine.Assertions;

namespace ECSGame.Scripts.Core
{
    public abstract class CustomProvider
    {
        protected object CachedInstance { get; private set; }
        public Type ImplType { get; private set; }
        protected abstract Dictionary<Enum, Type> GetDictWithImpl();

        public virtual T GetImplInstance<T>() where T : ILoadable =>
            ImplType == null || CachedInstance == null
                ? throw new Exception(
                    $"Impl Type not set or Instance is null! Check init with SetImpl() in config / {this}")
                : (T)CachedInstance;

        public virtual Type GetImplType(Enum gameConfig)
        {
            if (gameConfig == null)
                throw new NullReferenceException($"Game Config does not exist.");

            var dict = GetDictWithImpl();

            if (!dict.ContainsKey(gameConfig))
                throw new KeyNotFoundException(DBError.NoTypeForImpl + gameConfig);

            return dict[gameConfig];
        }

        public void SetImpl(Enum selector)
        {
            Debug.LogWarning($"set impl {selector}");
            if (selector == null) throw new NullReferenceException($"Enum is null! / {this}");
            var dict = GetDictWithImpl();

            Assert.IsNotNull(dict);
            Assert.IsTrue(dict.Count != 0, $"Dictionary empty! {this}");
            Assert.IsFalse(!dict.ContainsKey(selector), DBError.NoTypeForImpl + selector + " / " + this);

            if (dict[selector].IsInterface)
                throw new Exception($"Impl is interface! {dict[selector]} IN {this}");

            ImplType = dict[selector];

            CachedInstance ??= Activator.CreateInstance(ImplType);
        }
    }
}