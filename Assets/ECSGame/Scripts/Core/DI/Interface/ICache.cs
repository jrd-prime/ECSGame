using System;
using System.Collections.Generic;
using ECSGame.Scripts.Core.Config.Interface;

namespace ECSGame.Scripts.Core.DI.Interface
{
    public interface ICache : IFieldsInjectable
    {
        public T Get<T>() where T : class;
        public Dictionary<Type, object> GetCache();
        public void Add(in IBindableConfiguration tempConfig);
        public void Add(Type baseType, Type implType = null, in object implInstance = null);
        public void Clear();
    }
}