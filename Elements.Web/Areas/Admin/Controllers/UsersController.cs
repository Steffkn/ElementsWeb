using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Elements.Data;
using Elements.Web.Areas.Admin.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Elements.Web.Areas.Admin.Controllers
{
    public class UsersController : AdminController
    {
        private readonly IMapper mapper;

        public UsersController(ElementsContext context, IMapper mapper) : base(context)
        {
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult ManageUsers()
        {
            var users = this.mapper.Map<IEnumerable<AdministrateUserViewModel>>(this.Context.Users.Include(u => u.Topics));

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