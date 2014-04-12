using System;
using System.Threading.Tasks;
using Halfblood.UnitTests.DepartmentOrderTests;
using NUnit.Framework;
using ReactiveUI;
using Rhino.Mocks;

namespace TDD
{
    public class FilteringTest
    {
        // Просмотреть список заявок, фильтрануть.
        public async void Filtering()
        {
            // LOAD
            var filteringModel = MockRepository.GenerateMock<IFilteringModel<DepartmentOrder, DepartmentOrderFilter>>();
            filteringModel.Filter.Rn = 100;

            ReactiveList<DepartmentOrder> departmentOrders = await filteringModel.Load();
            IObservable<bool> isExecutingNotification = filteringModel.IsExecuting;

            Assert.That(filteringModel.IsBusy, Is.True);
            Assert.That(filteringModel.Result, Is.Not.Null);

            // ABORT LOADING
            filteringModel.Abort();
        }
    }

    public interface IFilteringModelFactory
    {
        IFilteringModel<TEntity, TFilter> Create<TEntity, TFilter>();
    }
    public interface IFilteringModel<TEntity, TFilter>
    {
        TFilter Filter { get; set; }
        IObservable<bool> IsExecuting { get; }
        bool IsBusy { get; }
        ReactiveList<TEntity> Result { get; }

        Task<ReactiveList<TEntity>> Load();
        void Abort();
    }
}
