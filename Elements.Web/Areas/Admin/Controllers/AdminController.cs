namespace Elements.Web.Areas.Admin.Controllers
{
    using Elements.Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area(Constants.AdminAreaName)]
    [Authorize(Roles = Constants.AdminRoleName)]
    public abstract class AdminController : Controller
    {
        public AdminController()
        { }
    }
}