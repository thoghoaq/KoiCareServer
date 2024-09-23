using KoiCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KoiCare.Infrastructure.Dependencies.Database
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Pond> Ponds { get; set; }
        public DbSet<SaltRequirement> SaltRequirements { get; set; }
        public DbSet<WaterParameter> WaterParameters { get; set; }
        public DbSet<KoiIndividual> KoiIndividuals { get; set; }
        public DbSet<KoiType> KoiTypes { get; set; }
        public DbSet<PerfectWaterVolume> PerfectWaterVolumes { get; set; }
        public DbSet<FeedingSchedule> FeedingSchedules { get; set; }
        public DbSet<KoiGrowth> KoiGrowths { get; set; }
        public DbSet<FeedCalculation> FeedCalculations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Username).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
                entity.Property(e => e.IdentityId).HasMaxLength(64).IsRequired();
            });
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(32).IsRequired();
            });
            modelBuilder.Entity<Role>()
                .HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" }
                );

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Price).IsRequired();
                entity.Property(e => e.CategoryId).IsRequired();
            });

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.Price).IsRequired();
                entity.Property(e => e.ProductId).IsRequired();
                entity.Property(e => e.OrderId).IsRequired();
            });

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId);

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.CustomerId).IsRequired();
                entity.Property(e => e.Total).IsRequired();
                entity.Property(e => e.OrderDate).IsRequired();
            });

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.CustomerId);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderId);

            modelBuilder.Entity<BlogPost>(entity =>
            {
                entity.Property(e => e.Title).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.AuthorId).IsRequired();
            });

            modelBuilder.Entity<BlogPost>()
                .HasOne(bp => bp.Author)
                .WithMany(u => u.BlogPosts)
                .HasForeignKey(bp => bp.AuthorId);

            modelBuilder.Entity<Pond>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.OwnerId).IsRequired();
            });

            modelBuilder.Entity<Pond>()
                .HasOne(p => p.User)
                .WithMany(u => u.Ponds)
                .HasForeignKey(p => p.OwnerId);

            modelBuilder.Entity<SaltRequirement>(entity =>
            {
                entity.Property(e => e.RequiredAmount).IsRequired();
            });

            modelBuilder.Entity<SaltRequirement>()
                .HasOne(sr => sr.Pond)
                .WithOne(p => p.SaltRequirement)
                .HasForeignKey<SaltRequirement>(sr => sr.PondId);

            modelBuilder.Entity<WaterParameter>(entity =>
            {
                entity.Property(e => e.PondId).IsRequired();
            });

            modelBuilder.Entity<WaterParameter>()
                .HasOne(wp => wp.Pond)
                .WithMany(p => p.WaterParameters)
                .HasForeignKey(wp => wp.PondId);

            modelBuilder.Entity<KoiIndividual>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.PondId).IsRequired();
            });

            modelBuilder.Entity<KoiIndividual>()
                .HasOne(ki => ki.Pond)
                .WithMany(p => p.KoiIndividuals)
                .HasForeignKey(ki => ki.PondId);

            modelBuilder.Entity<KoiIndividual>()
                .HasOne(ki => ki.KoiType)
                .WithMany(s => s.KoiIndividuals)
                .HasForeignKey(ki => ki.KoiTypeId);

            modelBuilder.Entity<KoiType>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<KoiType>()
                .HasMany(s => s.KoiIndividuals)
                .WithOne(ki => ki.KoiType)
                .HasForeignKey(ki => ki.KoiTypeId);

            modelBuilder.Entity<PerfectWaterVolume>()
                .HasOne(kt => kt.KoiType)
                .WithMany(pwv => pwv.PerfectWaterVolumes)
                .HasForeignKey(kt => kt.KoiTypeId);

            modelBuilder.Entity<FeedingSchedule>(entity =>
            {
                entity.Property(e => e.KoiIndividualId).IsRequired();
            });

            modelBuilder.Entity<FeedingSchedule>()
                .HasOne(fs => fs.KoiIndividual)
                .WithMany(ki => ki.FeedingSchedules)
                .HasForeignKey(fs => fs.KoiIndividualId);

            modelBuilder.Entity<KoiGrowth>(entity =>
            {
                entity.Property(e => e.KoiIndividualId).IsRequired();
            });

            modelBuilder.Entity<KoiGrowth>()
                .HasOne(kg => kg.KoiIndividual)
                .WithMany(ki => ki.KoiGrowths)
                .HasForeignKey(kg => kg.KoiIndividualId);

            modelBuilder.Entity<FeedCalculation>(entity =>
            {
                entity.Property(e => e.KoiIndividualId).IsRequired();
            });

            modelBuilder.Entity<FeedCalculation>()
                .HasOne(fc => fc.KoiIndividual)
                .WithMany(ki => ki.FeedCalculations)
                .HasForeignKey(fc => fc.KoiIndividualId);
        }
    }
}
