using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using ECSGame.Scripts.Core.Config;
using ECSGame.Scripts.Core.Config.Enum;
using ECSGame.Scripts.Core.DI;
using ECSGame.Scripts.Core.DI.Interface;
using Moq;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    [TestFixture]
    public class AppContextTests
    {
        private AppContext _context;

        // [SetUp]
        // public void SetUp()
        // {
        //     var go = new GameObject();
        //     _context = go.AddComponent<AppContext>().GetComponent<AppContext>();
        //
        //     var mockGameConfig = ScriptableObject.CreateInstance<GameConfigScriptable>();
        //     mockGameConfig._dataBase = DataBaseEnum.Local;
        //     mockGameConfig._serviceFactory = ServiceFactoryEnum.Standard;
        //     _context._gameConfig = mockGameConfig;
        // }

        // [Test]
        // public void Wtf_Test()
        // {
        //     var stub = new Mock<IAppContext>();
        //     stub.Setup(x => x.InitializeContainerAsync()).Verifiable();
        //     stub.Object.InitializeContainerAsync();
        //     stub.Verify(x => x.InitializeContainerAsync(), Times.Once);
        // }
    }
}