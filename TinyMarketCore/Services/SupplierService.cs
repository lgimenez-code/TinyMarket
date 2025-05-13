using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMarketCore.Entities;
using TinyMarketCore.Interfaces;

namespace TinyMarketCore.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierService(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public IEnumerable<Supplier> GetAll()
        {
            return _supplierRepository.GetAll();
        }

        public int Add(Supplier entity)
        {
            return _supplierRepository.Add(entity);
        }

        public void Update(Supplier entity)
        {
            _supplierRepository.Update(entity);
        }

        public void Delete(int id)
        {
            _supplierRepository.Delete(id);
        }
    }
}
