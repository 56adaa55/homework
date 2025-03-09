using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeFactorys
{
    interface Method
    {
        double get_area();
        bool is_valid();
        void display();
    }

    class Rectangle : Method
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
        public void display()
        {
            Console.WriteLine($"长:{length},宽:{width}");
        }
    }
    class Triangle : Method
    {
        public double a { get; set; }
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
        public void display()
        {
            Console.WriteLine($"三边长:{a},{b},{c}");
        }
    }
    class Square : Method
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
        public void display()
        {
            Console.WriteLine($"边长:{length}");
        }
    }

    class ShapeFactory
    {
        public static Method CreateShape(string shapeType)
        {
            Random random = new Random();
            switch (shapeType)
            {
                case "Rectangle":
                    return new Rectangle { length = random.Next(1, 10), width = random.Next(1, 10) };
                case "Triangle":
                    double a = random.Next(1, 10);
                    double b = random.Next(1, 10);
                    double c = random.Next(1, 10);
                    return new Triangle { a = a, b = b, c = c };
                case "Square":
                    return new Square { length = random.Next(1, 10) };
                default:
                    return null;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            double totalArea = 0;
            Random random = new Random();
            string[] shapeTypes = { "Rectangle", "Triangle", "Square" };

            for (int i = 0; i < 10; i++)
            {
                string shapeType = shapeTypes[random.Next(shapeTypes.Length)];
                Method shape = ShapeFactory.CreateShape(shapeType);

                // 打印每个形状的面积，并累加总面积
                if (shape != null && shape.is_valid())
                {
                    double area = shape.get_area();
                    Console.Write($"形状: {shapeType}, 面积: {area} ");
                    shape.display();
                    totalArea += area;
                }
                else
                {
                    Console.WriteLine($"形状: {shapeType},输入不合格！ ");
                }
            }

            Console.WriteLine($"所有形状的总面积为: {totalArea}");
        }
    }
}

