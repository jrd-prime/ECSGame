using System;
using System.Collections.Generic;
using ECSGame.Scripts.Core.AssetLoader;

namespace ECSGame.Scripts.Core.Config.Providers
{
    public class AssetLoaderProvider : CustomProvider
    {
        protected override Dictionary<Enum, Type> GetDictWithImpl()
        {
            return new Dictionary<Enum, Type>
            {
                { AssetLoaderSelect.Local, typeof(LocalAssetLoader) }
            };
        }
    }

    public enum AssetLoaderSelect
    {
        Local,
        Remote
    }
}