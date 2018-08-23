using AutoMapper;
using Elements.Common;
using Elements.Data;
using Elements.Models;
using Elements.Services.Public;
using Elements.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elements.Tests.Services.Public.UserServiceTests
{
    [TestClass]
    public class SetAvatarAsync
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
        public async Task SetAvatar_WithOutImage_ShouldSetDefaultAvatar()
        {
            User user1 = new User() { Id = "user1Id", UserName = "User1" };
            User user2 = new User() { Id = "user2Id", UserName = "User2" };

            this.dbContext.Users.Add(user1);
            this.dbContext.Users.Add(user2);

            this.dbContext.SaveChanges();

            var service = new UserService(this.dbContext, mapper);
            string imagePath = null;

            await service.SetAvatarAsync(user1.Id, imagePath);

            var user = this.dbContext.Users.FirstOrDefault(u => u.Id == user1.Id);

            Assert.IsNotNull(user.Avatar);
            Assert.AreEqual(Constants.DefaultAvatar, user.Avatar);
        }

        [TestMethod]
        public async Task SetAvatar_WithImage_ShouldSetCorrectImage()
        {
            User user1 = new User() { Id = "user1Id", UserName = "User1" };
            User user2 = new User() { Id = "user2Id", UserName = "User2" };

            this.dbContext.Users.Add(user1);
            this.dbContext.Users.Add(user2);

            this.dbContext.SaveChanges();

            var service = new UserService(this.dbContext, mapper);
            string imagePath = "testImage";

            await service.SetAvatarAsync(user1.Id, imagePath);

            var user = this.dbContext.Users.FirstOrDefault(u => u.Id == user1.Id);

            Assert.IsNotNull(user.Avatar);
            Assert.AreEqual(imagePath, user.Avatar);
        }
    }
}
