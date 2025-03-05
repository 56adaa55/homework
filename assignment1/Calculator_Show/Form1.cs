using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator_Show
{
    public partial class Form1: Form
    {
        private int num1, num2, result;
        private string sign;
        public Form1()
        {
            InitializeComponent();
        }

        private void calculator_Click(object sender, EventArgs e)
        {
            switch (sign)
            {
                case "+":
                    result = num1 + num2;
                    break;
                case "-":
                    result = num1 - num2;
                    break;
                case "*":
                    result = num1 * num2;
                    break;
                case "/":
                    result = num1 / num2;
                    break;
            }
            lblResult.Text = result.ToString();
        }

        private void Num1(object sender, EventArgs e)
        {
            // Get the value from txtNum1 TextBox and parse it as an integer
            num1 = int.Parse(txtNum1.Text);
        }

        private void Sign(object sender, EventArgs e)
        {
            // Get the sign (+, -, *, /) from a control like a button, for example
            sign = Signtxt.Text;
        }

        private void Num2(object sender, EventArgs e)
        {
            // Get the value from txtNum2 TextBox and parse it as an integer
            num2 = int.Parse(txtNum2.Text);
        }

        private void Result(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
