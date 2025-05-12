using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyMarketCore.Entities
{
    public class Category
    {
        public int? CategoryId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string? Status { get; set; }

    }
}
