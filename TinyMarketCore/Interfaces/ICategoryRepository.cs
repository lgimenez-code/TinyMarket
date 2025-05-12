using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMarketCore.Entities;

namespace TinyMarketCore.Interfaces
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAll();
        int Add(Category entity);
        void Update(Category entity);
        void Delete(int id);
    }
}
