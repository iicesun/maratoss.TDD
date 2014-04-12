using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;

namespace TDD
 {
     public class DepartmentOrderViewModelTest
     {
         public async void Edit_Create()
         {
             var editModelFactory = MockRepository.GenerateMock<IEditModelFactory>();
             IEditModel<DepartmentOrder> editModel = await editModelFactory.Create<DepartmentOrder>(100200300);

             Assert.That(editModel.Entity, Is.Not.Null);

             //////
             editModelFactory = MockRepository.GenerateMock<IEditModelFactory>();
             editModel = editModelFactory.Create(new DepartmentOrder(100200300));

             Assert.That(editModel.Entity, Is.Not.Null);
         }
         public void Edit_Commit()
         {
             var departmentOrder = new DepartmentOrder(100200300) { StateDate = new DateTime(2000, 1, 1) };
             var editModelFactory = MockRepository.GenerateMock<IEditModelFactory>();
             IEditModel<DepartmentOrder> editModel = editModelFactory.Create(departmentOrder);

             Assert.That(!object.ReferenceEquals(departmentOrder, editModel.Entity));
             Assert.That(departmentOrder.StateDate, Is.EqualTo(editModel.Entity.StateDate));

             editModel.Entity.StateDate = DateTime.Today;
             editModel.Commit();
             Assert.That(departmentOrder.StateDate, Is.EqualTo(DateTime.Today));
         }
         public void Edit_Cancel()
         {
             var departmentOrder = new DepartmentOrder(100200300) { StateDate = new DateTime(2000, 1, 1) };
             var editModelFactory = MockRepository.GenerateMock<IEditModelFactory>();
             IEditModel<DepartmentOrder> editModel = editModelFactory.Create(departmentOrder);
             editModel.Entity.StateDate = DateTime.Today;

             Assert.That(departmentOrder.StateDate, Is.Not.EqualTo(editModel.Entity.StateDate));
             editModel.Cancel();
             Assert.That(departmentOrder.StateDate, Is.EqualTo(editModel.Entity.StateDate));
         }
     }

     public interface IEditModelFactory
     {
         Task<IEditModel<T>> Create<T>(long id);
         IEditModel<T> Create<T>(T entityForEdit);
     }
     public interface IEditModel<out TEntity> : IReadModel<TEntity>
     {
         bool IsChanged { get; }
         IObservable<bool> IsExecuting { get; }

         void Commit();
         void Cancel();
     }
 }