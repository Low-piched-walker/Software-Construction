using System;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Print welcome message
            Console.WriteLine("Welcome to the calculator program!");

            // Get input from the user
            Console.Write("Enter the first number: ");
            double num1 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter the second number: ");
            double num2 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter an operator (+, -, *, /): ");
            char op = Convert.ToChar(Console.ReadLine());

            // Perform the calculation based on the operator
            double result = 0;

            switch (op)
            {
                case '+':
                    result = num1 + num2;
                    break;
                case '-':
                    result = num1 - num2;
                    break;
                case '*':
                    result = num1 * num2;
                    break;
                case '/':
                    result = num1 / num2;
                    break;
                default:
                    Console.WriteLine("错误的符号，请重新输入！");
                    return;
            }

            // Print the result
            Console.WriteLine("The result is: " + result);
        }
    }
}