using PSD_Lab_5.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PSD_Lab_5.Services
{
    public class SessionStorage
    {
        private readonly List<Order> _orders = new List<Order>();

        public void AddOrder(Order order)
        {
            _orders.Add(order);
        }

        public void ReplaceOrder(Order order, Order newOrder)
        {
            _orders.Remove(order);
            _orders.Add(newOrder);
        }

        public Order GetOrder(Guid guid)
        {
            return _orders.FirstOrDefault(o => o.OrderId == guid);
        }

        public List<Order> GetOrders()
        {
            return _orders;
        }
    }
}
