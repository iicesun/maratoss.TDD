using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Mocks;

namespace TDD
{
    class EditWithCatalogTest
    {
        public async void Test2()
        {
            var getterCatalogs = MockRepository.GenerateMock<IGetterCatalogs>();
            IList<Catalog> catalogs = await getterCatalogs.Load();
        }
    }

    internal interface IGetterCatalogs
    {
        Task<IList<Catalog>> Load();
    }

    internal class Catalog
    {
    }
}
