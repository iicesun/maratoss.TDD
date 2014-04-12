using NUnit.Framework;
using Rhino.Mocks;

namespace TDD
{
    public class ChangeServiceTest
    {
        public void Test()
        {
            var poco = new POCO {POCO_Property = "Foo"};
            IChangeService<POCO> changeService = ChangeService.Create(poco);

            Assert.That(changeService.Value, Is.TypeOf<POCO>());
            Assert.That(changeService.Value.POCO_Property, Is.EqualTo("Foo"));

            changeService.Value.POCO_Property = "Bar";
            Assert.That(poco.POCO_Property, Is.EqualTo("Foo"));

            // test commit
            changeService.Commit();
            Assert.That(poco.POCO_Property, Is.EqualTo("Bar"));

            // test cancel
            changeService.Value.POCO_Property = "Baz";
            changeService.Cancel();
            Assert.That(poco.POCO_Property, Is.EqualTo("Bar"));
        }

        class POCO
        {
            public string POCO_Property { get; set; } 
        }
    }

    public class ChangeService
    {
        public static IChangeService Create(object @object)
        {
            return MockRepository.GenerateMock<IChangeService>();
        }

        public static IChangeService<T> Create<T>(T @object)
        {
            return MockRepository.GenerateMock<IChangeService<T>>();
        }
    }

    public interface IChangeService
    {
        object Value { get; }
        void Commit();
        void Cancel();
    }
    public interface IChangeService<out T> : IChangeService
    {
        new T Value { get; }
    }
}
