using UnityEngine;

namespace ECSGame.Scripts.TestViews
{
    public class OldStyleView : IViews
    {
        public void ShowInfo()
        {
            Debug.LogWarning("Its oldstyle view!");
        }
    }
}