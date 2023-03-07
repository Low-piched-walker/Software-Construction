namespace CalculatorApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            double num1 = double.Parse(txtNumber1.Text);
            double num2 = double.Parse(txtNumber2.Text);
            double result = num1 + num2;
            lblResult.Text = result.ToString();

        }

        private void btnSubtract_Click(object sender, EventArgs e)
        {
            double num1 = double.Parse(txtNumber1.Text);
            double num2 = double.Parse(txtNumber2.Text);
            double result = num1 - num2;
            lblResult.Text = result.ToString();

        }

        private void btnMultiply_Click(object sender, EventArgs e)
        {
            double num1 = double.Parse(txtNumber1.Text);
            double num2 = double.Parse(txtNumber2.Text);
            double result = num1 * num2;
            lblResult.Text = result.ToString();

        }

        private void btnDivide_Click(object sender, EventArgs e)
        {
            double num1 = double.Parse(txtNumber1.Text);
            double num2 = double.Parse(txtNumber2.Text);
            double result = num1 / num2;
            lblResult.Text = result.ToString();

        }

    }
}