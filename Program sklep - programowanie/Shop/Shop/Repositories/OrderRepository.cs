using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Repositories
{
    public class OrderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _dbContext.Orders.ToList();
        }

        public void RemoveOrderById(int id)
        {
            var order = _dbContext.Orders.FirstOrDefault(x => x.Id == id);
            _dbContext.Orders.Remove(order);
            SaveChanges();
        }

        public void ChangeOrderStatus(int id, OrderStatus status)
        {
            var order = _dbContext.Orders.FirstOrDefault(x => x.Id == id);
            order.Status = status;
            _dbContext.Update(order);
            SaveChanges();
        }

        public void CreateOrder(Order order, Product product)
        {
            var entity = new ProductOrder { ProductId = product.Id, Order = order, ProductAmount = product.Amount };
            _dbContext.ProductOrders.Add(entity);
            SaveChanges();
        }

        public List<Order> GetUserOrders(int userId)
        {
            return _dbContext.Orders.Where(x => x.UserId == userId).ToList();
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
