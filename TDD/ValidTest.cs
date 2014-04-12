using FluentValidation.Results;
using NUnit.Framework;
using Rhino.Mocks;

namespace TDD
{
    public class ValidTest
    {
        public void test()
        {
            var editModel = MockRepository.GenerateMock<IEditModel<DepartmentOrder>>();
            var validModelFactory = MockRepository.GenerateMock<IValidModelFactory>();
            
            IValidModel validModel = validModelFactory.Create(editModel, "EDIT");
            Assert.That(validModel.IsValid == false);
            Assert.That(validModel.ValidationResult != null);
        }
    }

    public interface IValidModel
    {
        bool IsValid { get; }
        ValidationResult ValidationResult { get; }
    }
    public interface IValidModelFactory
    {
        IValidModel Create(object model, string contract = null);
    }
}
