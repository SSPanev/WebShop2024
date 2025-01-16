using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebShop2024.Infrastructure.Data.Entities;

namespace WebShop2024.Core.Contracts
{
    public interface IOrderService
    {
        bool Create(int productId, string userId, int quantity);
        List<Order> GetOrders();

        List<Order> GetOrdersByUser(string userId);
    }
}
