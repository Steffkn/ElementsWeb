using AutoMapper;
using Elements.Data;
using Elements.Models.Forum;
using Elements.Services.Admin;
using Elements.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elements.Tests.Services.Admin.ManageCategoriesServiceTests
{
    [TestClass]
    public class DeleteCategoryAsyncTests
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
        public async Task DeleteCategoryAsync_WithNotExistingCategory_ShouldReturnFalse()
        {
            var service = new ManageCategoriesService(this.dbContext, mapper);

            bool result = await service.DeleteCategoryAsync(15);

            Assert.IsFalse(result);

            var dbCategory = this.dbContext.ForumCategories.ToList();

            Assert.IsNotNull(dbCategory);
            Assert.AreEqual(0, dbCategory.Count);
        }

        [TestMethod]
        public async Task DeleteCategoryAsync_WithOneCategory_ShouldReturnTrue()
        {
            ForumCategory category = new ForumCategory() { Id = 1, IsActive = true, Name = "category 1", CategoryType = ForumCategoryType.Community, IconUrl = "url" };

            this.dbContext.ForumCategories.Add(category);
            this.dbContext.SaveChanges();

            var service = new ManageCategoriesService(this.dbContext, mapper);

            bool result = await service.DeleteCategoryAsync(category.Id);

            Assert.IsTrue(result);

            var dbCategory = this.dbContext.ForumCategories.Where(c => c.IsActive == true).ToList();

            Assert.IsNotNull(dbCategory);
            Assert.AreEqual(0, dbCategory.Count);
        }

        [TestMethod]
        public async Task DeleteCategoryAsync_WithManyCategories_ShouldDeleteCorrectly()
        {
            ForumCategory category1 = new ForumCategory() { Id = 1, IsActive = true, Name = "category 1", CategoryType = ForumCategoryType.Community, IconUrl = "url1" };
            ForumCategory category2 = new ForumCategory() { Id = 2, IsActive = true, Name = "category 2", CategoryType = ForumCategoryType.Community, IconUrl = "url2" };
            ForumCategory category3 = new ForumCategory() { Id = 3, IsActive = true, Name = "category 3", CategoryType = ForumCategoryType.Community, IconUrl = "url3" };

            this.dbContext.ForumCategories.Add(category1);
            this.dbContext.ForumCategories.Add(category2);
            this.dbContext.ForumCategories.Add(category3);
            this.dbContext.SaveChanges();

            var service = new ManageCategoriesService(this.dbContext, mapper);

            bool result = await service.DeleteCategoryAsync(category2.Id);

            Assert.IsTrue(result);

            var dbCategory = this.dbContext.ForumCategories.Where(c => c.IsActive == true).ToList();

            Assert.IsNotNull(dbCategory);
            Assert.AreEqual(2, dbCategory.Count);

            CollectionAssert.AreEqual(new[] { 1, 3 }, dbCategory.Select(c => c.Id).ToArray());
            CollectionAssert.AreEqual(new[] { "url1", "url3" }, dbCategory.Select(c => c.IconUrl).ToArray());
            CollectionAssert.AreEqual(new[] { "category 1", "category 3" }, dbCategory.Select(c => c.Name).ToArray());
        }
    }
}
