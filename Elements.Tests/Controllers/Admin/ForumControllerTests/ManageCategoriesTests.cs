using Elements.Services.Admin.Interfaces;
using Elements.Services.Models.Areas.Admin.ViewModels;
using Elements.Services.Public.Interfaces;
using Elements.Web.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Elements.Tests.Controllers.Admin.ForumControllerTests
{
    [TestClass]
    public class ManageCategoriesTests
    {
        [TestMethod]
        public void ManageCategories_RetursViewResult()
        {
            var categoriesService = new Mock<IManageCategoriesService>();
            var categoryService = new Mock<ICategoryService>();

            var controller = new ForumController(categoriesService.Object, categoryService.Object);

            var result = controller.ManageCategories();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var resultModel = result as ViewResult;
            Assert.IsNotNull(resultModel.Model);
        }

        [TestMethod]
        public void ManageCategories_RetursCorrectModel()
        {
            var manageCategoriesService = new Mock<IManageCategoriesService>();
            var categoryService = new Mock<ICategoryService>();
            CategoryViewModel categoryViewModel = new CategoryViewModel()
            {
                Id = 2,
                IsActive = true,
                Name = "ala bala"
            };

            manageCategoriesService.Setup(service => service.GetCategories())
                .Returns(new List<CategoryViewModel>()
                {
                    categoryViewModel
                });

            var controller = new ForumController(manageCategoriesService.Object, categoryService.Object);

            var result = controller.ManageCategories();

            var resultModel = result as ViewResult;
            Assert.IsNotNull(resultModel.Model);
            Assert.IsInstanceOfType(resultModel.Model, typeof(IEnumerable<CategoryViewModel>));

            var category = (resultModel.Model as IEnumerable<CategoryViewModel>).FirstOrDefault();
            Assert.AreEqual(categoryViewModel.Id, category.Id);
            Assert.AreEqual(categoryViewModel.IsActive, category.IsActive);
            Assert.AreEqual(categoryViewModel.Name, category.Name);
        }

        [TestMethod]
        public void ManageCategories_CallsGetCategories()
        {
            var manageCategoriesService = new Mock<IManageCategoriesService>();
            var categoryService = new Mock<ICategoryService>();
            var methodCalled = false;
            manageCategoriesService.Setup(service => service.GetCategories())
                .Returns(new List<CategoryViewModel>()
                {
                    new CategoryViewModel()
                    {
                        Id = 2,
                        IsActive = true,
                        Name = "ala bala"
                    }
                })
                .Callback(() => methodCalled = true);

            var controller = new ForumController(manageCategoriesService.Object, categoryService.Object);

            var result = controller.ManageCategories();

            Assert.IsTrue(methodCalled);
        }
    }
}
