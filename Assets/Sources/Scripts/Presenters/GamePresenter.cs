using Sources.Scripts.Annotation;
using UnityEngine;

namespace Sources.Scripts
{
    public class GamePresenter : IPresenters
    {
        private ITestPresenter _testPresenter;
        
        public void gogo()
        {
            Debug.Log("game presenter in gogo");
            // _testPresenter.Present();
            
        }
    }
}