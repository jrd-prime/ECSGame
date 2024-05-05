using System;

namespace ECSGame.Scripts.Core.Config
{
    public interface IProviderFromConfig
    {
        public Type GetImplType();
    }
}