using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NguyenXuanVinh_Project2.Models;

namespace Project2.Models
{
    public partial class Project2DbContext : DbContext
    {
        public Project2DbContext()
        {
        }

        public Project2DbContext(DbContextOptions<Project2DbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<OrderBook> OrderBooks { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=Project2;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(e => e.BookId)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Picture)
                    .HasMaxLength(100);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.CategoryId);

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.PublisherId);
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.AccountId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Picture)
                    .HasMaxLength(512)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(e => e.TotalMoney)
                      .HasComputedColumnSql("([Quantity]*[Price])", stored: false);
            });

            // ===== SEED DATA =====

            // Category
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Shounen" },
                new Category { CategoryId = 2, CategoryName = "Shoujo" },
                new Category { CategoryId = 3, CategoryName = "Seinen" }
            );

            // Publisher
            modelBuilder.Entity<Publisher>().HasData(
                new Publisher { PublisherId = 1, PublisherName = "Nhà xuất bản Kim Đồng" },
                new Publisher { PublisherId = 2, PublisherName = "Shueisha" },
                new Publisher { PublisherId = 3, PublisherName = "Shogakukan" },
                new Publisher { PublisherId = 4, PublisherName = "Nhà xuất bản Trẻ" }
            );

            // Book
            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    BookId = "M00001",
                    Title = "One Piece",
                    Author = "Eiichiro Oda",
                    Release = 2023,
                    Price = 120000,
                    CategoryId = 1, // Shounen
                    PublisherId = 1, // Kim Đồng
                    Picture = "onepiece.jpg"
                },
                new Book
                {
                    BookId = "M00002",
                    Title = "Jujutsu Kaisen",
                    Author = "Gege Akutami",
                    Release = 2023,
                    Price = 110000,
                    CategoryId = 1, // Shounen
                    PublisherId = 2, // Shueisha
                    Picture = "jujutsukaisen.jpg"
                },
                new Book
                {
                    BookId = "M00003",
                    Title = "Spy x Family",
                    Author = "Tatsuya Endo",
                    Release = 2023,
                    Price = 105000,
                    CategoryId = 2, // Shoujo
                    PublisherId = 3, // Shogakukan
                    Picture = "spyxfamily.webp"
                },
                new Book
                {
                    BookId = "M00004",
                    Title = "Chainsaw Man",
                    Author = "Tatsuki Fujimoto",
                    Release = 2023,
                    Price = 115000,
                    CategoryId = 3, // Seinen
                    PublisherId = 4, // Trẻ
                    Picture = "chainsawman.jpg"
                },
                new Book
                {
                    BookId = "M00005",
                    Title = "Dragon Ball Super",
                    Author = "Akira Toriyama",
                    Release = 2023,
                    Price = 130000,
                    CategoryId = 1, // Shounen
                    PublisherId = 1, // Kim Đồng
                    Picture = "dbs.jpg"
                },
            new Book
            {
                BookId = "M00006",
                Title = "Tokyo Revengers",
                Author = "Ken Wakui",
                Release = 2023,
                Price = 125000,
                CategoryId = 3, // Seinen
                PublisherId = 4, // Trẻ
                Picture = "tokyorevengers.webp"
            },
             new Book
             {
                 BookId = "M00007",
                 Title = "Demon Slayer: Kimetsu no Yaiba",
                 Author = "Koyoharu Gotouge",
                 Release = 2023,
                 Price = 120000,
                 CategoryId = 1, // Shounen
                 PublisherId = 1, // Kim Đồng
                 Picture = "kimet.jpg"
             },
             new Book
             {
                 BookId = "M00008",
                 Title = "My Hero Academia",
                 Author = "Kohei Horikoshi",
                 Release = 2023,
                 Price = 115000,
                 CategoryId = 1, // Shounen
                 PublisherId = 1, // Kim Đồng
                 Picture = "mha.jpg"
             },
             new Book
             {
                 BookId = "M00009",
                 Title = "Attack on Titan",
                 Author = "Hajime Isayama",
                 Release = 2023,
                 Price = 135000,
                 CategoryId = 3, // Seinen
                 PublisherId = 4, // Trẻ
                 Picture = "titan.jpg"
             },
              new Book
              {
                  BookId = "M00010",
                  Title = "Blue Lock",
                  Author = "Muneyuki Kaneshiro & Yusuke Nomura",
                  Release = 2023,
                  Price = 110000,
                  CategoryId = 1, // Shounen
                  PublisherId = 1, // Kim Đồng
                  Picture = "bluelock.png"
              }
            );
        }
    }
}
