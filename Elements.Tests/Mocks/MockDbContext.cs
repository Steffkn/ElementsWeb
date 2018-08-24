namespace Elements.Tests.Mocks
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Elements.Data;

    public static class MockDbContext
    {
        public static ElementsContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ElementsContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

            return new FakeDBContext(options, null);
        }
    }
}
