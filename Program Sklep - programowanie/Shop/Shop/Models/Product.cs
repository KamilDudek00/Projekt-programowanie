namespace Shop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public string Size { get; set; }
        public string Sex { get; set; }
        public string Image { get; set; }
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
        public Product() { }
        public Product(Product copy)
        {
            Id = copy.Id;
            SerialNumber = copy.SerialNumber;
            Name = copy.Name;
            Amount = copy.Amount;
            Price = copy.Price;
            Size = copy.Size;
            Sex = copy.Sex;
            Image = copy.Image;
        }
    }
}
