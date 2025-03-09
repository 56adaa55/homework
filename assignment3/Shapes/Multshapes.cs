using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shapes
{
    interface Method
    {
        double get_area();
        bool is_valid();
    }
    class Rectangle:Method
    {
        public double length { get; set; }
        public double width { get; set; }
        public double get_area()
        {
            return length * width;
        }
        public bool is_valid()
        {
            return length > 0 && width > 0;
        }
    }
    class Triangle:Method
    {
        public double a { get;set; }
        public double b { get; set; }
        public double c { get; set; }
        public double get_area()
        {
            double p = (a + b + c) / 2;
            return Math.Sqrt(p * (p - a) * (p - b) * (p - c));
        }
        public bool is_valid()
        {
            return a + b > c && a + c > b && b + c > a;
        }
    }
    class Square:Method
    {
        public double length { get; set; }
        public double get_area()
        {
            return length * length;
        }
        public bool is_valid()
        {
            return length > 0;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // 实例化 Rectangle 类
            Rectangle rectangle = new Rectangle();
            Console.WriteLine("请输入矩形的长：");
            rectangle.length = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("请输入矩形的宽：");
            rectangle.width = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("矩形的面积为: " + rectangle.get_area());
            Console.WriteLine("矩形是否有效: " + rectangle.is_valid());

            // 实例化 Triangle 类
            Triangle triangle = new Triangle();
            Console.WriteLine("请输入三角形的三边 a, b, c:");
            triangle.a = Convert.ToDouble(Console.ReadLine());
            triangle.b = Convert.ToDouble(Console.ReadLine());
            triangle.c = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("三角形的面积为: " + triangle.get_area());
            Console.WriteLine("三角形是否有效: " + triangle.is_valid());

            // 实例化 Square 类
            Square square = new Square();
            Console.WriteLine("请输入正方形的边长：");
            square.length = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("正方形的面积为: " + square.get_area());
            Console.WriteLine("正方形是否有效: " + square.is_valid());
        }
    }
}
