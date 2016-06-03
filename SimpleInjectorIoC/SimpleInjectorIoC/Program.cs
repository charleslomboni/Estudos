using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjectorIoC.Base;
using SimpleInjectorIoC.Interface;

namespace SimpleInjectorIoC {
    class Program {
        static void Main(string[] args)
        {
            Start();

            var container = new SimpleInjector.Container();
            container.Register<IOrder, PurchaseOrder>();

            var shoppingCart = container.GetInstance<ShoppingCart>();
            shoppingCart.CheckOut();

            Console.ReadKey();
        }

        protected static void Start() {
            // 1 - Cria um novo container
            var container = new Container();
            // 2 - Configura o container (register)
            container.Register<IOrder, PurchaseOrder>(Lifestyle.Transient);
            // 3 - Optionally, verify the container's configuration
            container.Verify();
            // 4 - Registra o container como MVC3 IDependencyResolver
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}
