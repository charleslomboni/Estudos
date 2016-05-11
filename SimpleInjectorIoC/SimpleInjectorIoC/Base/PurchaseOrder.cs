using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleInjectorIoC.Interface;

namespace SimpleInjectorIoC.Base
{
    public class PurchaseOrder : IOrder
    {
        public void Process()
        {
            Console.WriteLine("Purchase Order processed");
        }
    }
}
