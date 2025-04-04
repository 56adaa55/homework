using OrderControlSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;

namespace OrderControl_Show
{
    public partial class Form2:Form
    {
        private Order order;
        private OrderDetails orderDetail;
        private OrderDbContext dbContext;
        public Form2(OrderDbContext context)
        {
            InitializeComponent();
            dbContext = context;
            order = new Order();
            order.OrderDetails = new List<OrderDetails>();
            orderDetail = new OrderDetails();
            ID.DataBindings.Add("Text", order, "ID");
            CustomerName.DataBindings.Add("Text", order, "Customer");
            ProductName.DataBindings.Add("Text", orderDetail, "ProductName");
            Num.DataBindings.Add("Text", orderDetail, "Quantity");
            Price.DataBindings.Add("Text", orderDetail, "UnitPrice");
        }
        private int Get_Detail_Money()
        {
            int sum = 0;

            // 获取当前订单的所有商品，包括历史数据和新添加的商品
            var existingOrder = dbContext.Orders
                .Include(o => o.OrderDetails)  // 加载订单详情
                .FirstOrDefault(o => o.ID == order.ID);  // 根据ID查询订单

            // 确保存在订单
            if (existingOrder != null)
            {
                // 遍历数据库中已存在的订单详情
                foreach (var temp_detail in existingOrder.OrderDetails)
                {
                    // 如果 TotalAmount 没有计算，则动态计算
                    if (temp_detail.TotalAmount == 0)  // 假设 TotalAmount 为 0 表示未计算
                    {
                        temp_detail.TotalAmount = temp_detail.Quantity * temp_detail.UnitPrice;
                    }

                    // 累加到总金额
                    sum += temp_detail.TotalAmount;
                }
            }

            // 然后再遍历当前订单的订单详情
            foreach (var temp_detail in order.OrderDetails)
            {
                // 如果 TotalAmount 没有计算，则动态计算
                if (temp_detail.TotalAmount == 0)  // 假设 TotalAmount 为 0 表示未计算
                {
                    temp_detail.TotalAmount = temp_detail.Quantity * temp_detail.UnitPrice;
                }

                // 累加到总金额
                sum += temp_detail.TotalAmount;
            }

            return sum;
        }

        private void Add_button(object sender, EventArgs e)
        {
            var existingOrder = dbContext.Orders
                .Include(o => o.OrderDetails)  // 加载订单详情
                .FirstOrDefault(o => o.ID == order.ID);  // 根据ID查询订单

            if (existingOrder == null)
            {
                // 如果订单不存在，则添加新订单
                order.Money = Get_Detail_Money();
                dbContext.Orders.Add(order);  // 添加订单到数据库
                dbContext.SaveChanges();      // 保存到数据库
            }
            else
            {
                // 如果订单已存在，更新订单的详情
                foreach (var temp_order in order.OrderDetails)
                    existingOrder.OrderDetails.Add(temp_order);  // 添加新的订单详情
                existingOrder.Money = Get_Detail_Money();  // 更新订单总金额
                dbContext.SaveChanges();  // 保存更改
            }
        }

        private void Change_button(object sender, EventArgs e)
        {
            var existingOrder = dbContext.Orders
                .Include(o => o.OrderDetails)  // 加载订单详情
                .FirstOrDefault(o => o.ID == order.ID);  // 根据ID查询订单

            if (existingOrder == null)
            {
                MessageBox.Show("该订单不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                // 更新已有订单的属性
                existingOrder.Customer = order.Customer;
                existingOrder.OrderDetails = order.OrderDetails;
                existingOrder.Money = Get_Detail_Money();

                dbContext.SaveChanges();  // 保存到数据库
            }
        }

        private void add_details(object sender, EventArgs e)
        {
            // 检查订单详情中是否已存在相同的商品
            bool productExists = order.OrderDetails.Any(detail => detail.Equals(orderDetail));

            if (productExists)
            {
                MessageBox.Show("已存在相同商品！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // 如果存在相同商品，则不继续执行添加操作
            }

            // 添加新的订单详情
            order.OrderDetails.Add(new OrderDetails(orderDetail.ProductName, orderDetail.Quantity, orderDetail.UnitPrice));

            // 因为 OrderDetails 是 Order 的一部分，修改会自动保存
        }
    }
}

