using KoiCare.Domain.Entities;
using KoiCare.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace KoiCare.Infrastructure.Dependencies.Database
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Gender> Genders { get; set; }
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
        public DbSet<ServingSize> ServingSizes { get; set; }
        public DbSet<KoiGroup> KoiGroups { get; set; }

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
            modelBuilder.Entity<User>()
                .HasOne(u => u.Gender)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.GenderId);

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(32).IsRequired();
            });
            modelBuilder.Entity<Role>()
                .HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" }
                );

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(32).IsRequired();
            });
            modelBuilder.Entity<Gender>()
                .HasData(
                new Gender { Id = 1, Name = "Male" },
                new Gender { Id = 2, Name = "Female" }
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
                entity.Property(e => e.PondId).IsRequired();
            });

            modelBuilder.Entity<FeedingSchedule>()
                .HasOne(fs => fs.Pond)
                .WithMany(ki => ki.FeedingSchedules)
                .HasForeignKey(fs => fs.PondId);

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

            modelBuilder.Entity<KoiGroup>()
                .HasMany(kg => kg.KoiTypes)
                .WithOne(kt => kt.KoiGroup)
                .HasForeignKey(kt => kt.KoiGroupId);

            modelBuilder.Entity<KoiGroup>()
                .HasMany(kg => kg.ServingSizes)
                .WithOne(ss => ss.KoiGroup)
                .HasForeignKey(ss => ss.KoiGroupId);

            modelBuilder.Entity<KoiGroup>()
                .HasData(
                    new KoiGroup { Id = 1, Name = "Nhóm 1" },
                    new KoiGroup { Id = 2, Name = "Nhóm 2" },
                    new KoiGroup { Id = 3, Name = "Nhóm 3" },
                    new KoiGroup { Id = 4, Name = "Nhóm 4" },
                    new KoiGroup { Id = 5, Name = "Nhóm 5" }
                );

            modelBuilder.Entity<KoiType>()
                .HasData(
                    new KoiType { Id = 1, Name = "Kohaku", KoiGroupId = 1, Description = "Kohaku là loại koi có nền trắng với các mảng đỏ (hi thường gọi là “hi”) trên thân. Đây là loại koi cơ bản và được xem là quan trọng nhất trong các dòng cá koi." },
                    new KoiType { Id = 2, Name = "Taisho Sanke (Sanke)", KoiGroupId = 1, Description = "Sanke có nền trắng với các mảng đỏ và đen trên thân. Mảng đen thường tập trung ở phần thân sau và không xuất hiện trên đầu." },
                    new KoiType { Id = 3, Name = "Showa Sanshoku (Showa)", KoiGroupId = 1, Description = "Showa có nền đen với các mảng đỏ và trắng trên thân. Khác với Sanke, Showa có mảng đen (sumi) xuất hiện trên đầu." },
                    new KoiType { Id = 4, Name = "Utsurimono (Utsuri)", KoiGroupId = 1, Description = "Utsuri có nền đen với các mảng màu khác như trắng, đỏ, hoặc vàng. Tùy thuộc vào màu phụ, Utsuri được phân loại thành Shiro Utsuri (đen và trắng), Hi Utsuri (đen và đỏ), hoặc Ki Utsuri (đen và vàng)." },
                    new KoiType { Id = 5, Name = "Bekko", KoiGroupId = 1, Description = "Bekko có nền trắng, đỏ, hoặc vàng với các mảng đen (sumi) trên thân. Loại koi này có ba biến thể chính là Shiro Bekko (trắng và đen), Aka Bekko (đỏ và đen), và Ki Bekko (vàng và đen)." },

                    new KoiType { Id = 6, Name = "Asagi", KoiGroupId = 2, Description = "Asagi có thân màu xanh lơ với các vảy được xếp thành hàng theo kiểu lưới, bụng và các vây thường có màu đỏ hoặc cam." },
                    new KoiType { Id = 12, Name = "Shusui", KoiGroupId = 2, Description = "Shusui là một phiên bản không có vảy của Asagi, với thân màu xanh và các mảng đỏ hoặc cam chạy dọc theo thân và bụng." },

                    new KoiType { Id = 7, Name = "Tancho", KoiGroupId = 3, Description = "Tancho có nền trắng tinh khiết với một đốm đỏ tròn trên đầu, tượng trưng cho lá cờ của Nhật Bản. Đây là một loại cá rất quý hiếm và được yêu thích." },
                    new KoiType { Id = 10, Name = "Ogons", KoiGroupId = 3, Description = "Ogons là dòng koi kim loại với màu sắc sáng bóng, thường có các màu như vàng, bạch kim, hoặc đồng." },

                    new KoiType { Id = 9, Name = "Kawarimono", KoiGroupId = 4, Description = "Kawarimono là một nhóm gồm những loại koi không thuộc các dòng cơ bản khác. Chúng có nhiều hình dạng và màu sắc khác nhau, từ xanh lá, đen, đến bạc." },
                    new KoiType { Id = 11, Name = "Doitsu Koi", KoiGroupId = 4, Description = "Doitsu Koi là một dòng koi không có vảy hoặc chỉ có vảy dọc theo phần lưng. Loại này có thể xuất hiện trong nhiều biến thể như Kohaku, Sanke, và Showa." },

                    new KoiType { Id = 8, Name = "Goshiki", KoiGroupId = 5, Description = "Goshiki có màu nền xanh xám với các vảy màu đen, trắng, đỏ, và xanh. Màu đỏ thường chiếm ưu thế ở phần thân." }
                );

            modelBuilder.Entity<ServingSize>()
                .HasOne(ss => ss.KoiGroup)
                .WithMany(kg => kg.ServingSizes)
                .HasForeignKey(ss => ss.KoiGroupId);

            modelBuilder.Entity<ServingSize>()
                .HasData(
                    new ServingSize
                    {
                        Id = 1,
                        AgeRange = EAgeRange.UnderYear,
                        WeightPercent = 0.04M,
                        FoodDescription = "Hạt nhỏ giàu protein (35-40%) / vitamin tăng cường màu sắc",
                        DailyFrequency = "3",
                        KoiGroupId = 1
                    },
                    new ServingSize
                    {
                        Id = 2,
                        AgeRange = EAgeRange.YearToThree,
                        WeightPercent = 0.015M,
                        FoodDescription = "Hạt protein cao (30-35%), bổ sung vitamin và khoáng chất",
                        DailyFrequency = "2",
                        KoiGroupId = 1
                    },
                    new ServingSize
                    {
                        Id = 3,
                        AgeRange = EAgeRange.AboveThree,
                        WeightPercent = 0.01M,
                        FoodDescription = "Hạt lớn, protein 30%, nhiều chất xơ để hỗ trợ tiêu hóa",
                        DailyFrequency = "1-2",
                        KoiGroupId = 1
                    },

                    new ServingSize
                    {
                        Id = 4,
                        AgeRange = EAgeRange.UnderYear,
                        WeightPercent = 0.04M,
                        FoodDescription = "Hạt protein cao (35-40%), nhiều carotenoid cho màu sắc",
                        DailyFrequency = "3",
                        KoiGroupId = 2
                    },
                    new ServingSize
                    {
                        Id = 5,
                        AgeRange = EAgeRange.YearToThree,
                        WeightPercent = 0.015M,
                        FoodDescription = "Protein 30-35%, bổ sung thêm canxi và chất xơ",
                        DailyFrequency = "2",
                        KoiGroupId = 2
                    },
                    new ServingSize
                    {
                        Id = 6,
                        AgeRange = EAgeRange.AboveThree,
                        WeightPercent = 0.01M,
                        FoodDescription = "Hạt lớn, bổ sung vitamin tăng cường",
                        DailyFrequency = "1-2",
                        KoiGroupId = 2
                    },

                    new ServingSize
                    {
                        Id = 7,
                        AgeRange = EAgeRange.UnderYear,
                        WeightPercent = 0.04M,
                        FoodDescription = "Protein cao (35%), bổ sung vitamin C và D",
                        DailyFrequency = "3",
                        KoiGroupId = 3
                    },
                    new ServingSize
                    {
                        Id = 8,
                        AgeRange = EAgeRange.YearToThree,
                        WeightPercent = 0.015M,
                        FoodDescription = "Protein 30%, bổ sung vi lượng tăng cường độ bóng",
                        DailyFrequency = "2",
                        KoiGroupId = 3
                    },
                    new ServingSize
                    {
                        Id = 9,
                        AgeRange = EAgeRange.AboveThree,
                        WeightPercent = 0.01M,
                        FoodDescription = "Hạt lớn, protein thấp hơn nhưng nhiều xơ",
                        DailyFrequency = "1",
                        KoiGroupId = 3
                    },

                    new ServingSize
                    {
                        Id = 10,
                        AgeRange = EAgeRange.UnderYear,
                        WeightPercent = 0.04M,
                        FoodDescription = "Hạt giàu protein (35-40%), dễ tiêu hóa",
                        DailyFrequency = "3",
                        KoiGroupId = 4
                    },
                    new ServingSize
                    {
                        Id = 11,
                        AgeRange = EAgeRange.YearToThree,
                        WeightPercent = 0.015M,
                        FoodDescription = "Protein 30-35%, bổ sung chất béo cho sự phát triển",
                        DailyFrequency = "2",
                        KoiGroupId = 4
                    },
                    new ServingSize
                    {
                        Id = 12,
                        AgeRange = EAgeRange.AboveThree,
                        WeightPercent = 0.01M,
                        FoodDescription = "Hạt tổng hợp, cân bằng dinh dưỡng",
                        DailyFrequency = "1",
                        KoiGroupId = 4
                    },

                    new ServingSize
                    {
                        Id = 13,
                        AgeRange = EAgeRange.UnderYear,
                        WeightPercent = 0.04M,
                        FoodDescription = "Hạt giàu protein (35-40%), nhiều carotenoid",
                        DailyFrequency = "3",
                        KoiGroupId = 5
                    },
                    new ServingSize
                    {
                        Id = 14,
                        AgeRange = EAgeRange.YearToThree,
                        WeightPercent = 0.015M,
                        FoodDescription = "Protein 30%, vitamin E và A",
                        DailyFrequency = "2",
                        KoiGroupId = 5
                    },
                    new ServingSize
                    {
                        Id = 15,
                        AgeRange = EAgeRange.AboveThree,
                        WeightPercent = 0.01M,
                        FoodDescription = "Hạt bổ sung chất xơ và chất chống oxy hóa",
                        DailyFrequency = "1",
                        KoiGroupId = 5
                    }
                );
        }
    }
}
