using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estoc;
namespace ResetBD
{
    class Program
    {
        static void Main(string[] args)
        {
            Control control= new Control();
            control.Drops();
            control.Creates();
        }
    }
}
