using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMarketCore.Entities;

namespace TinyMarketCore.Services
{
    public interface ISupplierService
    {
        IEnumerable<Supplier> GetAll();
        int Add(Supplier entity);
        void Update(Supplier entity);
        void Delete(int id);
    }
}
