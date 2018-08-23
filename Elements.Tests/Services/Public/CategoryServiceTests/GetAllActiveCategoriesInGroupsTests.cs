using AutoMapper;
using Elements.Data;
using Elements.Models.Forum;
using Elements.Services.Public;
using Elements.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elements.Tests.Services.Public.CategoryServiceTests
{
    [TestClass]
    public class GetAllActiveCategoriesInGroupsTests
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
        public void GetAllActiveCategoriesInGroups_WithNoCategories_ShouldReturnNone()
        {
            var service = new CategoryService(this.dbContext, mapper);

            var categories = service.GetAllActiveCategoriesInGroups();

            Assert.IsNotNull(categories);
            Assert.AreEqual(0, categories.Count());
        }

        [TestMethod]
        public void GetAllActiveCategoriesInSameGroups_WithFewCategories_ShouldReturnCorectly()
        {
            this.dbContext.ForumCategories.Add(new ForumCategory()
            {
                Id = 1,
                Name = "First",
                Description = "Category",
                CategoryType = ForumCategoryType.Community,
                IsActive = true,
                IsPrivate = false,
                IconUrl = "testUrl1"
            });

            this.dbContext.ForumCategories.Add(new ForumCategory()
            {
                Id = 2,
                Name = "Second",
                Description = "Second Category",
                CategoryType = ForumCategoryType.Community,
                IsActive = true,
                IsPrivate = false,
                IconUrl = "testUrl2"
            });

            this.dbContext.ForumCategories.Add(new ForumCategory()
            {
                Id = 3,
                Name = "Third",
                Description = "Third Category",
                CategoryType = ForumCategoryType.Community,
                IsActive = true,
                IsPrivate = false,
                IconUrl = "testUrl3"
            });

            this.dbContext.SaveChanges();

            var service = new CategoryService(this.dbContext, mapper);

            var categories = service.GetAllActiveCategoriesInGroups();

            Assert.IsNotNull(categories);
            Assert.AreEqual(1, categories.Keys.Count);
            Assert.IsNotNull(categories[ForumCategoryType.Community]);
            Assert.AreEqual(3, categories[ForumCategoryType.Community].Count());
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, categories[ForumCategoryType.Community].Select(i => i.Id).ToArray());
            CollectionAssert.AreEqual(new[] { "First", "Second", "Third" }, categories[ForumCategoryType.Community].Select(i => i.Name).ToArray());
        }

        [TestMethod]
        public void GetAllActiveCategoriesInDifferentGroups_WithFewCategories_ShouldReturnCorectly()
        {
            this.dbContext.ForumCategories.Add(new ForumCategory()
            {
                Id = 1,
                Name = "First",
                Description = "Category",
                CategoryType = ForumCategoryType.Community,
                IsActive = true,
                IsPrivate = false,
                IconUrl = "testUrl1"
            });

            this.dbContext.ForumCategories.Add(new ForumCategory()
            {
                Id = 2,
                Name = "Second",
                Description = "Second Category",
                CategoryType = ForumCategoryType.Gameplay,
                IsActive = true,
                IsPrivate = false,
                IconUrl = "testUrl2"
            });

            this.dbContext.ForumCategories.Add(new ForumCategory()
            {
                Id = 3,
                Name = "Third",
                Description = "Third Category",
                CategoryType = ForumCategoryType.Gameplay,
                IsActive = true,
                IsPrivate = false,
                IconUrl = "testUrl3"
            });

            this.dbContext.SaveChanges();

            var service = new CategoryService(this.dbContext, mapper);

            var categories = service.GetAllActiveCategoriesInGroups();

            Assert.IsNotNull(categories);
            Assert.AreEqual(2, categories.Keys.Count);

            Assert.IsNotNull(categories[ForumCategoryType.Community]);
            Assert.AreEqual(1, categories[ForumCategoryType.Community].Count());

            Assert.IsNotNull(categories[ForumCategoryType.Gameplay]);
            Assert.AreEqual(2, categories[ForumCategoryType.Gameplay].Count());

            CollectionAssert.AreEqual(new[] { 1, }, categories[ForumCategoryType.Community].Select(i => i.Id).ToArray());
            CollectionAssert.AreEqual(new[] { "First", }, categories[ForumCategoryType.Community].Select(i => i.Name).ToArray());

            CollectionAssert.AreEqual(new[] { 2, 3 }, categories[ForumCategoryType.Gameplay].Select(i => i.Id).ToArray());
            CollectionAssert.AreEqual(new[] { "Second", "Third" }, categories[ForumCategoryType.Gameplay].Select(i => i.Name).ToArray());
        }
    }
}
