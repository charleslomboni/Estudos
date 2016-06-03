using SimpleInjector.MVC.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleInjector.MVC.Interface
{
    public interface IUserRepository
    {
        User GetById(Guid id);
    }
}
