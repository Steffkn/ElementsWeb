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
    public class ForumController : AdminController
    {
        public ForumController(ElementsContext context) : base(context)
        { }

        public IActionResult AdminPanel()
        {
            return View();
        }

        public IActionResult ManageCategories()
        {
            return View();
        }
    }
}