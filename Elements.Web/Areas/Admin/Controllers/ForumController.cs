﻿namespace Elements.Web.Areas.Admin.Controllers
{
    using Elements.Models.Forum;
    using Elements.Services.Admin.Interfaces;
    using Elements.Services.Models.Areas.Admin.BindingModels;
    using Elements.Services.Models.Areas.Admin.ViewModels;
    using Elements.Services.Models.Forum.ViewModels;
    using Elements.Services.Public.Interfaces;
    using Elements.Web.Common;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class ForumController : AdminController
    {
        private readonly IManageCategoriesService manageCategories;
        private readonly ICategoryService categoryService;

        public ForumController(IManageCategoriesService manageCategories, ICategoryService categoryService)
            : base()
        {
            this.manageCategories = manageCategories;
            this.categoryService = categoryService;
        }

        public IActionResult ManageCategories()
        {
            var forumCategories = manageCategories.GetCategories();
            return this.View(model: forumCategories);
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            var mainCategoriesSelectViewModel =
                CategoryTypesManager.GetAllExcept(ForumCategoryType.News, ForumCategoryType.Development)
                    .Select(c => new SelectCategoryViewModel() { Id = (int)c, Name = c.ToString() });

            var viewModel = new AddCategoryViewModel()
            {
                MainCategories = mainCategoriesSelectViewModel
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(AddCategoryBindingModel model)
        {
            if (CustomValidator.IsFormFileLenghtBiggerThan(model.ImageFile, 500000))
            {
                this.ModelState.AddModelError(string.Empty, "Please select smaller image (png only)");
            }

            if (!CustomValidator.IsFormFileInFormat(model.ImageFile, "image/png"))
            {
                this.ModelState.AddModelError(string.Empty, "Please select a valid image file (png only)");
            }

            if (!this.ModelState.IsValid)
            {
                var mainCategoriesSelectViewModel =
                    CategoryTypesManager.GetAllExcept(ForumCategoryType.News, ForumCategoryType.Development)
                        .Select(c => new SelectCategoryViewModel() { Id = (int)c, Name = c.ToString() });

                var addCategoryViewModel = new AddCategoryViewModel()
                {
                    Name = model.Name,
                    Description = model.Description,
                    ImageFile = model.ImageFile,
                    MainCategories = mainCategoriesSelectViewModel,
                    MainCategoryId = model.MainCategoryId
                };

                return this.View(model: addCategoryViewModel);
            }

            var fileName = ImageManager.GetNewFileName(model.ImageFile.FileName);
            var iconUrl = ImageManager.GetIconRelativePath("icons", fileName);

            ForumCategory forumCategory = new ForumCategory()
            {
                Name = System.Net.WebUtility.HtmlEncode(model.Name),
                Description = System.Net.WebUtility.HtmlEncode(model.Description),
                IconUrl = iconUrl,
                CategoryType = (ForumCategoryType)model.MainCategoryId,
                IsActive = true,
                IsPrivate = false,
            };

            var result = manageCategories.AddAsync(forumCategory);

            var fullFilePathName = ImageManager.GetFullFilePath("icons", fileName);
            await ImageManager.UploadFileAsync(fullFilePathName, model.ImageFile);

            return this.RedirectToAction("ManageCategories");
        }

        [HttpGet]
        public IActionResult EditCategory(int id)
        {
            var category = manageCategories.GetCategoryById<EditCategoryViewModel>(id);

            var mainCategoriesSelectViewModel =
                CategoryTypesManager.GetAllExcept(ForumCategoryType.News, ForumCategoryType.Development)
                    .Select(c => new SelectCategoryViewModel() { Id = (int)c, Name = c.ToString() });
            category.MainCategories = mainCategoriesSelectViewModel.ToList();

            return this.View(model: category);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(EditCategoryBindingModel model)
        {
            if (model.ImageFile != null)
            {
                if (CustomValidator.IsFormFileLenghtBiggerThan(model.ImageFile, 500000))
                {
                    this.ModelState.AddModelError(string.Empty, "Please select smaller image (png only)");
                }

                if (!CustomValidator.IsFormFileInFormat(model.ImageFile, "image/png"))
                {
                    this.ModelState.AddModelError(string.Empty, "Please select a valid image file (png only)");
                }
            }

            if (!this.ModelState.IsValid)
            {
                var mainCategoriesSelectViewModel =
                    CategoryTypesManager.GetAllExcept(ForumCategoryType.News, ForumCategoryType.Development)
                        .Select(c => new SelectCategoryViewModel() { Id = (int)c, Name = c.ToString() });

                var editCategoryViewModel = new EditCategoryViewModel()
                {
                    Name = model.Name,
                    Description = model.Description,
                    ImageFile = model.ImageFile,
                    MainCategories = mainCategoriesSelectViewModel,
                    MainCategoryId = model.MainCategoryId,
                    IsPrivate = model.IsPrivate,
                    IsActive = model.IsActive,
                };

                return this.View(model: editCategoryViewModel);
            }

            string fileName = string.Empty;

            if (model.ImageFile != null)
            {
                fileName = ImageManager.GetNewFileName(model.ImageFile.FileName);
                var iconUrl = ImageManager.GetIconRelativePath("icons", fileName);

                model.IconUrl = iconUrl;
            }

            // TODO: handle the return value

            model.Name = System.Net.WebUtility.HtmlEncode(model.Name);
            model.Description = System.Net.WebUtility.HtmlEncode(model.Description);

            var result = await this.manageCategories.EditCategoryAsync(model);

            if (model.ImageFile != null)
            {
                var fullFilePathName = ImageManager.GetFullFilePath("icons", fileName);

                using (var fileStream = new FileStream(fullFilePathName, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(fileStream);
                }
            }

            if (!result)
            {
                var mainCategoriesSelectViewModel =
                    CategoryTypesManager.GetAllExcept(ForumCategoryType.News, ForumCategoryType.Development)
                        .Select(c => new SelectCategoryViewModel() { Id = (int)c, Name = c.ToString() });

                var editCategoryViewModel = new EditCategoryViewModel()
                {
                    Name = model.Name,
                    Description = model.Description,
                    ImageFile = model.ImageFile,
                    MainCategories = mainCategoriesSelectViewModel,
                    MainCategoryId = model.MainCategoryId,
                    IsPrivate = model.IsPrivate
                };

                this.ModelState.AddModelError(string.Empty, "Something went wrong while saving!");
                return this.View(model: editCategoryViewModel);
            }

            return this.RedirectToAction("ManageCategories");
        }

        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            var result = this.manageCategories.DeleteCategoryAsync(id);

            return this.RedirectToAction("ManageCategories");
        }
    }
}