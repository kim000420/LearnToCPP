using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SingleObject
{
    private static SingleObject st;

    public static SingleObject Instance()
    {
        if (st == null)
        {
            st = new SingleObject();
            Console.WriteLine("Create New SingleObject");
        }
        Console.WriteLine("Create SingleObject");
        return st;
    }
}