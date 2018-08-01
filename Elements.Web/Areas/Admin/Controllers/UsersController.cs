using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elements.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Elements.Web.Areas.Admin.Controllers
{
    public class UsersController : AdminController
    {
        public UsersController(ElementsContext context) : base(context)
        {
        }

        [HttpGet]
        public IActionResult ManageUsers()
        {
            var users = this.Context.Users.Include(u => u.Topics);

            return View(model: users.ToList());
        }

        [HttpGet]
        public IActionResult Details(string userId)
        {
            var user = this.Context.Users.Include(u => u.Topics).FirstOrDefault(u => u.Id == userId);

            return View(model: user);
        }
    }
}