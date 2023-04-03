using OM;
using System;
using System.Collections.Generic;

namespace OM
{
    class Order
    {
        public string OrderId { get; set; }
        public string CustomerName { get; set; }
        public double TotalAmount { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

        public Order() { }

        public Order(string orderId, string customerName)
        {
            OrderId = orderId;
            CustomerName = customerName;
            OrderDetails = new List<OrderDetail>();
        }

        public override bool Equals(object obj)
        {
            return obj is Order order &&
                   OrderId == order.OrderId &&
                   CustomerName == order.CustomerName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(OrderId, CustomerName);
        }

        public override string ToString()
        {
            return $"OrderId:{OrderId}, CustomerName:{CustomerName}, TotalAmount:{TotalAmount}";
        }
    }
}
