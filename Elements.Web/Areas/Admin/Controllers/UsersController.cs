namespace Elements.Web.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Elements.Models;
    using Elements.Services.Admin.Interfaces;
    using Elements.Services.Models.Areas.Admin.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : AdminController
    {
        private readonly IManageUsersService usersService;
        private readonly UserManager<User> userManager;

        public UsersController(IManageUsersService usersService, UserManager<User> userManager)
            : base()
        {
            this.usersService = usersService;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> ManageUsers()
        {
            var usersWithTopics = this.usersService.GetAllUsersWithTopics()
                .ToList();

            for (int i = 0; i < usersWithTopics.Count; i++)
            {
                var currentUser = usersWithTopics[i];
                var user = await userManager.FindByIdAsync(currentUser.Id);
                currentUser.Role = userManager.GetRolesAsync(user)
                    .Result.OrderBy(x => x)
                    .FirstOrDefault();
            }

            return this.View(usersWithTopics);
        }

        [HttpGet]
        public IActionResult Details(string username)
        {
            var user = userManager.FindByNameAsync(username).Result;

            UserDetailsViewModel userDetails = this.usersService.GetUserWithTopicsAndReplies(user.Id);

            userDetails.Roles = userManager.GetRolesAsync(user)
                .Result.OrderBy(x => x);

            return this.View(userDetails);
        }

        [HttpPost]
        public IActionResult RestrictUser(string id)
        {
            User user = this.usersService.RestrictUser(id);
            bool result = false;
            if (user != null)
            {
                result = true;
            }

            return Json(new { result, isRestricted = user?.IsRestricted });
        }

        [HttpPost]
        public IActionResult RestoreUser(string id)
        {
            User user = this.usersService.RestoreUser(id);
            bool result = false;
            if (user != null)
            {
                result = true;
            }

            return Json(new { result, isRestricted = user?.IsRestricted });
        }
    }
}