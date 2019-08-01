namespace Elements.Web.Areas.Admin.Controllers
{
    using Elements.Models;
    using Elements.Services.Admin.Interfaces;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// 
    /// api/users/{userId}
    /// api/users/{userId}/characters
    /// </summary>
    [Route("api/[controller]")]
    public class UsersController
    {
        private readonly IManageUsersService usersService;

        public UsersController(IManageUsersService usersService, UserManager<User> userManager)
        {
            this.usersService = usersService;
        }
    }
}