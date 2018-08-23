using AutoMapper;
using Elements.Data;
using Elements.Models.Forum;
using Elements.Services.Public;
using Elements.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Elements.Tests.Services.Public.CategoryServiceTests
{
    [TestClass]
    public class GetAllCategoriesForSelectTests
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
        public void GetAllCategoriesForSelect_WithNoCategories_ShouldReturnNone()
        {
            var service = new CategoryService(this.dbContext, mapper);

            var categories = service.GetAllCategoriesForSelect();

            Assert.IsNotNull(categories);
            Assert.AreEqual(0, categories.Count());
        }

        [TestMethod]
        public void GetAllCategoriesForSelect_WithFewCategories_ShouldReturnCorectly()
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

            var categories = service.GetAllCategoriesForSelect();

            Assert.IsNotNull(categories);
            Assert.AreEqual(3, categories.Count());
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, categories.Select(i => i.Id).ToArray());
            CollectionAssert.AreEqual(new[] { "First", "Second", "Third" }, categories.Select(i => i.Name).ToArray());
        }

        [TestMethod]
        public void GetAllActiveCategoriesForSelect_WithFewCategories_ShouldReturnCorectly()
        {
            this.dbContext.ForumCategories.Add(new ForumCategory()
            {
                Id = 1,
                Name = "First",
                Description = "Category",
                CategoryType = ForumCategoryType.Community,
                IsActive = false,
                IsPrivate = false,
                IconUrl = "testUrl1"
            });

            this.dbContext.ForumCategories.Add(new ForumCategory()
            {
                Id = 2,
                Name = "Second",
                Description = "Second Category",
                CategoryType = ForumCategoryType.Community,
                IsActive = false,
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

            var categories = service.GetAllCategoriesForSelect(true);

            Assert.IsNotNull(categories);
            Assert.AreEqual(1, categories.Count());
            CollectionAssert.AreEqual(new[] { 3 }, categories.Select(i => i.Id).ToArray());
            CollectionAssert.AreEqual(new[] { "Third" }, categories.Select(i => i.Name).ToArray());
        }

        [TestMethod]
        public void GetAllInActiveCategoriesForSelect_WithFewCategories_ShouldReturnCorectly()
        {
            this.dbContext.ForumCategories.Add(new ForumCategory()
            {
                Id = 1,
                Name = "First",
                Description = "Category",
                CategoryType = ForumCategoryType.Community,
                IsActive = false,
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

            var categories = service.GetAllCategoriesForSelect(false);

            Assert.IsNotNull(categories);
            Assert.AreEqual(1, categories.Count());
            CollectionAssert.AreEqual(new[] { 1 }, categories.Select(i => i.Id).ToArray());
            CollectionAssert.AreEqual(new[] { "First" }, categories.Select(i => i.Name).ToArray());
        }
    }
}
