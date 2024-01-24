using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class ProductOrder
    {
        [Key]
        public int Id { get; set; }
        public virtual Product Product { get; set; }
        public int ProductId { get; set; }
        public virtual Order Order { get; set; }
        public int OrderId { get; set; }
        public int ProductAmount { get; set; }
    }
}
