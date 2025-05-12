
namespace TinyMarketCore.Entities
{
    public class Product
    {
        public int? ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public int? CategoryId { get; set; }
        public int? SupplierId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Status { get; set; }
    }
}
