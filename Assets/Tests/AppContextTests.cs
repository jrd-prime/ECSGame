using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Sources.Scripts;
using Sources.Scripts.Core.Config;
using Sources.Scripts.Core.Config.Enum;
using Sources.Scripts.DI;
using UnityEngine;

namespace Tests
{
    [TestFixture]
    public class AppContextTests
    {
        private AppContext _context;

        [SetUp]
        public void SetUp()
        {
            var go = new GameObject();
            _context = go.AddComponent<AppContext>().GetComponent<AppContext>();

            var mockGameConfig = ScriptableObject.CreateInstance<GameConfigScriptable>();
            mockGameConfig._dataBase = DataBaseEnum.Local;
            mockGameConfig._serviceFactory = ServiceFactoryEnum.Standard;
            _context._gameConfig = mockGameConfig;
        }

        [Test]
        public void Wtf_Test()
        {
            var stub = new Mock<IAppContext>();
            stub.Setup(x => x.InitializeAsync()).Verifiable();
            stub.Object.InitializeAsync();
            stub.Verify(x => x.InitializeAsync(), Times.Once);
        }

        [Test]
        public void Initialize_DoesNotThrowException()
        {
            Assert.DoesNotThrowAsync(_context.InitializeAsync);
        }
    }
}