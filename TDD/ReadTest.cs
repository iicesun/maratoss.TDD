using System.ComponentModel;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;

namespace TDD
{
    class ReadTest
    {
        // Просмотреть заявку ТМЦ
        // Отредактировать/создать заявку ТМЦ
        // Сформировать выдачу по заявке ТМЦ на сертификат, который указан в заявке или на другой
        public async void Read()
        {
            var readModelFactory = MockRepository.GenerateMock<IReadModelFactory>();
            IReadModel<DepartmentOrder> readModel = await readModelFactory.Create<DepartmentOrder>(100200300);

            Assert.That(readModel.Entity, Is.Not.Null);
        }
    }

    public interface IReadModel<out TEntity> : INotifyPropertyChanged
    {
        TEntity Entity { get; }
    }
    public interface IReadModelFactory
    {
        Task<IReadModel<TEntity>> Create<TEntity>(long id);
    }
}
