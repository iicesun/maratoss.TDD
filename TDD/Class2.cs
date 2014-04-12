namespace Halfblood.UnitTests.DepartmentOrderTests
 {
     using System;
     using System.Linq;
     using System.Threading.Tasks;

     using TDD;

     using NUnit.Framework;
     using Rhino.Mocks;

     public class DepartmentOrderViewModelTest
     {
         // небольшая юзер стори:
         // нужно вывести все заявки за сегодняшнее число
         // должен отобразиться прогрессБар, пока выполняется запрос
         // если запрос выполняется долго, то должна быть возможность отменить запрос
         // взять последнюю созданную ззавку и открыть на просмотр, для более детализированного просмотра (если необходимо)
         public async void UX()
         {
             /*** нужно вывести все заявки за сегодняшнее число ***/
             var filteringModelFactory =
                 MockRepository.GenerateMock<IFilteringModelFactory>();

             // создаем модель для фильтрации
             IFilteringModel<DepartmentOrderLiteDto, DepartmentOrderFilter> filteringModel =
                 filteringModelFactory.Create<DepartmentOrderLiteDto, DepartmentOrderFilter>();

             // заполняем фильтр
             filteringModel.Filter.StateDate = FilterHelper.Today;

             // запускаем запрос и ждем
             await filteringModel.Load();

             // проверяем то вернул, али нет?
             Assert.That(filteringModel.Result, Is.Not.Null);
             Assert.That(filteringModel.Result.All(x => x.StateDate == DateTime.Today));

             // взять последнюю созданную ззавку и открыть на просмотр, для более детализированного просмотра (если необходимо)
             var readModelFactory = MockRepository.GenerateMock<IReadModelFactory>();
             IReadModel<DepartmentOrder> readModel =
                 await readModelFactory.Create<DepartmentOrder>(filteringModel.Result.Last().Rn);

             Assert.That(readModel.Entity != null);
             Assert.That(readModel.Entity.Rn == filteringModel.Result.Last().Rn);
             Assert.That(true/*тут проверка остальных свойств у ентити*/);
         }

         // После того как заявку посмотрели, должна быть возможность его отредактировать и сохранить/отменить изменения
         // при редактировании должна работать валидация
         // если сущность невалидна, то сохранение должно быть запрещено
         // должен быть индикатор показывающий идет сохранение ентитис или нет
         // после успешного/неуспешного редактирования должно показываться текстовое сообщение
         public async void UX_2()
         {
             var editModelFactory = MockRepository.GenerateMock<IEditModelFactory>();
             IEditModel<DepartmentOrder> editModel = await editModelFactory.Create<DepartmentOrder>(100200300);
             /*some editing editModel.Entity */
             editModel.Commit();
         }

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
     public interface IGettingEntity<out TEntity>
     {
         TEntity GetEntity(long id);
     }
     public interface IEditModel<out TEntity> : IReadModel<TEntity>
     {
         bool IsChanged { get; }
         IObservable<bool> IsExecuting { get; }

         void Commit();
         void Cancel();
     }
 }