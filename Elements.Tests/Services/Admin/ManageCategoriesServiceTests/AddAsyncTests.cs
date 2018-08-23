using AutoMapper;
using Elements.Data;
using Elements.Models;
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
    public class AddAsyncTests
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
        public async Task AddAsync_WithOneCategory_ShouldAddCorrectly()
        {
            ForumCategory category = new ForumCategory() { Id = 1, Name = "category 1", CategoryType = ForumCategoryType.Community, IconUrl = "url" };

            var service = new ManageCategoriesService(this.dbContext, mapper);

            bool result = await service.AddAsync(category);

            Assert.IsTrue(result);

            var dbCategory = this.dbContext.ForumCategories.FirstOrDefault(fc => fc.Id == category.Id);

            Assert.IsNotNull(dbCategory);
            Assert.AreEqual(category.Name, dbCategory.Name);
            Assert.AreEqual(category.IconUrl, dbCategory.IconUrl);
            Assert.AreEqual(category.CategoryType, dbCategory.CategoryType);
        }

        [TestMethod]
        public async Task AddAsync_WithManyCategories_ShouldAddCorrectly()
        {
            ForumCategory category1 = new ForumCategory() { Id = 1, Name = "category 1", CategoryType = ForumCategoryType.Community, IconUrl = "url1" };
            ForumCategory category2 = new ForumCategory() { Id = 2, Name = "category 2", CategoryType = ForumCategoryType.Community, IconUrl = "url2" };
            ForumCategory category3 = new ForumCategory() { Id = 3, Name = "category 3", CategoryType = ForumCategoryType.Community, IconUrl = "url3" };

            var service = new ManageCategoriesService(this.dbContext, mapper);

            bool result1 = await service.AddAsync(category1);
            Assert.IsTrue(result1, "category 1");

            bool result2 = await service.AddAsync(category2);
            Assert.IsTrue(result2, "category 2");

            bool result3 = await service.AddAsync(category3);
            Assert.IsTrue(result3, "category 3");

            var dbCategory = this.dbContext.ForumCategories.ToList();

            Assert.IsNotNull(dbCategory);
            Assert.AreEqual(3, dbCategory.Count);

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, dbCategory.Select(c => c.Id).ToArray());
            CollectionAssert.AreEqual(new[] { "url1", "url2", "url3" }, dbCategory.Select(c => c.IconUrl).ToArray());
            CollectionAssert.AreEqual(new[] { "category 1", "category 2", "category 3" }, dbCategory.Select(c => c.Name).ToArray());
        }
    }
}
