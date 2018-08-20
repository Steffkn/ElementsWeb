namespace Elements.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using Elements.Services.Admin.Interfaces;
    using Elements.Services.Models.Areas.Admin.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : AdminController
    {
        private readonly IManageUsersService usersService;

        public UsersController(IManageUsersService usersService)
            : base()
        {
            this.usersService = usersService;
        }

        [HttpGet]
        public IActionResult ManageUsers()
        {
            IEnumerable<AdministrateUserViewModel> usersWithTopics = this.usersService.GetAllUsersWithTopics();
            return View(model: usersWithTopics);
        }

        [HttpGet]
        public IActionResult Details(string userId)
        {
            AdministrateUserViewModel userWithTopics = this.usersService.GetUserWithTopics(userId);
            return View(model: userWithTopics);
        }
    }
}