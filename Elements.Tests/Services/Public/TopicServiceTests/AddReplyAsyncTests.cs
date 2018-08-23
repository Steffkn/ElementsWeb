using Elements.Data;
using Elements.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elements.Tests.Services.Public.TopicServiceTests
{
    [TestClass]
    public class AddReplyAsync
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
            var service = new TopicService(this.dbContext, mapper);

            var categories = service.GetAllActiveCategoriesInGroups();

            Assert.IsNotNull(categories);
            Assert.AreEqual(0, categories.Count());
        }

    }
}
