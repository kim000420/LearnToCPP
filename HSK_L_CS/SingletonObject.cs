using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singleton
{
    public class Program
    {
        static void Main(string[] args)
        {
            SingleObject obj_A = SingleObject.Instance();
            SingleObject obj_B = SingleObject.Instance();

            if (obj_A == obj_B)
            {
                Console.WriteLine("obj_A == obj_B");
            }
        }
    }
    public class SingleObject()
    {
        private static SingleObject st;

        public static SingleObject Instance()
        {
            if (st)
            {
                st = new SingleObject();
            }
            return SingleObject;
        }
    }


}
