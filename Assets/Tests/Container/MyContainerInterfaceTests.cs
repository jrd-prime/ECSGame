using System.Reflection;
using ECSGame.Scripts.Core.DI;
using Moq;
using NUnit.Framework;

namespace Tests.Container
{
    [TestFixture]
    public class MyContainerInterfaceTests
    {
        private TestObject _testObject;
        private TestImpl _testImpl;
        private Assembly _assemblyMock;
        private InjectResult _stubInjectResult;

        [SetUp]
        public void SetUp()
        {
            _testObject = new TestObject();
            _testImpl = new TestImpl();
        }

        // [Test]
        // public void Get_StateVerification()
        // {
        //     // Arrange
        //     IMyContainer containerFullMock =
        //         Mock.Of<IMyContainer>(x => x.GetService<TestObject>().Result == _testObject);
        //
        //     // Act
        //     var actual = containerFullMock.GetService<TestObject>();
        //
        //     // Assert
        //     Assert.IsInstanceOf(typeof(TestObject), actual.Result);
        // }

        // [Test]
        // public void Bind_StateVerification()
        // {
        //     // Arrange
        //     IMyContainer stubVerifyBindMethods = Mock.Of<IMyContainer>(
        //         container =>
        //             container.Bind<TestObject>().Result == _testObject &&
        //             container.Bind<TestBase, TestImpl>().Result == _testImpl &&
        //             container.Bind<TestObject>(_testObject).Result == _testObject &&
        //             container.Bind<TestBase>(_testImpl).Result == _testImpl
        //     );
        //
        //     // Act
        //     var actualOneGeneric = stubVerifyBindMethods.Bind<TestObject>().Result;
        //     var actualTwoGeneric = stubVerifyBindMethods.Bind<TestBase, TestImpl>().Result;
        //     var actualWithInstance = stubVerifyBindMethods.Bind<TestObject>(_testObject).Result;
        //     var actualGenericWithImplInstance = stubVerifyBindMethods.Bind<TestBase>(_testImpl).Result;
        //
        //     // Assert
        //     Assert.That(actualOneGeneric, Is.EqualTo(_testObject));
        //     Assert.That(actualTwoGeneric, Is.EqualTo(_testImpl));
        //     Assert.That(actualWithInstance, Is.EqualTo(_testObject));
        //     Assert.That(actualGenericWithImplInstance, Is.EqualTo(_testImpl));
        // }
    }
}