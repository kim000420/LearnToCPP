using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnToSingelton
{
    /*
    public class Program
    {
        static void Main()
        {
            Singleton obj_A = Singleton.Instance();
            Singleton obj_B = Singleton.Instance();

            if (obj_A == obj_B)
            {
                Console.WriteLine("obj_A == obj_B");
            }
            else
            {
                Console.WriteLine("obj_A != obj_B");
            }
        }
    }
    */

    public class Singleton
    {
        private static Singleton st;

        public static Singleton Instance()
        {
            if (st == null)
            {
                st = new Singleton();
                Console.WriteLine("Create New SingleObject");
            }
            Console.WriteLine("Create SingleObject");
            return st;
        }
    }
}