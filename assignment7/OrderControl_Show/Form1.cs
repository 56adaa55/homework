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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using System.Data.Entity;

namespace OrderControl_Show
{
    public partial class Form1: Form
    {
        private BindingList<Order> all_orders;
        private OrderDbContext dbContext;
        private BindingSource ordersBindingSource; // 订单绑定源
        private BindingSource orderDetailsBindingSource; // 订单明细绑定源
        public Form1()
        {
            InitializeComponent();
            dbContext = new OrderDbContext(); // 初始化数据库上下文

            // 使用数据库数据填充订单列表
            all_orders = new BindingList<Order>(dbContext.Orders.Include(temporder => temporder.OrderDetails).ToList());
            for (int i = 0; i < all_orders.Count; i++)
                all_orders[i].CalculateTotalAmount();
            ordersBindingSource = new BindingSource();
            orderDetailsBindingSource = new BindingSource();
            ordersBindingSource.DataSource = all_orders;
            orderDetailsBindingSource.DataSource = all_orders;
            orderDetailsBindingSource.DataMember = "OrderDetails";
            OrderdataGridView.DataSource = ordersBindingSource;
            OrderDetailsdataGridView.DataSource = orderDetailsBindingSource;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(dbContext);
            DialogResult result = form2.ShowDialog();
        }
        private void dtLineHCurvePara_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowidx = (e.RowIndex + 1).ToString();
            var centerFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            var headerBuunds = new System.Drawing.Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowidx, new Font("宋体", 11), SystemBrushes.ControlText, headerBuunds, centerFormat);
        }
        private void OrderdataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == OrderdataGridView.Columns["delete"].Index)
            {
                int rowIndex = e.RowIndex;
                Order orderToRemove = (Order)OrderdataGridView.Rows[rowIndex].DataBoundItem;

                // 从数据库中删除订单
                dbContext.Orders.Remove(orderToRemove);
                dbContext.SaveChanges(); // 保存到数据库

                // 从 BindingList 中移除订单
                all_orders.Remove(orderToRemove);
            }
            else if (e.ColumnIndex == OrderdataGridView.Columns["show"].Index)
            {
                Order selectedOrder = ordersBindingSource.Current as Order;
                if (selectedOrder != null)
                {
                    // 将选中的订单的 OrderDetails 绑定到订单明细 DataGridView
                    orderDetailsBindingSource.DataSource = selectedOrder.OrderDetails;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string customerNameFilter = FindName.Text;
            int IDNumber = 0;
            int MoneyNumber = 0;

            int.TryParse(FindID.Text, out IDNumber);
            int.TryParse(FindMoney.Text, out MoneyNumber);

            var filteredData = dbContext.Orders
                .Include(o => o.OrderDetails)
                .Where(p => (string.IsNullOrEmpty(customerNameFilter) || p.Customer.Contains(customerNameFilter)) &&
                            (IDNumber == 0 || p.ID == IDNumber) &&
                            (MoneyNumber == 0 || p.Money == MoneyNumber))
                .ToList();

            ordersBindingSource.DataSource = filteredData;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            all_orders = new BindingList<Order>(dbContext.Orders.Include(o => o.OrderDetails).ToList());
            ordersBindingSource.DataSource = all_orders;
        }


    }
}
