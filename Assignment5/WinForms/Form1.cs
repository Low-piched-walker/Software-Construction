using OM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace WinForms
{
    public partial class Form1 : Form
    {
        
            private OrderService orderService;
            private BindingSource orderBindingSource;

            public Form1()
            {
                InitializeComponent();

                // 初始化OrderService和BindingSource
                orderService = new OrderService();
                orderBindingSource = new BindingSource();
                orderBindingSource.DataSource = orderService.GetAllOrders();

                // 绑定DataGridView和BindingSource
                orderDataGridView.DataSource = orderBindingSource;
            }

            // 查询订单
            private void queryButton_Click(object sender, EventArgs e)
            {
                string keyword = keywordTextBox.Text;
                List<Order> orders = null;

                if (orderIdRadioButton.Checked)
                {
                    orders = orderService.QueryOrdersByOrderId(keyword);
                }
                else if (customerNameRadioButton.Checked)
                {
                    orders = orderService.QueryOrdersByCustomerName(keyword);
                }
                else if (itemNameRadioButton.Checked)
                {
                    orders = orderService.QueryOrdersByItemName(keyword);
                }
                else if (totalAmountRadioButton.Checked)
                {
                    double totalAmount = double.Parse(keyword);
                    orders = orderService.QueryOrdersByTotalAmount(totalAmount);
                }

                orderBindingSource.DataSource = orders;
            }

            // 新建订单
            private void newOrderButton_Click(object sender, EventArgs e)
            {
                OrderEditForm editForm = new OrderEditForm(null);
                DialogResult result = editForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    Order newOrder = editForm.GetOrder();
                    orderService.AddOrder(newOrder);
                    orderBindingSource.DataSource = orderService.GetAllOrders();
                }
            }

            // 修改订单
            private void editOrderButton_Click(object sender, EventArgs e)
            {
                Order selectedOrder = orderBindingSource.Current as Order;

                if (selectedOrder == null)
                {
                    MessageBox.Show("请先选择要修改的订单");
                    return;
                }

                OrderEditForm editForm = new OrderEditForm(selectedOrder);
                DialogResult result = editForm.ShowDialog();

                if (result == DialogResult.OK)
            {
                Order updatedOrder = editForm.GetOrder();
                orderService.UpdateOrder(updatedOrder);
                orderBindingSource.DataSource = orderService.GetAllOrders();
            }
        }
        // 删除订单
        private void deleteOrderButton_Click(object sender, EventArgs e)
        {
            Order selectedOrder = orderBindingSource.Current as Order;

            if (selectedOrder == null)
            {
                MessageBox.Show("请先选择要删除的订单");
                return;
            }

            DialogResult result = MessageBox.Show("确定要删除该订单吗？", "删除订单", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                orderService.RemoveOrder(selectedOrder.OrderId);
                orderBindingSource.DataSource = orderService.GetAllOrders();
            }
        }

    }
}
