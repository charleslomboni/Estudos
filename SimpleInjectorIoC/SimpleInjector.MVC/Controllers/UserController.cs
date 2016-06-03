using SimpleInjector.MVC.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimpleInjector.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository repository;

        public UserController(IUserRepository repository)
        {
            this.repository = repository;
        }

        // GET: User
        [HttpGet]
        public ActionResult Index()
        {
            User user = this.repository.GetById(Guid.NewGuid());
            return View();
        }
    }
}