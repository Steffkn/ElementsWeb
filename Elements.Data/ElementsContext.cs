using Elements.Models;
using Elements.Models.Characters;
using Elements.Models.Forum;
using Elements.Models.Props.Inventories;
using Elements.Models.Props.Items;
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

        public DbSet<Character> Characters { get; set; }

        public DbSet<CharacterInfo> CharacterInfos { get; set; }

        public DbSet<InteractableItem> InteractableItems { get; set; }

        public DbSet<CharacterInventory> CharacterInventories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
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

            builder.Entity<User>()
                .HasMany(u => u.Characters)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Character>()
                    .HasIndex(b => b.Name)
                    .IsUnique();

            builder.Entity<CharacterInfo>()
                .HasOne(chi => chi.Character)
                .WithOne(ch => ch.CharacterInfo)
                .HasForeignKey<Character>(ch => ch.CharacterInfoId);

            builder.Entity<Character>()
                .HasOne(chi => chi.CharacterInfo)
                .WithOne(ch => ch.Character)
                .HasForeignKey<CharacterInfo>(chi => chi.CharacterId);

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

            builder.Entity<CharacterInventory>()
                .HasKey((item) => new { item.CharacterId, item.Slot });

            builder.Entity<CharacterInventory>()
                .HasOne(chItem => chItem.Character)
                .WithMany(ch => ch.Inventory)
                .HasForeignKey(chItem => chItem.CharacterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CharacterInventory>()
                .HasOne(chItem => chItem.Item)
                .WithMany(item => item.Characters)
                .HasForeignKey(chItem => chItem.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
