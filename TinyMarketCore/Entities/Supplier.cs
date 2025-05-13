using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyMarketCore.Entities
{
    public class Supplier
    {
        public int? SupplierId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int? ProvinceId { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string Status { get; set; }

    }
}
