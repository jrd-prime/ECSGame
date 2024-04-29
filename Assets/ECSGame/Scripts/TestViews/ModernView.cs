using UnityEngine;

namespace ECSGame.Scripts.TestViews
{
    public class ModernView : IViews
    {
        public void ShowInfo()
        {
            Debug.LogWarning("Its modern view!");
        }
    }
}