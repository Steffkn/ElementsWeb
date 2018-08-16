using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Elements.Data;
using Elements.Web.Areas.Admin.Models.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Elements.Web.Areas.Admin.Controllers
{
    public class ForumController : AdminController
    {
        private readonly IMapper mapper;

        public ForumController(ElementsContext context, IMapper mapper) : base(context)
        {
            this.mapper = mapper;
        }

        public IActionResult AdminPanel()
        {
            return View();
        }

        public IActionResult ManageCategories()
        {
            var forumCategories = this.mapper.Map<IEnumerable<CategoryViewModel>>(this.Context.ForumCategories.Include(c => c.Topics));
            return this.View(model: forumCategories);
        }

        [HttpGet]
        public IActionResult EditCategory(int categoryId)
        {
            var categoryFromDb = this.Context.ForumCategories.Include(c => c.Topics).FirstOrDefault(c => c.Id == categoryId);
            var category = this.mapper.Map<CategoryViewModel>(categoryFromDb);

            return this.View(model: category);
        }

        [HttpPost]
        public IActionResult EditCategory(CategoryBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var categoryViewModel = new CategoryViewModel() { Id = model.Id, Name = model.Name, Description = model.Description };
                return this.View(model: categoryViewModel);
            }

            using (this.Context)
            {
                var category = this.Context.ForumCategories.Find(model.Id);
                if (category != null)
                {

                    category.Name = model.Name;
                    category.Description = model.Description;
                    this.Context.SaveChanges();
                }
            }

            return this.RedirectToAction("ManageCategories");
        }

        [HttpPost]
        public IActionResult DeleteCategory(CategoryBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var categoryViewModel = new CategoryViewModel() { Id = model.Id, Name = model.Name, Description = model.Description };
                return this.View(model: categoryViewModel);
            }

            using (this.Context)
            {
                var category = this.Context.ForumCategories.Find(model.Id);
                if (category != null)
                {
                    this.Context.ForumCategories.Remove(category);
                    this.Context.SaveChanges();
                }
            }

            return this.RedirectToAction("ManageCategories");
        }
    }
}