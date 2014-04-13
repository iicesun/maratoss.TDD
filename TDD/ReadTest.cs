using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using ReactiveUI;
using Rhino.Mocks;

namespace TDD
{
    class ReadTest
    {
        // Просмотреть заявку ТМЦ
        // Отредактировать/создать заявку ТМЦ
        // Сформировать выдачу по заявке ТМЦ на сертификат, который указан в заявке или на другой
        [Test]
        public async void Read()
        {
            var readModelFactory = new ReadModelFactory(MockRepository.GenerateMock<IGettingEntity>(),
                MockRepository.GenerateMock<IDependencyResolver>());

            IReadModel<DepartmentOrder> readModel =
                await readModelFactory.Create<IReadModel<DepartmentOrder>>(100200300);

            Assert.That(readModel.Entity, Is.Not.Null);
        }

        public void read2()
        {
            var readModelFactory = new ReadModelFactory(MockRepository.GenerateMock<IGettingEntity>(),
                MockRepository.GenerateMock<IDependencyResolver>());

            readModelFactory.Create<IConcreteReadModel>(100200300);
        }
    }

    internal interface IConcreteReadModel : IReadModel<DepartmentOrder>
    {
    }

    public interface IReadModel : INotifyPropertyChanged
    {
        object Entity { get; set; }
    }
    public interface IReadModel<TEntity> : IReadModel
    {
        new TEntity Entity { get; set; }
    }
    public interface IReadModelFactory
    {
        IObservable<TConcreteReadModel> Create<TConcreteReadModel>(long id, string contract = null)
            where TConcreteReadModel : IReadModel;
    }

    public class ReadModelFactory : IReadModelFactory
    {
        private readonly IGettingEntity _gettingEntity;
        private readonly IDependencyResolver _dependencyResolver;

        public ReadModelFactory(IGettingEntity gettingEntity, IDependencyResolver dependencyResolver)
        {
            _gettingEntity = gettingEntity;
            _dependencyResolver = dependencyResolver;
        }

        public IObservable<TConcreteReadModel> Create<TConcreteReadModel>(long id, string contract = null)
            where TConcreteReadModel : IReadModel
        {
            var concreteReadModelType = typeof (TConcreteReadModel);
            Type genericReadModelType = concreteReadModelType.GetGenericType(typeof(IReadModel<>));

            var command = new ReactiveCommand();
            var observable = command.RegisterAsync(_ => {
                IUiEntity entity = _gettingEntity.GetEntity(genericReadModelType, id).Wait();
                if (concreteReadModelType == typeof(IReadModel<>)) {
                    return Observable.Return(new DefaultReadModel<IUiEntity>(entity));
                }
                var readModel = (IReadModel<IUiEntity>)_dependencyResolver.GetService(typeof(TConcreteReadModel), contract);
                if (readModel != null) {
                    readModel.Entity = entity;
                    return Observable.Return(readModel);
                }

                throw new InvalidOperationException(
                    string.Format("Resolve {0} return null", concreteReadModelType));
            });

            command.Execute(null);
            return observable.Cast<TConcreteReadModel>();
        }

        class DefaultReadModel<TEntity> : IReadModel<TEntity>
        {
            public DefaultReadModel(TEntity readEntity)
            {
                Entity = readEntity;
            }

            public event PropertyChangedEventHandler PropertyChanged;
            public TEntity Entity { get; set; }
            object IReadModel.Entity
            {
                get { return Entity; }
                set { Entity = (TEntity)value; }
            }
        }
    }
}
