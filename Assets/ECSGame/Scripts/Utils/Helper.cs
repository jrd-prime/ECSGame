using System;
using System.Linq;

namespace ECSGame.Scripts.Utils
{
    public static class Helper
    {
        public static string TypeNameCutter(Type type)
        {
            return type.Name.Split('.').Last();
        }
    }
}