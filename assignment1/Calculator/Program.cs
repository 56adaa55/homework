// See https://aka.ms/new-console-template for more information
using System;
namespace CalculatorApplication
{
    class Calculator
    {
        static void Main(string[] args)
        {
            Console.WriteLine("请输入第一个数字：");
            int num1 = int.Parse(Console.ReadLine());
            Console.WriteLine("请输入运算符：");
            string sign = Console.ReadLine();
            Console.WriteLine("请输入第二个数字：");
            int num2 = int.Parse(Console.ReadLine());
            switch (sign)
            {
                case "+":
                    Console.WriteLine("Answer:{0}", num1 + num2);
                    break;
                case "-":
                    Console.WriteLine("Answer:{0}", num1 - num2);
                    break;
                case "*":
                    Console.WriteLine("Answer:{0}", num1 * num2);
                    break;
                case "/":
                    Console.WriteLine("Answer:{0}", num1 / num2);
                    break;
            }
        }
    }
}
