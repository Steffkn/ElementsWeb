using Elements.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Elements.Tests.Mocks
{
    public class FakeDBContext : ElementsContext
    {
        public FakeDBContext(DbContextOptions<ElementsContext> options, IConfiguration configuration)
            : base(options, configuration)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
