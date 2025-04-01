using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderControlSystem
{
    public enum OrderSortCriteria
    {
        ByOrderID,
        ByCustomer,
        ByMoney
    }
    public class Order
    {
        public int ID { get; set; }
        public string Customer { get; set; }
        public int Money { get; set; } // 总金额

        public List<OrderDetails> OrderDetails { get; set; } // 订单的商品详细信息
        public Order()
        {

        }
        public Order(int num, string customer)
        {
            this.ID = num;
            this.Customer = customer;
            this.OrderDetails = new List<OrderDetails>(); // 初始化订单详情列表
            this.Money = 0; // 初始总金额为0
        }

        // 计算订单的总金额
        public void CalculateTotalAmount()
        {
            this.Money = OrderDetails.Sum(detail => detail.TotalAmount); // 根据订单详情计算总金额
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj is Order other)
            {
                return ID == other.ID;
            }

            return false;
        }

        public override string ToString()
        {
            string orderDetailsStr = string.Join("\n", OrderDetails.Select(detail => detail.ToString())); // 获取每个商品的详细信息
            return $"Order ID: {ID}, Customer: {Customer}, Total Money: {Money}\nOrder Details:\n{orderDetailsStr}";
        }
    }

    public class OrderDetails
    {
        public string ProductName { get; set; } // 商品名称
        public int Quantity { get; set; }      // 商品数量
        public int UnitPrice { get; set; }     // 商品单价
        public int TotalAmount { get; set; }   // 商品总金额 = 数量 * 单价
        public OrderDetails()
        {

        }
        public OrderDetails(string productName, int quantity, int unitPrice)
        {
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;
            TotalAmount = quantity * unitPrice; // 计算该商品的总金额
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (OrderDetails)obj;
            return ProductName == other.ProductName && Quantity == other.Quantity && UnitPrice == other.UnitPrice;
        }
        public override string ToString()
        {
            return $"Product: {ProductName}, Quantity: {Quantity}, Unit Price: {UnitPrice:C}, Total Amount: {TotalAmount:C}";
        }
    }

    public class OrderService
    {
        private List<Order> all_orders;
        private OrderSortCriteria currentSortCriteria;

        public OrderService()
        {
            all_orders = new List<Order>();
            currentSortCriteria = OrderSortCriteria.ByOrderID; // 默认按订单号排序
        }

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

        // 添加订单，参数化输入
        public void AddOrder(int id, string customer, List<OrderDetails> orderDetails)
        {
            Order newOrder = new Order(id, customer);

            // 检查订单是否已经存在
            var existingOrder = all_orders.FirstOrDefault(order => order.Equals(newOrder));
            if (existingOrder == null)
            {
                newOrder.OrderDetails = orderDetails; // 设置订单详情
                newOrder.CalculateTotalAmount(); // 计算订单总金额
                all_orders.Add(newOrder);
                SortOrders(); // 排序
                Console.WriteLine("订单和详细信息已添加成功！");
            }
            else
            {
                Console.WriteLine("该订单已存在！");
            }
        }

        // 删除订单，参数化输入订单ID
        public void DeleteOrder(int id)
        {
            var orderToRemove = all_orders.FirstOrDefault(order => order.ID == id);
            if (orderToRemove != null)
            {
                all_orders.Remove(orderToRemove);
                SortOrders();
                Console.WriteLine("订单删除成功！");
            }
            else
            {
                Console.WriteLine("未找到指定的订单！");
            }
        }

        // 修改订单，参数化输入订单ID和修改的内容
        public void ChangeOrder(int id, int? newCustomer = null, string newProductName = null, int? newQuantity = null, int? newUnitPrice = null)
        {
            int findIndex = all_orders.FindIndex(order => order.ID == id);
            if (findIndex != -1)
            {
                // 修改客户信息
                if (newCustomer.HasValue)
                {
                    all_orders[findIndex].Customer = newCustomer.Value.ToString();
                }

                // 修改商品信息
                if (newProductName != null && newQuantity.HasValue && newUnitPrice.HasValue)
                {
                    var productToModify = all_orders[findIndex].OrderDetails.FirstOrDefault(detail => detail.ProductName == newProductName);

                    if (productToModify != null)
                    {
                        productToModify.Quantity = newQuantity.Value;
                        productToModify.UnitPrice = newUnitPrice.Value;
                        productToModify.TotalAmount = productToModify.Quantity * productToModify.UnitPrice; // 更新商品总金额
                    }
                    else
                    {
                        Console.WriteLine("未找到指定的商品！");
                    }
                }

                all_orders[findIndex].CalculateTotalAmount(); // 修改后重新计算订单总金额
                SortOrders(); // 排序
                Console.WriteLine("订单已修改！");
            }
            else
            {
                Console.WriteLine("该订单不存在！");
            }
        }

        // 根据排序标准排序订单
        public void SortOrdersOption(int choice)
        {
            switch (choice)
            {
                case 1:
                    currentSortCriteria = OrderSortCriteria.ByOrderID; // 按订单ID排序
                    break;
                case 2:
                    currentSortCriteria = OrderSortCriteria.ByCustomer; // 按客户名称排序
                    break;
                case 3:
                    currentSortCriteria = OrderSortCriteria.ByMoney; // 按金额排序
                    break;
                default:
                    Console.WriteLine("无效的选择！");
                    return;
            }

            // 排序
            SortOrders();
        }

        // 查找订单，参数化输入查找条件
        public void FindOrders(int choice, int? id = null, string customer = null, int? money = null)
        {
            IEnumerable<Order> foundOrders = null;
            if (choice == 1 && id.HasValue)
            {
                foundOrders = all_orders.Where(w => w.ID == id.Value);
            }
            else if (choice == 2 && customer != null)
            {
                foundOrders = all_orders.Where(w => w.Customer == customer);
            }
            else if (choice == 3 && money.HasValue)
            {
                foundOrders = all_orders.Where(w => w.Money == money.Value);
            }
            else
            {
                Console.WriteLine("无效的查找条件！");
                return;
            }

            if (!foundOrders.Any())
            {
                Console.WriteLine("未找到符合条件的订单！");
            }
            else
            {
                foreach (var order in foundOrders)
                {
                    Console.WriteLine(order.ToString());
                }
            }
        }
        public List<Order> GetOrders() => all_orders;
        // 显示所有订单
        public void DisplayOrders()
        {
            if (all_orders.Count == 0)
            {
                Console.WriteLine("目前没有任何订单！");
            }
            else
            {
                foreach (var order in all_orders)
                {
                    Console.WriteLine(order.ToString());
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            OrderService orderService = new OrderService();
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("欢迎使用订单管理系统");
                Console.WriteLine("请选择操作：");
                Console.WriteLine("1. 添加订单");
                Console.WriteLine("2. 删除订单");
                Console.WriteLine("3. 修改订单");
                Console.WriteLine("4. 查找订单");
                Console.WriteLine("5. 查看所有订单");
                Console.WriteLine("6. 排序订单");
                Console.WriteLine("7. 退出");
                Console.Write("请输入您的选择: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        // 添加订单
                        Console.Write("请输入订单ID: ");
                        int orderId = int.Parse(Console.ReadLine());
                        Console.Write("请输入客户姓名: ");
                        string customerName = Console.ReadLine();

                        List<OrderDetails> orderDetails = new List<OrderDetails>();
                        bool addMoreProducts = true;

                        while (addMoreProducts)
                        {
                            Console.Write("请输入产品名称: ");
                            string productName = Console.ReadLine();

                            // 检查该商品是否已经存在
                            bool productExists = orderDetails.Any(detail => detail.ProductName.Equals(productName, StringComparison.OrdinalIgnoreCase));
                            if (productExists)
                            {
                                Console.WriteLine("该商品已经添加过，请输入一个不同的商品！");
                                continue; // 跳过此次循环，要求用户重新输入商品
                            }

                            Console.Write("请输入产品数量: ");
                            int quantity = int.Parse(Console.ReadLine());
                            Console.Write("请输入产品价格: ");
                            int price = int.Parse(Console.ReadLine());

                            // 添加新的订单商品
                            orderDetails.Add(new OrderDetails(productName, quantity, price));

                            Console.Write("是否继续添加产品？(y/n): ");
                            string continueChoice = Console.ReadLine().ToLower();
                            if (continueChoice != "y")
                            {
                                addMoreProducts = false;
                            }
                        }

                        orderService.AddOrder(orderId, customerName, orderDetails);
                        break;
                    case "2":
                        // 删除订单
                        Console.Write("请输入要删除的订单ID: ");
                        int orderIdToDelete = int.Parse(Console.ReadLine());
                        orderService.DeleteOrder(orderIdToDelete);
                        break;

                    case "3":
                        // 修改订单
                        Console.Write("请输入要修改的订单ID: ");
                        int orderIdToModify = int.Parse(Console.ReadLine());
                        orderService.ChangeOrder(orderIdToModify);
                        break;

                    case "4":
                        // 查找订单
                        Console.WriteLine("请选择查找方式：");
                        Console.WriteLine("1. 根据订单ID查找");
                        Console.WriteLine("2. 根据客户姓名查找");
                        Console.WriteLine("3. 根据订单金额查找");

                        int choice1 = int.Parse(Console.ReadLine());

                        // 根据选择的查找方式执行不同的操作
                        if (choice1 == 1)
                        {
                            Console.Write("请输入要查找的订单ID: ");
                            int orderIdToFind = int.Parse(Console.ReadLine());
                            orderService.FindOrders(choice1, id: orderIdToFind);
                        }
                        else if (choice1 == 2)
                        {
                            Console.Write("请输入客户姓名: ");
                            string find_customerName = Console.ReadLine();
                            orderService.FindOrders(choice1, customer: find_customerName);
                        }
                        else if (choice1 == 3)
                        {
                            Console.Write("请输入订单金额: ");
                            int orderMoney = int.Parse(Console.ReadLine());
                            orderService.FindOrders(choice1, money: orderMoney);
                        }
                        break;

                    case "5":
                        // 查看所有订单
                        orderService.DisplayOrders();
                        break;

                    case "6":
                        // 排序订单
                        Console.WriteLine("请选择排序方式：");
                        Console.WriteLine("1. 按订单ID排序");
                        Console.WriteLine("2. 按客户姓名排序");
                        Console.WriteLine("3. 按订单金额排序");

                        int sortChoice = int.Parse(Console.ReadLine());
                        orderService.SortOrdersOption(sortChoice);
                        break;

                    case "7":
                        // 退出程序
                        exit = true;
                        Console.WriteLine("谢谢使用订单管理系统！");
                        break;

                    default:
                        Console.WriteLine("无效的选择，请重新输入！");
                        break;
                }

                Console.WriteLine("按任意键返回主菜单...");
                Console.ReadKey();
            }
        }
    }
}
