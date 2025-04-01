using OrderControlSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderControl_Show
{
    public partial class Form2:Form
    {
        private Order order;
        private OrderDetails orderDetail;
        private BindingList<Order> all_orders;
        private BindingList<OrderDetails> all_orderdetails;
        public Form2(BindingList<Order>main_orders,BindingList<OrderDetails>main_orderDetails)
        {
            InitializeComponent();
            all_orders = new BindingList<Order>();
            all_orderdetails = new BindingList<OrderDetails>();
            all_orders = main_orders;
            all_orderdetails = main_orderDetails;
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
            foreach (var temp_detail in order.OrderDetails)
                sum += temp_detail.TotalAmount;
            return sum;
        }
        private void Add_button(object sender, EventArgs e)
        {
            int index = all_orders
               .Select((order, idx) => new { order, idx })
               .FirstOrDefault(x => x.order.ID == order.ID)?.idx ?? -1;
            if (index == -1)
            {
                order.Money = Get_Detail_Money();
                all_orders.Add(order);
            }
            else
            {
                foreach (var temp_order in order.OrderDetails)
                    all_orders[index].OrderDetails.Add(temp_order);
                order.Money = Get_Detail_Money();
            }
        }
        private void Change_button(object sender, EventArgs e)
        {
            int index = all_orders
               .Select((order, idx) => new { order, idx })
               .FirstOrDefault(x => x.order.ID == order.ID)?.idx ?? -1;
            if(index==-1)
            {
                MessageBox.Show("该订单不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // 不继续执行添加操作
            }
            else
            {
                all_orders[index].ID = order.ID;
                all_orders[index].Customer = order.Customer;
                all_orders[index].OrderDetails = order.OrderDetails;
                all_orders[index].Money= Get_Detail_Money();
            }
        }

        private void add_details(object sender, EventArgs e)
        {
            bool productExists = order.OrderDetails.Any(detail => detail.Equals(orderDetail));
            if (productExists)
            {
                MessageBox.Show("已存在相同商品！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // 不继续执行添加操作
            }
            order.OrderDetails.Add(new OrderDetails(orderDetail.ProductName,orderDetail.Quantity,orderDetail.UnitPrice));
        }

    }
}
