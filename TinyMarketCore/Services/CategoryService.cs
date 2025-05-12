using TinyMarketCore.Entities;
using TinyMarketCore.Interfaces;

namespace TinyMarketCore.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IEnumerable<Category> GetAll()
        {
            return _categoryRepository.GetAll();
        }
        public int Add(Category entity)
        {
            return _categoryRepository.Add(entity);
        }
        public void Update(Category entity)
        {
            _categoryRepository.Update(entity);
        }
        public void Delete(int id)
        {
            _categoryRepository.Delete(id);
        }
    }
}
