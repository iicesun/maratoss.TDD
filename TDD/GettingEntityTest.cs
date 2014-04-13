using System;
using System.Reactive.Linq;

namespace TDD
{
    using NUnit.Framework;
    using Rhino.Mocks;

    public class GettingEntityTest
    {
        public void Test()
        {
            var gettingEntity = MockRepository.GenerateMock<IGettingEntity>();
            var entity = gettingEntity.GetEntity<DepartmentOrder>(100200300L).Wait();

            Assert.That(entity, Is.Not.Null);
            Assert.That(entity.Rn, Is.EqualTo(100200300L));
        }
    }

    public interface IUiEntity
    {
        long Rn { get; }
    }
    public interface IGettingEntity
    {
        IObservable<TEntity> GetEntity<TEntity>(long id) where TEntity : IUiEntity;
        IObservable<IUiEntity> GetEntity(Type entityType, long id);
    }
}
