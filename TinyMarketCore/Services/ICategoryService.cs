using TinyMarketCore.Entities;
using TinyMarketCore.Interfaces;

namespace TinyMarketCore.Services
{
    public interface ICategoryService : ICategoryRepository
    {
        IEnumerable<Category> GetAll();
        int Add(Category entity);
        void Update(Category entity);
        void Delete(int id);
    }
}
