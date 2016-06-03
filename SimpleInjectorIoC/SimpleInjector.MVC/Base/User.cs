using System;

namespace SimpleInjector.MVC.Controllers
{
    public class User
    {
        public User() { }

        public Guid Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
    }
}