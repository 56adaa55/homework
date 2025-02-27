using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{
    class Is_Matrix
    {
        public bool is_matrix(int[,] nums,int n)
        {
            int num = nums[0, 0];
            for(int i=1;i<n;i++)
            {
                if (num != nums[i, i])
                    return false;
            }
            return true;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("输入矩阵的行数：");
            int M = int.Parse(Console.ReadLine());
            Console.WriteLine("输入矩阵的列数：");
            int N = int.Parse(Console.ReadLine());
            int[,] nums = new int[M, N];
            Console.WriteLine("请输入二维数组的元素，按行输入，每行输入 " + N + " 个数字（用空格分隔）:");
            for (int i = 0; i < M; i++)
            {
                Console.WriteLine("输入第 " + (i + 1) + " 行的数据：");
                string[] input = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < N; j++)
                {
                    nums[i, j] = int.Parse(input[j]);
                }
            }
            Is_Matrix is_Matrix = new Is_Matrix();
            int min_num = M < N ? M : N;
            if (is_Matrix.is_matrix(nums, min_num))
                Console.WriteLine("该矩阵是托普利茨矩阵");
            else
                Console.WriteLine("该矩阵不是托普利茨矩阵");
        }
    }
}
