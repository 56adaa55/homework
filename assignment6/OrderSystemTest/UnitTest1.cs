using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;


namespace OrderSystemTest
{
    [TestClass]
    public class OrderServiceTests
    {
        private OrderService orderService;

        [TestInitialize]
        public void Setup()
        {
            // 每个测试方法前初始化 OrderService 实例
            orderService = new OrderService();
        }

        [TestMethod]
        public void AddOrder_ShouldAddOrderSuccessfully()
        {
            // Arrange
            List<OrderDetails> orderDetails = new List<OrderDetails>
            {
                new OrderDetails("Product A", 2, 10),
                new OrderDetails("Product B", 1, 20)
            };

            // Act
            orderService.AddOrder(1, "Customer1", orderDetails);

            // Assert
            var order = orderService.GetOrders().FirstOrDefault(o => o.ID == 1);
            Assert.IsNotNull(order, "Order should be added successfully.");
            Assert.AreEqual("Customer1", order.Customer, "Order customer should be 'Customer1'.");
            Assert.AreEqual(2, order.OrderDetails.Count, "Order should have two products.");
        }

        [TestMethod]
        public void AddOrder_ShouldNotAddDuplicateOrder()
        {
            // Arrange
            List<OrderDetails> orderDetails = new List<OrderDetails>
            {
                new OrderDetails("Product A", 2, 10),
                new OrderDetails("Product B", 1, 20)
            };
            orderService.AddOrder(1, "Customer1", orderDetails);

            // Act
            orderService.AddOrder(1, "Customer1", orderDetails); // Try to add duplicate order

            // Assert
            var orderCount = orderService.GetOrders().Count(o => o.ID == 1);
            Assert.AreEqual(1, orderCount, "Duplicate order should not be added.");
        }

        [TestMethod]
        public void DeleteOrder_ShouldDeleteExistingOrder()
        {
            // Arrange
            List<OrderDetails> orderDetails = new List<OrderDetails>
            {
                new OrderDetails("Product A", 2, 10)
            };
            orderService.AddOrder(1, "Customer1", orderDetails);

            // Act
            orderService.DeleteOrder(1);

            // Assert
            var order = orderService.GetOrders().FirstOrDefault(o => o.ID == 1);
            Assert.IsNull(order, "Order should be deleted.");
        }

        [TestMethod]
        public void ChangeOrder_ShouldUpdateCustomerAndProductInfo()
        {
            // Arrange
            List<OrderDetails> orderDetails = new List<OrderDetails>
            {
                new OrderDetails("Product A", 2, 10)
            };
            orderService.AddOrder(1, "Customer1", orderDetails);

            // Act
            orderService.ChangeOrder(1, newCustomer: 1001, newProductName: "Product A", newQuantity: 5, newUnitPrice: 15);

            // Assert
            var order = orderService.GetOrders().FirstOrDefault(o => o.ID == 1);
            Assert.IsNotNull(order, "Order should exist.");
            Assert.AreEqual("1001", order.Customer, "Customer should be updated.");
            var product = order.OrderDetails.FirstOrDefault(d => d.ProductName == "Product A");
            Assert.IsNotNull(product, "Product should exist.");
            Assert.AreEqual(5, product.Quantity, "Product quantity should be updated.");
            Assert.AreEqual(15, product.UnitPrice, "Product unit price should be updated.");
        }

        [TestMethod]
        public void SortOrdersOption_ShouldSortOrdersByOrderID()
        {
            // Arrange
            List<OrderDetails> orderDetails1 = new List<OrderDetails>
            {
                new OrderDetails("Product A", 2, 10)
            };
            List<OrderDetails> orderDetails2 = new List<OrderDetails>
            {
                new OrderDetails("Product B", 1, 20)
            };
            orderService.AddOrder(2, "Customer2", orderDetails2);
            orderService.AddOrder(1, "Customer1", orderDetails1);

            // Act
            orderService.SortOrdersOption(1); // Sort by Order ID

            // Assert
            var sortedOrders = orderService.GetOrders().ToList();
            Assert.AreEqual(1, sortedOrders[0].ID, "First order should have ID = 1.");
            Assert.AreEqual(2, sortedOrders[1].ID, "Second order should have ID = 2.");
        }

        [TestMethod]
        public void SortOrdersOption_ShouldSortOrdersByCustomer()
        {
            // Arrange
            List<OrderDetails> orderDetails1 = new List<OrderDetails>
            {
                new OrderDetails("Product A", 2, 10)
            };
            List<OrderDetails> orderDetails2 = new List<OrderDetails>
            {
                new OrderDetails("Product B", 1, 20)
            };
            orderService.AddOrder(2, "Customer2", orderDetails2);
            orderService.AddOrder(1, "Customer1", orderDetails1);

            // Act
            orderService.SortOrdersOption(2); // Sort by Customer

            // Assert
            var sortedOrders = orderService.GetOrders().ToList();
            Assert.AreEqual("Customer1", sortedOrders[0].Customer, "First order should be from Customer1.");
            Assert.AreEqual("Customer2", sortedOrders[1].Customer, "Second order should be from Customer2.");
        }

