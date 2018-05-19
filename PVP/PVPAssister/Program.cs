using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVPAssister
{
    class Program
    {
        static void Main(string[] args)
        {
            Computer computer = new Computer();
            computer.ComputeMingwensForEachYingxiong();
            computer.Print();
        }
    }
}
