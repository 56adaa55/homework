using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sieve_of_Eratosthenes
{
    class Eratosthenes
    {
        public void eratosthenes_way(ref bool[]nums)
        {
            for (int i=0;i<99;i++)
            {
                if (nums[i])
                {
                    for (int j = 2*i+2; j < 99; j += (i+2))
                        nums[j] = false;
                }
            }
        }
        static void Main(string[] args)
        {
            bool[] is_prime = new bool[99];
            for (int i = 0; i<is_prime.Length; i++)
                is_prime[i] = true; 
            Eratosthenes eratosthenes = new Eratosthenes();
            eratosthenes.eratosthenes_way(ref is_prime);
            for (int i = 0; i < 99; i++)
            {
                if (is_prime[i])
                    Console.WriteLine("{0} ", i + 2);
            }
        }
    }
}
