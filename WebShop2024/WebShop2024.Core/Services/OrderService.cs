using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebShop2024.Core.Contracts;
using WebShop2024.Infrastructure.Data;
using WebShop2024.Infrastructure.Data.Entities;

namespace WebShop2024.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Create(int productId, string userId, int quantity)
        {
            var product = _context.Products.SingleOrDefault(x => x.Id == productId);

            if (product == null)
                return false;

            Order new_order = new Order()
            {
                OrderDate = DateTime.Now,
                ProductId = productId,
                UserId = userId,
                Quantity = quantity,
                Price = product.Price,
                Discount = product.Discount
            };

            product.Quantity -= quantity;

            _context.Products.Update(product);
            _context.Orders.Add(new_order);

            return _context.SaveChanges() != 0;
        }

        public List<Order> GetOrders()
        {
            return _context.Orders.OrderByDescending(x => x.OrderDate).ToList();
        }

        public List<Order> GetOrdersByUser(string userId)
        {
            return _context.Orders.Where(x => x.UserId == userId).OrderByDescending(x => x.OrderDate).ToList();
        }
    }
}
