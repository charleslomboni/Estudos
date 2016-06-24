using SimpleInjector.MVC.Interface;
using SimpleInjector.MVC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SimpleInjector.MVC
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RegisterSimpleInjector();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void RegisterSimpleInjector()
        {
            // 1 - Cria um novo container do simple injector
            var container = new Container();

            // 2 - Configura o container (Register)
            container.Register<IUserRepository, UserRepository>(Lifestyle.Singleton);
        }
    }
}
