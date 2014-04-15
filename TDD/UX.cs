using System.Reactive.Linq;

namespace TDD
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using Rhino.Mocks;

    public class UX_Test
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
                readModelFactory.Create<IReadModel<DepartmentOrder>>(filteringModel.Result.Last().Rn).Wait();

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

    }
}