        [TestMethod]
        public void SortOrdersOption_ShouldSortOrdersByMoney()
        {
            // Arrange
            List<OrderDetails> orderDetails1 = new List<OrderDetails>
            {
                new OrderDetails("Product A", 2, 15)
            };
            List<OrderDetails> orderDetails2 = new List<OrderDetails>
            {
                new OrderDetails("Product B", 1, 20)
            };
            orderService.AddOrder(2, "Customer2", orderDetails2);
            orderService.AddOrder(1, "Customer1", orderDetails1);

            // Act
            orderService.SortOrdersOption(3); // Sort by Money

            // Assert
            var sortedOrders = orderService.GetOrders().ToList();
            Assert.AreEqual(20, sortedOrders[0].Money, "First order should have smaller total money.");
            Assert.AreEqual(30, sortedOrders[1].Money, "Second order should have larger total money.");
        }
        public class OrderService
        {
            private List<Order> all_orders = new List<Order>();
            private OrderSortCriteria currentSortCriteria = OrderSortCriteria.ByOrderID;

            public void AddOrder(int id, string customer, List<OrderDetails> orderDetails)
            {
                Order newOrder = new Order(id, customer);
                var existingOrder = all_orders.FirstOrDefault(order => order.Equals(newOrder));
                if (existingOrder == null)
                {
                    newOrder.OrderDetails = orderDetails;
                    newOrder.CalculateTotalAmount();
                    all_orders.Add(newOrder);
                    SortOrders();
                }
            }

            public void DeleteOrder(int id)
            {
                var orderToRemove = all_orders.FirstOrDefault(order => order.ID == id);
                if (orderToRemove != null)
                {
                    all_orders.Remove(orderToRemove);
                    SortOrders();
                }
            }

            public void ChangeOrder(int id, int? newCustomer = null, string newProductName = null, int? newQuantity = null, int? newUnitPrice = null)
            {
                int findIndex = all_orders.FindIndex(order => order.ID == id);
                if (findIndex != -1)
                {
                    if (newCustomer.HasValue)
                    {
                        all_orders[findIndex].Customer = newCustomer.Value.ToString();
                    }

                    if (newProductName != null && newQuantity.HasValue && newUnitPrice.HasValue)
                    {
                        var productToModify = all_orders[findIndex].OrderDetails.FirstOrDefault(detail => detail.ProductName == newProductName);
                        if (productToModify != null)
                        {
                            productToModify.Quantity = newQuantity.Value;
                            productToModify.UnitPrice = newUnitPrice.Value;
                            productToModify.TotalAmount = productToModify.Quantity * productToModify.UnitPrice;
                        }
                    }

                    all_orders[findIndex].CalculateTotalAmount();
                    SortOrders();
                }
            }

            public List<Order> GetOrders() => all_orders;

            private void SortOrders()
            {
                switch (currentSortCriteria)
                {
                    case OrderSortCriteria.ByOrderID:
                        all_orders.Sort((x, y) => x.ID.CompareTo(y.ID));
                        break;
                    case OrderSortCriteria.ByCustomer:
                        all_orders.Sort((x, y) => x.Customer.CompareTo(y.Customer));
                        break;
                    case OrderSortCriteria.ByMoney:
                        all_orders.Sort((x, y) => x.Money.CompareTo(y.Money));
                        break;
                }
            }

            public void SortOrdersOption(int choice)
            {
                switch (choice)
                {
                    case 1:
                        currentSortCriteria = OrderSortCriteria.ByOrderID;
                        break;
                    case 2:
                        currentSortCriteria = OrderSortCriteria.ByCustomer;
                        break;
                    case 3:
                        currentSortCriteria = OrderSortCriteria.ByMoney;
                        break;
                }
                SortOrders();
            }
        }

        public class Order
        {
            public int ID { get; set; }
            public string Customer { get; set; }
            public List<OrderDetails> OrderDetails { get; set; }
            public int Money => OrderDetails.Sum(d => d.TotalAmount);

            public Order(int id, string customer)
            {
                ID = id;
                Customer = customer;
            }

            public void CalculateTotalAmount()
            {
                foreach (var detail in OrderDetails)
                {
                    detail.TotalAmount = detail.Quantity * detail.UnitPrice;
                }
            }

            public override bool Equals(object obj)
            {
                return obj is Order order && order.ID == this.ID;
            }

            public override int GetHashCode()
            {
                return ID.GetHashCode();
            }
        }

        public class OrderDetails
        {
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public int UnitPrice { get; set; }
            public int TotalAmount { get; set; }

            public OrderDetails(string productName, int quantity, int unitPrice)
            {
                ProductName = productName;
                Quantity = quantity;
                UnitPrice = unitPrice;
                TotalAmount = quantity * unitPrice;
            }
        }

        public enum OrderSortCriteria
        {
            ByOrderID,
            ByCustomer,
            ByMoney
        }
    }
}

