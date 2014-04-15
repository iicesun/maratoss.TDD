using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TDD
{
    public class GetGenericViaReflection
    {
        [Test]
        public void Test()
        {
            var obj = new SomeClass();
            Type type = obj.GetGenericType(typeof (IReadModel<>));

            Assert.That(type, Is.Not.Null);
            Assert.That(type, Is.EqualTo(typeof(DepartmentOrder)));
        }

        class SomeClass : IReadModel<DepartmentOrder>
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public DepartmentOrder Entity { get; set; }

            object IReadModel.Entity
            {
                get
                {
                    return Entity;
                }
                set
                {
                    Entity = (DepartmentOrder)value;
                }
            }
        }
    }

    //TODO: сюда бы пришпандырить еще кэш, чтоб по 10 раз одно и то же не искать
    public static class GenericUtilities
    {
        public static Type GetGenericType(this object @object, Type genericInterfaceType)
        {
            if (@object == null) throw new ArgumentNullException("object");

            return GetGenericType(@object.GetType(), genericInterfaceType);
        }

        public static Type GetGenericType(this Type sourceType, Type genericInterfaceType)
        {
            if (sourceType == null) throw new ArgumentNullException("sourceType");

            var searchedInterface = sourceType.GetInterfaces().
                FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == genericInterfaceType);

            if (searchedInterface == null) {
                return null;
            }

            Type[] args = searchedInterface.GetGenericArguments();

            if (!args.Any()) {
                return null;
            }

            return args[0];
        }

    }
}
