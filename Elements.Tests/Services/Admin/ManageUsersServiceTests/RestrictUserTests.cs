using AutoMapper;
using Elements.Data;
using Elements.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Elements.Tests.Services.Admin.ManageUsersServiceTests
{
    [TestClass]
    public class RestrictUserTests
    {
        private ElementsContext dbContext;
        private IMapper mapper;

        [TestInitialize]
        public void TestInitializer()
        {
            this.dbContext = MockDbContext.GetDbContext();
            this.mapper = MockAutoMapper.Get();
        }
    }
}
