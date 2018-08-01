namespace Elements.Web.Areas.Admin.Controllers
{
    using Elements.Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public abstract class AdminController : Controller
    {
        public AdminController(ElementsContext context)
        {
            this.Context = context;
        }

        public ElementsContext Context { get; private set; }
    }
}