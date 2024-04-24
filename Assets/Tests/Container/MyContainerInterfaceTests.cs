using System.Reflection;
using Moq;
using NUnit.Framework;
using Sources.Scripts.DI;

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

        [Test]
        public void Get_StateVerification()
        {
            // Arrange
            IMyContainer containerMock =
                Mock.Of<IMyContainer>(x => x.GetServiceAsync<TestObject>().Result == _testObject);

            // Act
            var actual = containerMock.GetServiceAsync<TestObject>();

            // Assert
            Assert.IsInstanceOf(typeof(TestObject), actual.Result);
        }

        [Test]
        public void Bind_StateVerification()
        {
            // Arrange
            IMyContainer stubVerifyBindMethods = Mock.Of<IMyContainer>(
                container =>
                    container.BindAsync<TestObject>().Result == _testObject &&
                    container.BindAsync<TestBase, TestImpl>().Result == _testImpl &&
                    container.BindAsync<TestObject>(_testObject).Result == _testObject &&
                    container.BindAsync<TestBase>(_testImpl).Result == _testImpl
            );

            // Act
            var actualOneGeneric = stubVerifyBindMethods.BindAsync<TestObject>().Result;
            var actualTwoGeneric = stubVerifyBindMethods.BindAsync<TestBase, TestImpl>().Result;
            var actualWithInstance = stubVerifyBindMethods.BindAsync<TestObject>(_testObject).Result;
            var actualGenericWithImplInstance = stubVerifyBindMethods.BindAsync<TestBase>(_testImpl).Result;

            // Assert
            Assert.That(actualOneGeneric, Is.EqualTo(_testObject));
            Assert.That(actualTwoGeneric, Is.EqualTo(_testImpl));
            Assert.That(actualWithInstance, Is.EqualTo(_testObject));
            Assert.That(actualGenericWithImplInstance, Is.EqualTo(_testImpl));
        }
    }
}