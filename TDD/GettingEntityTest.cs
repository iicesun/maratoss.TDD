namespace TDD
{
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Rhino.Mocks;

    public class GettingEntityTest
    {
        public async void Test()
        {
            var gettingEntity = MockRepository.GenerateMock<IGettingEntity>();
            var entity = await gettingEntity.GetEntity<DepartmentOrder>(100200300L);

            Assert.That(entity, Is.Not.Null);
            Assert.That(entity.Rn, Is.EqualTo(100200300L));
        }
    }

    public interface IGettingEntity
    {
        Task<TEntity> GetEntity<TEntity>(long id);
    }
}
