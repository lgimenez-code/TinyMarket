using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMarketCore.Entities;

namespace TinyMarketCore.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetProductsFiltered(ProductFiltered entity);
        int Add(Product entity);
        void Update(Product entity);
        void Delete(int id);
    }
}
