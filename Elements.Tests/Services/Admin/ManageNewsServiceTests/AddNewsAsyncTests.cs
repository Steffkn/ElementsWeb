using AutoMapper;
using Elements.Data;
using Elements.Models;
using Elements.Models.Forum;
using Elements.Services.Admin;
using Elements.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Elements.Tests.Services.Admin.ManageNewsServiceTests
{
    [TestClass]
    public class AddNewsAsyncTests
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
        public async Task AddNewsAsync_WithNullNews_ShouldReturnFalse()
        {
            var service = new ManageNewsService(this.dbContext, mapper);

            Topic news1 = null;

            bool result = await service.AddNewsAsync(news1);
            Assert.IsFalse(result);

            var dbNews = this.dbContext.Topics.FirstOrDefault(fc => fc.Id == 123);
            Assert.IsNull(dbNews);
        }

        [TestMethod]
        public async Task AddNewsAsync_WithNoNews_ShouldAddCorrectly()
        {
            User user1 = new User() { Id = "user1Id", UserName = "User1" };

            ForumCategory forumCategory1 = new ForumCategory()
            {
                Id = 1,
                Name = "First",
                Description = "Category",
                CategoryType = ForumCategoryType.News,
                IsActive = true,
                IsPrivate = false,
                IconUrl = "testUrl1"
            };

            DateTime dateTime = new DateTime(1000000000000000000);

            string[] newsTitles = new[] { "Some title", "Some more title", "Third one" };
            string[] newsContents = new[] { "some Content", "Some more Content", "Third one is a charm" };

            this.dbContext.Users.Add(user1);
            this.dbContext.ForumCategories.Add(forumCategory1);

            this.dbContext.SaveChanges();

            var service = new ManageNewsService(this.dbContext, mapper);

            Topic news1 = new Topic()
            {
                Id = 1,
                AuthorId = user1.Id,
                CategoryId = forumCategory1.Id,
                Content = newsContents[0],
                CreateDate = dateTime,
                Title = newsTitles[0],
                TopicType = TopicType.News,
                IsActive = true,
                ImageUrl = "testUrl1"
            };

            bool result = await service.AddNewsAsync(news1);
            Assert.IsTrue(result);

            var dbNews = this.dbContext.Topics.FirstOrDefault(fc => fc.Id == news1.Id);

            Assert.IsNotNull(dbNews);
            Assert.AreEqual(news1.Id, dbNews.Id);
            Assert.AreEqual(news1.AuthorId, dbNews.AuthorId);
            Assert.AreEqual(news1.CategoryId, dbNews.CategoryId);
            Assert.AreEqual(news1.Title, dbNews.Title);
            Assert.AreEqual(news1.TopicType, dbNews.TopicType);
            Assert.AreEqual(news1.ImageUrl, dbNews.ImageUrl);
        }
    }
}
