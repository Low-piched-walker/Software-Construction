using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderManagement
{
    // 订单明细类
    public class OrderDetails
    {
        public string ProductName { get; set; } // 商品名称
        public double Price { get; set; } // 单价
        public int Quantity { get; set; } // 数量

        // 构造函数
        public OrderDetails(string productName, double price, int quantity)
        {
            ProductName = productName;
            Price = price;
            Quantity = quantity;
        }

        // 重写Equals方法
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            OrderDetails other = (OrderDetails)obj;
            return ProductName == other.ProductName && Price == other.Price && Quantity == other.Quantity;
        }

        // 重写ToString方法
        public override string ToString()
        {
            return $"[商品名称：{ProductName}，单价：{Price}，数量：{Quantity}]";
        }
    }

    // 订单类
    public class Order
    {
        public string OrderNumber { get; set; } // 订单号
        public string Customer { get; set; } // 客户
        public List<OrderDetails> Details { get; set; } // 订单明细

        // 构造函数
        public Order(string orderNumber, string customer, List<OrderDetails> details)
        {
            OrderNumber = orderNumber;
            Customer = customer;
            Details = details;
        }

        // 重写Equals方法
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Order other = (Order)obj;
            return OrderNumber == other.OrderNumber && Customer == other.Customer && Details.SequenceEqual(other.Details);
        }

        // 重写ToString方法
        public override string ToString()
        {
            return $"订单号：{OrderNumber}，客户：{Customer}，总金额：{GetTotalPrice()}，订单明细：{string.Join("", Details)}";
        }

        // 获取订单总金额
        public double GetTotalPrice()
        {
            return Details.Sum(d => d.Price * d.Quantity);
        }
    }

    // 订单服务类
    public class OrderService
    {
        private List<Order> orderList = new List<Order>(); // 订单列表

        // 添加订单
        public void AddOrder(Order order)
        {
            if (orderList.Contains(order))
            {
                throw new ApplicationException($"添加订单错误：订单{order.OrderNumber}已经存在！");
            }
            orderList.Add(order);
        }

        // 删除订单
        public void RemoveOrder(string orderNumber)
        {
            Order order = GetOrder(orderNumber);
            if (order != null)
            {
                orderList.Remove(order);
            }
            else
            {
                throw new ApplicationException($"删除订单错误：订单{orderNumber}不存在！");
            }
        }
        // 修改订单
        public void ModifyOrder(Order newOrder)
        {
            Order oldOrder = GetOrder(newOrder.OrderNumber);
            if (oldOrder != null)
            {
                orderList.Remove(oldOrder);
                orderList.Add(newOrder);
            }
            else
            {
                throw new ApplicationException($"修改订单错误：订单{newOrder.OrderNumber}不存在！");
            }
        }

        // 查询订单（按照订单号查询）
        public Order GetOrder(string orderNumber)
        {
            return orderList.FirstOrDefault(o => o.OrderNumber == orderNumber);
        }

        // 查询订单（按照商品名称查询）
        public List<Order> QueryOrdersByProductName(string productName)
        {
            var query = from order in orderList
                        from detail in order.Details
                        where detail.ProductName == productName
                        orderby order.GetTotalPrice()
                        select order;
            return query.ToList();
        }

        // 查询订单（按照客户查询）
        public List<Order> QueryOrdersByCustomer(string customer)
        {
            var query = from order in orderList
                        where order.Customer == customer
                        orderby order.GetTotalPrice()
                        select order;
            return query.ToList();
        }

        // 查询订单（按照订单金额范围查询）
        public List<Order> QueryOrdersByPriceRange(double minPrice, double maxPrice)
        {
            var query = from order in orderList
                        where order.GetTotalPrice() >= minPrice && order.GetTotalPrice() <= maxPrice
                        orderby order.GetTotalPrice()
                        select order;
            return query.ToList();
        }

        // 排序订单（默认按照订单号排序）
        public void SortOrders()
        {
            orderList.Sort((o1, o2) => o1.OrderNumber.CompareTo(o2.OrderNumber));
        }

        // 排序订单（自定义排序）
        public void SortOrders(Comparison<Order> comparison)
        {
            orderList.Sort(comparison);
        }

        // 显示所有订单
        public void ShowAllOrders()
        {
            foreach (Order order in orderList)
            {
                Console.WriteLine(order);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            OrderService orderService = new OrderService();
            // 添加订单
            Order order1 = new Order("2023001", "张三", new List<OrderDetails> { new OrderDetails("商品1", 10, 2), new OrderDetails("商品2", 20, 1) });
            Order order2 = new Order("2023002", "李四", new List<OrderDetails> { new OrderDetails("商品3", 30, 3), new OrderDetails("商品4", 40, 4) });
            orderService.AddOrder(order1);
            orderService.AddOrder(order2);

            // 查询订单
            Console.WriteLine("按照订单号查询：");
            Order order = orderService.GetOrder("2022001");
            Console.WriteLine(order);
            Console.WriteLine();

            Console.WriteLine("按照商品名称查询：");
            List<Order> orderList = orderService.QueryOrdersByProductName("商品1");
            foreach (Order o in orderList)
            {
                Console.WriteLine(o);
            }
            Console.WriteLine();

            Console.WriteLine("按照客户查询：");
            orderList = orderService.QueryOrdersByCustomer("李四");
            foreach (Order o in orderList)
            {
                Console.WriteLine(o);
            }
            Console.WriteLine();

            Console.WriteLine("按照订单金额范围查询：");
            orderList = orderService.QueryOrdersByPriceRange(50, 150);
            foreach (Order o in orderList)
            {
                Console.WriteLine(o);
            }
            Console.WriteLine();

            // 修改订单
            Console.WriteLine("修改订单：");
            Order newOrder = new Order("2023001", "王五", new List<OrderDetails> { new OrderDetails("商品1", 10, 2), new OrderDetails("商品2", 20, 1), new OrderDetails("商品5", 50, 2) });
            orderService.ModifyOrder(newOrder);
            order = orderService.GetOrder("2023001");
            Console.WriteLine(order);
            Console.WriteLine();

            // 删除订单
            Console.WriteLine("删除订单：");
            orderService.RemoveOrder("2023002");
            orderService.ShowAllOrders();

            Console.ReadKey();
        }
    }
}


