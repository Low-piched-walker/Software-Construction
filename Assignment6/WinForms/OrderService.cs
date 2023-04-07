using OM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OM
{
    class OrderService
    {
        private List<Order> orders;

        public OrderService()
        {
            orders = new List<Order>();
        }

        public void AddOrder(Order order)
        {
            if (orders.Contains(order))
            {
                throw new Exception($"The order with ID {order.OrderId} already exists.");
            }

            foreach (OrderDetail orderDetail in order.OrderDetails)
            {
                if (order.OrderDetails.FindAll(detail => detail.Equals(orderDetail)).Count > 1)
                {
                    throw new Exception($"The order detail with item name {orderDetail.ItemName} already exists.");
                }
            }

            order.TotalAmount = order.OrderDetails.Sum(detail => detail.ItemPrice * detail.ItemQuantity);
            orders.Add(order);
        }

        public void RemoveOrder(string orderId)
        {
            Order order = orders.Find(o => o.OrderId == orderId);

            if (order == null)
            {
                throw new Exception($"The order with ID {orderId} does not exist.");
            }

            orders.Remove(order);
        }

        public void UpdateOrder(Order order)
        {
            RemoveOrder(order.OrderId);
            AddOrder(order);
        }

        public List<Order> QueryOrdersByOrderId(string orderId)
        {
            return orders.Where(o => o.OrderId == orderId).OrderBy(o => o.TotalAmount).ToList();
        }

        public List<Order> QueryOrdersByCustomerName(string customerName)
        {
            return orders.Where(o => o.CustomerName == customerName).OrderBy(o => o.TotalAmount).ToList();
        }

        public List<Order> QueryOrdersByItemName(string itemName)
        {
            List<Order> queryResult = new List<Order>();

            foreach (Order order in orders)
            {
                foreach (OrderDetail orderDetail in order.OrderDetails)
                {
                    if (orderDetail.ItemName == itemName)
                    {
                        queryResult.Add(order);
                        break;
                    }
                }
            }

            queryResult.Sort((o1, o2) => o1.TotalAmount.CompareTo(o2.TotalAmount));
            return queryResult;
        }

        public List<Order> QueryOrdersByTotalAmount(double totalAmount)
        {
            return orders.Where(o => o.TotalAmount == totalAmount).OrderBy(o => o.TotalAmount).ToList();
        }

        public List<Order> SortOrders()
        {
            return orders.OrderBy(o => o.OrderId).ToList();
        }

        public List<Order> SortOrders(Func<Order, double> keySelector)
        {
            return orders.OrderBy(keySelector).ToList();
        }
    }
}
