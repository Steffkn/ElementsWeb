using AutoMapper;
using Elements.Data;
using Elements.Models;
using Elements.Models.Forum;
using Elements.Services.Public;
using Elements.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Elements.Tests.Services.Public.NewsServiceTests
{
    [TestClass]
    public class GetNewsDevTopicsWithAuthorsTests
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
        public void GetNewsDevTopicsWithAuthors_WithZeroCount_ShouldReturnNone()
        {
            var service = new NewsService(this.dbContext, mapper);

            var topics = service.GetNewsDevTopicsWithAuthors(0);

            Assert.IsNotNull(topics);
            Assert.AreEqual(0, topics.Count());
        }

        [TestMethod]
        public void GetNewsDevTopicsWithAuthors_WithFewNews_ShouldReturnCorrectly()
        {
            User user1 = new User() { Id = "user1Id", UserName = "User1" };
            User user2 = new User() { Id = "user2Id", UserName = "User2" };

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

            Topic news1 = new Topic()
            {
                Id = 1,
                AuthorId = user1.Id,
                CategoryId = 4,
                Content = newsContents[0],
                CreateDate = dateTime,
                Title = newsTitles[0],
                TopicType = TopicType.News,
                IsActive = true,
                ImageUrl = "testUrl1"
            };

            Topic news2 = new Topic()
            {
                Id = 2,
                AuthorId = user1.Id,
                CategoryId = 4,
                Content = newsContents[1],
                CreateDate = dateTime,
                Title = newsTitles[1],
                TopicType = TopicType.News,
                IsActive = true,
                ImageUrl = "testUrl2"
            };

            Topic news3 = new Topic()
            {
                Id = 3,
                AuthorId = user2.Id,
                CategoryId = 4,
                Content = newsContents[2],
                CreateDate = dateTime,
                Title = newsTitles[2],
                TopicType = TopicType.News,
                IsActive = true,
                ImageUrl = "testUrl2"
            };

            this.dbContext.Users.Add(user1);
            this.dbContext.Users.Add(user2);

            this.dbContext.ForumCategories.Add(forumCategory1);

            this.dbContext.Topics.Add(news1);
            this.dbContext.Topics.Add(news2);
            this.dbContext.Topics.Add(news3);

            this.dbContext.SaveChanges();

            var service = new NewsService(this.dbContext, mapper);

            var topics = service.GetNewsDevTopicsWithAuthors(5);

            Assert.IsNotNull(topics);
            Assert.AreEqual(3, topics.Count());
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, topics.Select(i => i.Id).ToArray());
            CollectionAssert.AreEqual(new[] { "Some title", "Some more title", "Third one" }, topics.Select(i => i.Title).ToArray());
        }

        [TestMethod]
        public void GetNewsDevTopicsWithAuthors_WithMoreThanCountNews_ShouldReturnCorrectly()
        {
            User user1 = new User() { Id = "user1Id", UserName = "User1" };
            User user2 = new User() { Id = "user2Id", UserName = "User2" };

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

            Topic news1 = new Topic()
            {
                Id = 1,
                AuthorId = user1.Id,
                CategoryId = 4,
                Content = newsContents[0],
                CreateDate = dateTime,
                Title = newsTitles[0],
                TopicType = TopicType.News,
                IsActive = true,
                ImageUrl = "testUrl1"
            };

            Topic news2 = new Topic()
            {
                Id = 2,
                AuthorId = user1.Id,
                CategoryId = 4,
                Content = newsContents[1],
                CreateDate = dateTime,
                Title = newsTitles[1],
                TopicType = TopicType.News,
                IsActive = true,
                ImageUrl = "testUrl2"
            };

            Topic news3 = new Topic()
            {
                Id = 3,
                AuthorId = user2.Id,
                CategoryId = 4,
                Content = newsContents[2],
                CreateDate = dateTime,
                Title = newsTitles[2],
                TopicType = TopicType.News,
                IsActive = true,
                ImageUrl = "testUrl2"
            };

            this.dbContext.Users.Add(user1);
            this.dbContext.Users.Add(user2);

            this.dbContext.ForumCategories.Add(forumCategory1);

            this.dbContext.Topics.Add(news1);
            this.dbContext.Topics.Add(news2);
            this.dbContext.Topics.Add(news3);

            this.dbContext.SaveChanges();

            var service = new NewsService(this.dbContext, mapper);

            const int maxCountTopicsToTake = 2;
            const int topicsExpected = 2;

            var topics = service.GetNewsDevTopicsWithAuthors(maxCountTopicsToTake);

            var expectedTitleCollection = newsTitles.Take(topicsExpected).ToList();

            Assert.IsNotNull(topics);
            Assert.AreEqual(topicsExpected, topics.Count());
            CollectionAssert.AreEqual(new[] { 1, 2 }, topics.Select(i => i.Id).ToArray());
            CollectionAssert.AreEqual(expectedTitleCollection, topics.Select(i => i.Title).ToArray());
        }

        [TestMethod]
        public void GetNewsDevTopicsWithAuthors_WithNewsAndDev_ShouldReturnCorrectly()
        {
            User user1 = new User() { Id = "user1Id", UserName = "User1" };
            User user2 = new User() { Id = "user2Id", UserName = "User2" };

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

            Topic news1 = new Topic()
            {
                Id = 1,
                AuthorId = user1.Id,
                CategoryId = 4,
                Content = newsContents[0],
                CreateDate = dateTime,
                Title = newsTitles[0],
                TopicType = TopicType.Development,
                IsActive = true,
                ImageUrl = "testUrl1"
            };

            Topic news2 = new Topic()
            {
                Id = 2,
                AuthorId = user1.Id,
                CategoryId = 4,
                Content = newsContents[1],
                CreateDate = dateTime,
                Title = newsTitles[1],
                TopicType = TopicType.Development,
                IsActive = true,
                ImageUrl = "testUrl2"
            };

            Topic news3 = new Topic()
            {
                Id = 3,
                AuthorId = user2.Id,
                CategoryId = 4,
                Content = newsContents[2],
                CreateDate = dateTime,
                Title = newsTitles[2],
                TopicType = TopicType.News,
                IsActive = true,
                ImageUrl = "testUrl2"
            };

            this.dbContext.Users.Add(user1);
            this.dbContext.Users.Add(user2);

            this.dbContext.ForumCategories.Add(forumCategory1);

            this.dbContext.Topics.Add(news1);
            this.dbContext.Topics.Add(news2);
            this.dbContext.Topics.Add(news3);

            this.dbContext.SaveChanges();

            var service = new NewsService(this.dbContext, mapper);

            const int maxCountTopicsToTake = 2;
            const int topicsExpected = 2;

            var topics = service.GetNewsDevTopicsWithAuthors(maxCountTopicsToTake);

            var expectedTitleCollection = newsTitles.Take(topicsExpected).ToList();

            Assert.IsNotNull(topics);
            Assert.AreEqual(topicsExpected, topics.Count());
            CollectionAssert.AreEqual(new[] { 1, 2 }, topics.Select(i => i.Id).ToArray());
            CollectionAssert.AreEqual(expectedTitleCollection, topics.Select(i => i.Title).ToArray());
        }
    }
}
