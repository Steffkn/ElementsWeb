namespace Elements.Web.Areas.Admin.Controllers
{
    using Elements.Services.Admin.Interfaces;
    using Elements.Services.Models.Areas.Admin.BindingModels;
    using Elements.Services.Models.Areas.Admin.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class ForumController : AdminController
    {
        private readonly IManageCategoriesService forumService;

        public ForumController(IManageCategoriesService forumService)
            : base()
        {
            this.forumService = forumService;
        }

        public IActionResult AdminPanel()
        {
            return View();
        }

        public IActionResult ManageCategories()
        {
            var forumCategories = forumService.GetCategories();
            return this.View(model: forumCategories);
        }

        [HttpGet]
        public IActionResult EditCategory(int categoryId)
        {
            CategoryViewModel category = forumService.GetCategoryById(categoryId);
            return this.View(model: category);
        }

        [HttpPost]
        public IActionResult EditCategory(CategoryBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                // TODO: this is not quite right (Topics Count? )
                var categoryViewModel = new CategoryViewModel() { Id = model.Id, Name = model.Name, Description = model.Description };
                return this.View(model: categoryViewModel);
            }

            // TODO: handle the return value
            this.forumService.EditCategory(model);

            return this.RedirectToAction("ManageCategories");
        }

        [HttpPost]
        public IActionResult DeleteCategory(CategoryBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                // TODO: this is not quite right (Topics Count? )
                var categoryViewModel = new CategoryViewModel() { Id = model.Id, Name = model.Name, Description = model.Description };
                return this.View(model: categoryViewModel);
            }

            // TODO: handle the return value
            this.forumService.DeleteCategory(model);

            return this.RedirectToAction("ManageCategories");
        }
    }
}