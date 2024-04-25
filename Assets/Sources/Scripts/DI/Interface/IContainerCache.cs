using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sources.Scripts.DI.Interface
{
    public interface IContainerCache
    {
        Task<T> Get<T>() where T : class;
        Dictionary<Type, object> GetCache();
        void Add<T>() where T : class;
        void Add<TBase, TImpl>() where TBase : class where TImpl : TBase;
        void Add<T>(in object instance) where T : class;
        void Add(Type type, in object instance);
        Task Add(Dictionary<Type, Type> dictionary);
        void Clear();
    }

   
}