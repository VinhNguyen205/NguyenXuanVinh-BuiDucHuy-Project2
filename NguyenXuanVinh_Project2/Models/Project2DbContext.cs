using System;
using Microsoft.EntityFrameworkCore;
using NguyenXuanVinh_Project2.Models;

namespace NguyenXuanVinh_Project2.Models
{
    public partial class Project2DbContext : DbContext
    {
        public Project2DbContext() { }

        public Project2DbContext(DbContextOptions<Project2DbContext> options)
            : base(options) { }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<OrderBook> OrderBooks { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }

        // Thêm mới
        public virtual DbSet<Gift> Gifts { get; set; }
        public virtual DbSet<OrderGift> OrderGifts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=.;Database=Project2;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== ACCOUNT =====
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccountId);
                entity.Property(e => e.AccountId)
                      .HasMaxLength(36)
                      .IsUnicode(false);
                entity.Property(e => e.Username)
                      .HasMaxLength(64)
                      .IsUnicode(false)
                      .IsRequired();
                entity.Property(e => e.Password)
                      .HasMaxLength(256)
                      .IsUnicode(false);
                entity.Property(e => e.Picture)
                      .HasMaxLength(512)
                      .IsUnicode(false);
                entity.Property(e => e.Email)
                      .HasMaxLength(64)
                      .IsUnicode(false);
                entity.Property(e => e.Role)
                      .HasMaxLength(20);
            });

            // ===== CATEGORY =====
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId);
                entity.Property(e => e.CategoryName)
                      .HasMaxLength(100);
            });

            // ===== PUBLISHER =====
            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.HasKey(e => e.PublisherId);
                entity.Property(e => e.PublisherName)
                      .HasMaxLength(200);
                entity.Property(e => e.Phone)
                      .HasMaxLength(30)
                      .IsUnicode(false);
                entity.Property(e => e.Address)
                      .HasMaxLength(200);
            });

            // ===== BOOK =====
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.BookId);
                entity.Property(e => e.BookId)
                      .HasMaxLength(10)
                      .IsUnicode(false);
                entity.Property(e => e.Title)
                      .HasMaxLength(200)
                      .IsRequired();
                entity.Property(e => e.Author)
                      .HasMaxLength(100);
                entity.Property(e => e.Picture)
                      .HasMaxLength(100);

                entity.HasOne(d => d.Category)
                      .WithMany(p => p.Books)
                      .HasForeignKey(d => d.CategoryId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(d => d.Publisher)
                      .WithMany(p => p.Books)
                      .HasForeignKey(d => d.PublisherId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // ===== ORDERBOOK =====
            modelBuilder.Entity<OrderBook>(entity =>
            {
                entity.HasKey(e => e.OrderId);
                entity.Property(e => e.OrderId)
                      .HasMaxLength(16)
                      .IsUnicode(false);
                entity.Property(e => e.Status)
                      .HasMaxLength(16)
                      .IsUnicode(false);
                entity.HasOne(d => d.Account)
                      .WithMany(p => p.OrderBooks)
                      .HasForeignKey(d => d.AccountId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // ===== ORDERDETAIL =====
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailId);
                entity.Property(e => e.TotalMoney)
                      .HasComputedColumnSql("([Quantity]*[Price])", stored: false);
                entity.HasOne(d => d.OrderBook)
                      .WithMany(p => p.OrderDetails)
                      .HasForeignKey(d => d.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(d => d.Book)
                      .WithMany(p => p.OrderDetails)
                      .HasForeignKey(d => d.BookId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== GIFT =====
            modelBuilder.Entity<Gift>(entity =>
            {
                entity.HasKey(e => e.GiftId);

                entity.Property(e => e.GiftName)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(e => e.Price)
                    .HasColumnType("float"); // <- CHỈNH DÒNG NÀY CHO TRÙNG SQL

                entity.Property(e => e.Description)
                    .HasColumnType("ntext");

                entity.Property(e => e.Picture)
                    .HasMaxLength(200);
            });


            // ===== ORDERGIFT =====
            modelBuilder.Entity<OrderGift>(entity =>
            {
                entity.HasKey(e => e.OrderGiftId);

                entity.HasOne(d => d.OrderBook)
                      .WithMany(p => p.OrderGifts)
                      .HasForeignKey(d => d.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Gift)
                      .WithMany(p => p.OrderGifts)
                      .HasForeignKey(d => d.GiftId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== SEED DATA =====
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Shounen" },
                new Category { CategoryId = 2, CategoryName = "Shoujo" },
                new Category { CategoryId = 3, CategoryName = "Seinen" },
                new Category { CategoryId = 4, CategoryName = "Isekai" },
                new Category { CategoryId = 5, CategoryName = "Fantasy" },
                new Category { CategoryId = 6, CategoryName = "Action" },
                new Category { CategoryId = 7, CategoryName = "Adventure " },
                new Category { CategoryId = 8, CategoryName = "LightNovel " }
            );

            modelBuilder.Entity<Publisher>().HasData(
                new Publisher { PublisherId = 1, PublisherName = "Nhà xuất bản Kim Đồng" },
                new Publisher { PublisherId = 2, PublisherName = "Shueisha" },
                new Publisher { PublisherId = 3, PublisherName = "Shogakukan" },
                new Publisher { PublisherId = 4, PublisherName = "Nhà xuất bản Trẻ" }
            );

            // Thay thế phần seed data Book trong Project2DbContext.cs
            // 8 thể loại: 1=Shounen, 2=ShouJo, 3=Seinen, 4=Isekai, 5=Fantasy, 6=Action, 7=Adventure, 8=LightNovel

            modelBuilder.Entity<Book>().HasData(
                // SHOUNEN - Hành động thiếu niên (1)
                new Book { BookId = "M00001", Title = "One Piece", Author = "Eiichiro Oda", Release = 2023, Price = 278000, CategoryId = 1, PublisherId = 1, Picture = "onepiece.jpg" },
                new Book { BookId = "M00002", Title = "Jujutsu Kaisen", Author = "Gege Akutami", Release = 2023, Price = 145000, CategoryId = 1, PublisherId = 2, Picture = "jujutsukaisen.jpg" },
                new Book { BookId = "M00005", Title = "Dragon Ball Super", Author = "Akira Toriyama", Release = 2023, Price = 206000, CategoryId = 1, PublisherId = 1, Picture = "dbs.jpg" },
                new Book { BookId = "M00007", Title = "Demon Slayer: Kimetsu no Yaiba", Author = "Koyoharu Gotouge", Release = 2023, Price = 176000, CategoryId = 1, PublisherId = 1, Picture = "kimet.jpg" },
                new Book { BookId = "M00010", Title = "Blue Lock", Author = "Muneyuki Kaneshiro & Yusuke Nomura", Release = 2023, Price = 138000, CategoryId = 1, PublisherId = 1, Picture = "bluelock.png" },
                new Book { BookId = "M00011", Title = "Naruto", Author = "Masashi Kishimoto", Release = 2023, Price = 289000, CategoryId = 1, PublisherId = 2, Picture = "naruto.jpg" },
                new Book { BookId = "M00014", Title = "Fairy Tail", Author = "Hiro Mashima", Release = 2023, Price = 258000, CategoryId = 1, PublisherId = 1, Picture = "fairytail.jpg" },
                new Book { BookId = "M00016", Title = "The Promised Neverland", Author = "Kaiu Shirai", Release = 2023, Price = 277000, CategoryId = 1, PublisherId = 2, Picture = "neverland.jpg" },
                new Book { BookId = "M00018", Title = "Hunter x Hunter", Author = "Yoshihiro Togashi", Release = 2023, Price = 224000, CategoryId = 1, PublisherId = 2, Picture = "hunter.jpg" },
                new Book { BookId = "M00020", Title = "Assassination Classroom", Author = "Yūsei Matsui", Release = 2023, Price = 298000, CategoryId = 1, PublisherId = 2, Picture = "assclass.jpg" },

                // SHOUJO - Lãng mạn thiếu nữ (2)
                new Book { BookId = "M00022", Title = "Horimiya", Author = "HERO & Daisuke Hagiwara", Release = 2023, Price = 183000, CategoryId = 2, PublisherId = 3, Picture = "horimiya.jpg" },
                new Book { BookId = "M00023", Title = "Ao Haru Ride", Author = "Io Sakisaka", Release = 2023, Price = 142000, CategoryId = 2, PublisherId = 4, Picture = "aoharu.jpg" },
                new Book { BookId = "M00024", Title = "Your Lie in April", Author = "Naoshi Arakawa", Release = 2023, Price = 211000, CategoryId = 2, PublisherId = 4, Picture = "yourlie.jpg" },
                new Book { BookId = "M00025", Title = "Orange", Author = "Ichigo Takano", Release = 2023, Price = 199000, CategoryId = 2, PublisherId = 3, Picture = "orange.jpg" },
                new Book { BookId = "M00026", Title = "Clannad", Author = "Key/VisualArt's", Release = 2023, Price = 276000, CategoryId = 2, PublisherId = 4, Picture = "clannad.jpg" },
                new Book { BookId = "M00021", Title = "Komi Can't Communicate", Author = "Tomohito Oda", Release = 2023, Price = 119000, CategoryId = 2, PublisherId = 3, Picture = "komi.jpg" },

                // SEINEN - Trưởng thành, tối, phức tạp (3)
                new Book { BookId = "M00004", Title = "Chainsaw Man", Author = "Tatsuki Fujimoto", Release = 2023, Price = 254000, CategoryId = 3, PublisherId = 4, Picture = "chainsawman.jpg" },
                new Book { BookId = "M00009", Title = "Attack on Titan", Author = "Hajime Isayama", Release = 2023, Price = 197000, CategoryId = 3, PublisherId = 4, Picture = "titan.jpg" },
                new Book { BookId = "M00015", Title = "Death Note", Author = "Tsugumi Ohba & Takeshi Obata", Release = 2023, Price = 103000, CategoryId = 3, PublisherId = 2, Picture = "deathnote.jpg" },
                new Book { BookId = "M00017", Title = "Fullmetal Alchemist", Author = "Hiromu Arakawa", Release = 2023, Price = 193000, CategoryId = 3, PublisherId = 3, Picture = "fma.jpg" },
                new Book { BookId = "M00027", Title = "Parasyte", Author = "Hitoshi Iwaaki", Release = 2023, Price = 167000, CategoryId = 3, PublisherId = 2, Picture = "parasyte.jpg" },
                new Book { BookId = "M00028", Title = "Erased", Author = "Kei Sanbe", Release = 2023, Price = 248000, CategoryId = 3, PublisherId = 3, Picture = "erased.jpg" },
                new Book { BookId = "M00036", Title = "Goblin Slayer", Author = "Kumo Kagyu", Release = 2023, Price = 163000, CategoryId = 3, PublisherId = 4, Picture = "goblin.jpg" },

                // ISEKAI - Chuyển sinh thế giới khác (4)
                new Book { BookId = "M00031", Title = "Overlord", Author = "Kugane Maruyama", Release = 2023, Price = 185000, CategoryId = 4, PublisherId = 3, Picture = "overlord.jpg" },
                new Book { BookId = "M00032", Title = "Re:Zero", Author = "Tappei Nagatsuki", Release = 2023, Price = 152000, CategoryId = 4, PublisherId = 3, Picture = "rezero.jpg" },
                new Book { BookId = "M00033", Title = "Konosuba", Author = "Natsume Akatsuki", Release = 2023, Price = 223000, CategoryId = 4, PublisherId = 4, Picture = "konosuba.jpg" },
                new Book { BookId = "M00034", Title = "Slime Datta Ken", Author = "Fuse", Release = 2023, Price = 178000, CategoryId = 4, PublisherId = 3, Picture = "slime.jpg" },
                new Book { BookId = "M00035", Title = "Sword Art Online", Author = "Reki Kawahara", Release = 2023, Price = 294000, CategoryId = 4, PublisherId = 3, Picture = "sao.jpg" },
                new Book { BookId = "M00030", Title = "No Game No Life", Author = "Yuu Kamiya", Release = 2023, Price = 297000, CategoryId = 4, PublisherId = 3, Picture = "ngnl.jpg" },

                // FANTASY - Phép thuật, thế giới ảo (5)
                new Book { BookId = "M00003", Title = "Spy x Family", Author = "Tatsuya Endo", Release = 2023, Price = 182000, CategoryId = 5, PublisherId = 3, Picture = "spyxfamily.webp" },
                new Book { BookId = "M00039", Title = "Weathering With You", Author = "Makoto Shinkai", Release = 2023, Price = 277000, CategoryId = 5, PublisherId = 4, Picture = "weathering.jpg" },

                // ACTION - Hành động thuần túy (6)
                new Book { BookId = "M00006", Title = "Tokyo Revengers", Author = "Ken Wakui", Release = 2023, Price = 294000, CategoryId = 6, PublisherId = 4, Picture = "tokyorevengers.webp" },
                new Book { BookId = "M00008", Title = "My Hero Academia", Author = "Kohei Horikoshi", Release = 2023, Price = 231000, CategoryId = 6, PublisherId = 1, Picture = "mha.jpg" },
                new Book { BookId = "M00012", Title = "Bleach", Author = "Tite Kubo", Release = 2023, Price = 214000, CategoryId = 6, PublisherId = 2, Picture = "bleach.jpg" },
                new Book { BookId = "M00013", Title = "Black Clover", Author = "Yūki Tabata", Release = 2023, Price = 169000, CategoryId = 6, PublisherId = 1, Picture = "blackclover.jpg" },
                new Book { BookId = "M00019", Title = "Soul Eater", Author = "Atsushi Ōkubo", Release = 2023, Price = 132000, CategoryId = 6, PublisherId = 4, Picture = "souleater.jpg" },
            
             // ADVENTURE - Phiêu lưu, khám phá (7)
                new Book { BookId = "M00029", Title = "Made in Abyss", Author = "Akihito Tsukushi", Release = 2023, Price = 134000, CategoryId = 7, PublisherId = 2, Picture = "madeinabyss.jpg" },
                new Book { BookId = "M00040", Title = "Children of the Whales", Author = "Abi Umeda", Release = 2023, Price = 122000, CategoryId = 7, PublisherId = 2, Picture = "whales.jpg" },

                // LIGHTNOVEL - Tiểu thuyết nhẹ (8)
                new Book { BookId = "M00037", Title = "Light Novel Violet Evergarden", Author = "Kana Akatsuki", Release = 2023, Price = 196000, CategoryId = 8, PublisherId = 4, Picture = "violet.jpg" },
                new Book { BookId = "M00038", Title = "5 Centimeters per Second", Author = "Makoto Shinkai", Release = 2023, Price = 146000, CategoryId = 8, PublisherId = 4, Picture = "5cm.jpg" }
            );

            // ⚠️ Admin seed (password chỉ demo, nên hash thật)
            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    AccountId = Guid.NewGuid().ToString(),
                    Username = "vinh",
                    Password = "30062005",
                    FullName = "Administrator",
                    Email = "realsteelworld2k5@gmail.com",
                    Role = "Admin",
                    Picture = "admin.jpg",
                    IsAdmin = true,
                    Active = true
                }
            );
        }
        public DbSet<OrderGift> OrderGift { get; set; } = default!;
    }
}
