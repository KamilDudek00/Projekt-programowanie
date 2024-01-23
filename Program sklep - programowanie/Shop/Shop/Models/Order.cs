using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
        public OrderStatus Status { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
    }

    public enum OrderStatus
    {
        Złożone,
        Zrealizowane        
    }
}
