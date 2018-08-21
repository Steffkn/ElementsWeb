using System.Linq;
using Elements.Common;
using Elements.Services.Admin.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Elements.Web.Areas.Admin.Pages
{
    [Authorize(Policy = Constants.PolicyRequireAdminRole)]
    public class MainPanel : PageModel
    {
        private readonly IManageUsersService usersService;

        public MainPanel(IManageUsersService usersService)
        {
            this.usersService = usersService;
        }

        public int TotalUsers { get; set; }

        public void OnGet()
        {
            var users = this.usersService.GetUsers();

            this.TotalUsers = users.Count();
        }
    }
}