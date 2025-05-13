using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMarketCore.Entities;
using TinyMarketCore.Interfaces;

namespace TinyMarketCore.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IEnumerable<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public IEnumerable<Product> GetProductsFiltered(ProductFiltered entity)
        {
            return _productRepository.GetProductsFiltered(entity);
        }

        public int Add(Product entity)
        {
            return _productRepository.Add(entity);
        }

        public void Update(Product entity)
        {
            _productRepository.Update(entity);
        }

        public void Delete(int id)
        {
            _productRepository.Delete(id);
        }
    }
}
