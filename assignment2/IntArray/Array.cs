using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntArray
{
    class Array
    {
        public int get_max(int[]nums,int n)
        {
            int ans = nums[0];
            for(int i=1;i<n;i++)
            {
                if (nums[i] > ans)
                    ans = nums[i];
            }
            return ans;
        }
        public int get_min(int[] nums, int n)
        {
            int ans = nums[0];
            for (int i = 1; i < n; i++)
            {
                if (nums[i] < ans)
                    ans = nums[i];
            }
            return ans;
        }
        public int get_sum(int[] nums, int n)
        {
            int ans = 0;
            for (int i = 0; i < n; i++)
                ans += nums[i];
            return ans;
        }
        public double get_mean(int[] nums,int n)
        {
            return (double)get_sum(nums, n) / n;
        }
        static void Main(string[] args)
        {
            Array array = new Array();
            Console.WriteLine("请输入一串数字，用空格隔开：");
            string input = Console.ReadLine();
            string[] parts = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int[] numbers = new int[parts.Length];
            for (int i = 0; i < parts.Length; i++)
            {
                numbers[i] = int.Parse(parts[i]);  
            }
            Console.WriteLine("数组的最大值为：{0}", array.get_max(numbers, parts.Length));
            Console.WriteLine("数组的最小值为：{0}", array.get_min(numbers, parts.Length));
            Console.WriteLine("数组的和为：{0}", array.get_sum(numbers, parts.Length));
            Console.WriteLine("数组的平均值为：{0}", array.get_mean(numbers, parts.Length));
        }
    }
}
