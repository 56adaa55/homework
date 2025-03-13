using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace List
{
    public class List<T>
    {
        public class Node
        {
            public T data;
            public Node next;
            public Node(T d)
            {
                data = d;
                next = null;
            }
        }
        private Node head;
        public List()
        {
            head = null;
        }
        public void add(T data)
        {
            Node new_node = new Node(data);
            if (head == null)
                head = new_node;
            else
            {
                Node cur = head;
                while (cur.next != null)
                    cur = cur.next;
                cur.next = new_node;
            }
        }
        public void Foreach(Action<T> action)
        {
            Node cur = head;
            while(cur!=null)
            {
                action(cur.data);
                cur = cur.next;
            }
        }
    }
    public class Program
    {
        public static void Main()
        {
            List<int> mylist = new List<int>();
            Console.WriteLine("请输入数据：");
            string input = Console.ReadLine();
            string[] parts = input.Split(' '); // 使用空格作为分隔符
            // 将每个字符串转换为整数
            int[] numbers = parts.Select(x => Convert.ToInt32(x)).ToArray();
            for (int i = 0; i < numbers.Length; i++)
                mylist.add(numbers[i]);
            Console.Write("数组里的元素依次为：");
            mylist.Foreach(m => Console.Write(m + " "));
            Console.WriteLine();
            int max = int.MinValue;
            mylist.Foreach(m => { if (m > max) max = m; });
            Console.Write($"数组里的最大值为：{max}");
            Console.WriteLine();
            int min = int.MaxValue;
            mylist.Foreach(m => { if (m < min) min = m; });
            Console.Write($"数组里的最大值为：{min}");
            Console.WriteLine();
            int sum = 0;
            mylist.Foreach(m => sum = sum + m);
            Console.Write($"数组里元素的和为：{sum}");
        }
    }
}
