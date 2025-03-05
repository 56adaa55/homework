using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime
{
    class FindPrime
    {
        public bool IsPrime(int n)
        {
            for(int i=2;i<=n/2;i++)
            {
                if (n % i == 0)
                    return false;
            }
            return true;
        }
        static void Main(string[] args)
        {
            FindPrime findPrime = new FindPrime();
            Console.WriteLine("请输入一个数据：");
            int n = int.Parse(Console.ReadLine());
            for(int i=2;n>1;i++)
            {
                if(findPrime.IsPrime(i))
                {
                    if(n%i==0)
                    {
                        Console.Write(i + " ");
                        n /= i;
                        i--;
                    }
                }
            }
        }
    }
}
