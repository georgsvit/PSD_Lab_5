using System;

namespace PSD_Lab_5.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public Payment Payment { get; set; }
        public string Status { get; set; }

        public Order(Payment payment, string status)
        {
            OrderId = Guid.NewGuid();
            Payment = payment;
            Status = status;
        }

        public Order(Order order, string status)
        {
            OrderId = order.OrderId;
            Payment = order.Payment;
            Status = status;
        }
    }
}
