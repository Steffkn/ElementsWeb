using Elements.Models;
using Elements.Models.Forum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Elements.Data
{
    public class ElementsContext : IdentityDbContext<User>
    {
        private readonly IConfiguration configuration;

        public ElementsContext(DbContextOptions<ElementsContext> options,
            IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration;
        }

        public DbSet<ForumCategory> ForumCategories { get; set; }

        public DbSet<Reply> Replies { get; set; }

        public DbSet<Topic> Topics { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasMany(u => u.Topics)
                .WithOne(t => t.Author)
                .HasForeignKey(t => t.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>()
                .HasMany(u => u.Replies)
                .WithOne(r => r.Author)
                .HasForeignKey(r => r.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Topic>()
                .HasMany(t => t.Replies)
                .WithOne(r => r.Topic)
                .HasForeignKey(r => r.TopicId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Topic>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Topics)
                .HasForeignKey(t => t.CategoryId);

            builder.Entity<IdentityRole>().HasData();

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
