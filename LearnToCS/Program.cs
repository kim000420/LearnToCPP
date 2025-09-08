using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
        else
        {
            Console.WriteLine("obj_A != obj_B");
        }
    }
}
