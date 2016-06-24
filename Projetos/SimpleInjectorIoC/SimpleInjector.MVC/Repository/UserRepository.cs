using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimpleInjector.MVC.Controllers;
using SimpleInjector.MVC.Interface;

namespace SimpleInjector.MVC.Repository
{
    public class UserRepository : IUserRepository
    {
        public User GetById(Guid id)
        {
            return new User
            {
                Id = id,
                Idade = 25,
                Nome = "Charles Lomboni"
            };
        }
    }
}