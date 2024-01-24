using Microsoft.EntityFrameworkCore;
using Shop.Models;

namespace Shop.Repositories
{
    public class ProductRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddProduct(Product entity)
        {
            _dbContext.Products.Add(entity);
            SaveChanges();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _dbContext.Products.ToList();
        }

        public Product GetProductBySerialNumber(string serialNumber)
        {
            return _dbContext.Products.FirstOrDefault(x => x.SerialNumber == serialNumber);
        }

        public void RemoveProductBySerialNumber(string serialNumber)
        {
            var product = _dbContext.Products.FirstOrDefault(x => x.SerialNumber == serialNumber);
            _dbContext.Products.Remove(product);
            SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            _dbContext.Products.Update(product);
            SaveChanges();
        }

        public void SellProduct(int productId, int amount)
        {
            var product = _dbContext.Products.FirstOrDefault(x => x.Id == productId);
            product.Amount -= amount;
            _dbContext.Update(product);
            _dbContext.SaveChanges();
        }

        public void ImportProducts(IEnumerable<Product> products)
        {
            _dbContext.Products.AddRange(products);
            SaveChanges();
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
