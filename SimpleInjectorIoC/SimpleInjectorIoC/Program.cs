using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleInjectorIoC.Base;
using SimpleInjectorIoC.Interface;

namespace SimpleInjectorIoC
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new SimpleInjector.Container();
            container.Register<IOrder, PurchaseOrder>();

            var shoppingCart = container.GetInstance<ShoppingCart>();
            shoppingCart.CheckOut();

            Console.ReadKey();
        }
    }
}
