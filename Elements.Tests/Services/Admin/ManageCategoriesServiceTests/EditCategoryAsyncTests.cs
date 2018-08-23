using AutoMapper;
using Elements.Data;
using Elements.Models.Forum;
using Elements.Services.Admin;
using Elements.Services.Models.Areas.Admin.BindingModels;
using Elements.Tests.Mocks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elements.Tests.Services.Admin.ManageCategoriesServiceTests
{
    [TestClass]
    public class EditCategoryAsyncTests
    {
        private ElementsContext dbContext;
        private IMapper mapper;

        [TestInitialize]
        public void TestInitializer()
        {
            this.dbContext = MockDbContext.GetDbContext();
            this.mapper = MockAutoMapper.Get();
        }

        [TestMethod]
        public async Task EditCategoryAsync_WithNotExistingCategory_ShouldReturnFalse()
        {
            var service = new ManageCategoriesService(this.dbContext, mapper);

            var editedCategory = new EditCategoryBindingModel()
            {
                Id = 123,
                Name = "REE",
                IconUrl = "Image",
            };

            bool result = await service.EditCategoryAsync(editedCategory);

            Assert.IsFalse(result);

            var dbCategory = this.dbContext.ForumCategories.ToList();

            Assert.IsNotNull(dbCategory);
            Assert.AreEqual(0, dbCategory.Count);
        }

        public async Task EditCategoryAsync_WithOneCategory_ShouldReturnTrue()
        {
            ForumCategory category = new ForumCategory() { Id = 1, Name = "category 1", CategoryType = ForumCategoryType.Community, IconUrl = "url" };

            this.dbContext.ForumCategories.Add(category);
            this.dbContext.SaveChanges();

            var service = new ManageCategoriesService(this.dbContext, mapper);

            var editedCategory = new EditCategoryBindingModel()
            {
                Id = 1,
                Name = "REE",
                IconUrl = "Image",
            };

            bool result = await service.EditCategoryAsync(editedCategory);

            Assert.IsTrue(result);

            var dbCategories = this.dbContext.ForumCategories.ToList();
            var dbCategory = this.dbContext.ForumCategories.FirstOrDefault();

            Assert.IsNotNull(dbCategories);
            Assert.AreEqual(1, dbCategories.Count);

            Assert.AreEqual(category.Id, dbCategory.Id);
            Assert.AreEqual(category.Name, dbCategory.Name);
            Assert.AreEqual(category.IconUrl, dbCategory.IconUrl);
        }

        public async Task EditCategoryAsync_WithManyCategories_ShouldDeleteCorrectly()
        {
            ForumCategory category1 = new ForumCategory() { Id = 1, Name = "category 1", CategoryType = ForumCategoryType.Community, IconUrl = "url1" };
            ForumCategory category2 = new ForumCategory() { Id = 2, Name = "category 2", CategoryType = ForumCategoryType.Community, IconUrl = "url2" };
            ForumCategory category3 = new ForumCategory() { Id = 3, Name = "category 3", CategoryType = ForumCategoryType.Community, IconUrl = "url3" };

            this.dbContext.ForumCategories.Add(category1);
            this.dbContext.ForumCategories.Add(category2);
            this.dbContext.ForumCategories.Add(category3);
            this.dbContext.SaveChanges();

            var service = new ManageCategoriesService(this.dbContext, mapper);

            var editedCategory1 = new EditCategoryBindingModel()
            {
                Id = 1,
                Name = "REE",
                IconUrl = "Image",
            };

            bool result = await service.EditCategoryAsync(editedCategory1);

            Assert.IsTrue(result);

            var dbCategory = this.dbContext.ForumCategories.ToList();

            Assert.IsNotNull(dbCategory);
            Assert.AreEqual(3, dbCategory.Count);

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, dbCategory.Select(c => c.Id).ToArray());
            CollectionAssert.AreEqual(new[] { "url1", "Image", "url3" }, dbCategory.Select(c => c.IconUrl).ToArray());
            CollectionAssert.AreEqual(new[] { "category 1", "REE", "category 3" }, dbCategory.Select(c => c.Name).ToArray());
        }
    }
}
