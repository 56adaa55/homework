using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public delegate void EventHandler(object sender,DateTime datetime);

namespace Clock
{
    class Event
    {
        public event EventHandler OnClock;
        public void OnEvent(DateTime dateTime)
        {
            // 确保事件被触发时，已订阅处理函数
            OnClock?.Invoke(this, dateTime);
        }
    }
    class Clock
    {
        public Event clock = new Event();
        private void Tick(object sender,DateTime dateTime)
        {
            DateTime cur_time = DateTime.Now;
            Console.WriteLine($"Clock runs!The time is {cur_time}");
        }
        private void alarm(object sender,DateTime dateTime)
        {
            DateTime cur_time = DateTime.Now;
            if (cur_time.ToString("yyyy-MM-dd HH:mm:ss") == dateTime.ToString("yyyy-MM-dd HH:mm:ss"))
                Console.WriteLine($"Clock alarms!");
        }
        public  Clock()
        {
            clock.OnClock += Tick;
            clock.OnClock += alarm;
        }
    }
    class Program
    {
        public static void Main()
        {
            Clock clock_object = new Clock();
            // 提示用户输入日期和时间
            Console.WriteLine("请输入日期和时间 (格式: yyyy-MM-dd HH:mm:ss):");

            // 读取用户输入
            string userInput = Console.ReadLine();
            DateTime parsedDateTime;
            if (DateTime.TryParse(userInput, out parsedDateTime))
            {
                Console.WriteLine("输入的时间是: " + parsedDateTime);
            }
            else
            {
                Console.WriteLine("输入的时间格式无效，请确保格式为 yyyy-MM-dd HH:mm:ss.");
                Environment.Exit(0);
            }
            int i = 0;
            while(i<180)  //最多循环120秒
            { 
                clock_object.clock.OnEvent(parsedDateTime);
                Thread.Sleep(1000);
                i++;
            }
        }
    }
}
