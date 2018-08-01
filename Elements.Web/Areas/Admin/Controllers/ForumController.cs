using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elements.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Elements.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class ForumController : Controller
    {
        public ForumController(ElementsContext context)
        {
            this.Context = context;
        }

        public ElementsContext Context { get; private set; }

        public IActionResult AdminPanel()
        {
            return View();
        }

        public IActionResult ManageCategories()
        {
            return View();
        }

        public IActionResult ManageUsers()
        {
            var users = this.Context.Users.Include(u => u.Topics);

            return View(model: users.ToList());
        }
    }
}