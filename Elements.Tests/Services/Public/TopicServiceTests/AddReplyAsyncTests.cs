using AutoMapper;
using Elements.Data;
using Elements.Models;
using Elements.Models.Forum;
using Elements.Services.Public;
using Elements.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elements.Tests.Services.Public.TopicServiceTests
{
    [TestClass]
    public class AddReplyAsyncTests
    {
        private ElementsContext dbContext;
        private IMapper mapper;
        private DateTimeService dataTimeService = new DateTimeService();

        [TestInitialize]
        public void TestInitializer()
        {
            this.dbContext = MockDbContext.GetDbContext();
            this.mapper = MockAutoMapper.Get();
        }

        [TestMethod]
        public async Task AddReplyAsync_WithOneReply_ShouldAddCorrectly()
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
            DateTime dateTime2 = new DateTime(1000000000010000000);

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

            Reply reply = new Reply()
            {
                TopicId = news1.Id,
                AuthorId = user2.Id,
                Content = "Reply content",
            };

            this.dbContext.Users.Add(user1);
            this.dbContext.Users.Add(user2);
            this.dbContext.ForumCategories.Add(forumCategory1);
            this.dbContext.Topics.Add(news1);

            this.dbContext.SaveChanges();

            var service = new TopicService(this.dbContext, mapper, dataTimeService);

            Topic dbTopic = await service.AddReplyAsync(reply);

            Assert.IsNotNull(dbTopic);
            Assert.AreEqual(1, dbTopic.Replies.Count);

            var dbReply = dbTopic.Replies.First();
            Assert.IsNotNull(dbReply);
            Assert.AreEqual(news1.Id, dbReply.TopicId);
            Assert.AreEqual(user2.Id, dbReply.AuthorId);
            Assert.AreEqual(reply.Content, dbReply.Content);
            Assert.IsTrue(dbReply.IsActive);
        }

        [TestMethod]
        public async Task AddReplyAsync_WithExistingReply_ShouldAddCorrectly()
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
            DateTime dateTime2 = new DateTime(1000000000010000000);

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

            Reply reply1 = new Reply()
            {
                TopicId = news1.Id,
                AuthorId = user2.Id,
                Content = "Reply content",
            };

            Reply reply2 = new Reply()
            {
                TopicId = news1.Id,
                AuthorId = user2.Id,
                Content = "It works",
            };

            this.dbContext.Users.Add(user1);
            this.dbContext.Users.Add(user2);
            this.dbContext.ForumCategories.Add(forumCategory1);
            this.dbContext.Topics.Add(news1);

            this.dbContext.SaveChanges();

            var service = new TopicService(this.dbContext, mapper, dataTimeService);

            Topic dbTopic = await service.AddReplyAsync(reply1);
            dbTopic = await service.AddReplyAsync(reply2);

            Assert.IsNotNull(dbTopic);
            Assert.AreEqual(2, dbTopic.Replies.Count);

            var dbReply = dbTopic.Replies.ToList();
            Assert.IsNotNull(dbReply);

            foreach (var reply in dbReply)
            {
                Assert.AreEqual(news1.Id, reply.TopicId);
                Assert.AreEqual(user2.Id, reply.AuthorId);
                Assert.AreEqual(reply.Content, reply.Content);
                Assert.IsTrue(reply.IsActive);
            }
        }

        [TestMethod]
        public async Task AddReplyAsync_WithNullReply_ShouldThrowException()
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
            DateTime dateTime2 = new DateTime(1000000000010000000);

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

            Reply reply = null;

            this.dbContext.Users.Add(user1);
            this.dbContext.Users.Add(user2);
            this.dbContext.ForumCategories.Add(forumCategory1);
            this.dbContext.Topics.Add(news1);

            this.dbContext.SaveChanges();

            var service = new TopicService(this.dbContext, mapper, dataTimeService);

            Func<Task<Topic>> dbTopic = async () => await service.AddReplyAsync(reply);

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(dbTopic);
        }
    }
}
