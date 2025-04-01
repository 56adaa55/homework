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

namespace OrderControl_Show
{
    public partial class Form1: Form
    {
        private BindingList<Order> all_orders;
        private BindingList<OrderDetails> all_orderdetails;
        private BindingSource ordersBindingSource; // 订单绑定源
        private BindingSource orderDetailsBindingSource; // 订单明细绑定源
        public Form1()
        {
            InitializeComponent();
            all_orders = new BindingList<Order>();
            all_orderdetails = new BindingList<OrderDetails>();
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
            Form2 form2 = new Form2(all_orders,all_orderdetails);
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
                all_orders.Remove(orderToRemove);
            }
            else if(e.ColumnIndex == OrderdataGridView.Columns["show"].Index)
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
            // 如果文本为空或无法转换为数字，默认值为0
            int.TryParse(FindID.Text, out IDNumber);
            int.TryParse(FindMoney.Text, out MoneyNumber);
            bool name_empty = string.IsNullOrEmpty(customerNameFilter);
            bool id_empty =( IDNumber == 0);
            bool money_empty = (MoneyNumber == 0);
            var filteredData = all_orders.Where(p => (name_empty || (!name_empty && p.Customer == customerNameFilter)) && (id_empty || (!id_empty && p.ID == IDNumber)) && (money_empty || (!money_empty && p.Money == MoneyNumber)));
            ordersBindingSource.DataSource = filteredData.ToList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ordersBindingSource.DataSource = all_orders;
        }


    }
}
