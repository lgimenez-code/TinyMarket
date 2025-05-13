using TinyMarketCore.Entities;

namespace TinyMarketCore.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetProductsFiltered(ProductFiltered entity);
        int Add(Product entity);
        void Update(Product entity);
        void Delete(int id);
    }
}
