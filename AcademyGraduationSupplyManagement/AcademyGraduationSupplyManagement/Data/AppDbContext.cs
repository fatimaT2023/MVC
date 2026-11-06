
using AcademyGraduationSupplyManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AcademyGraduationSupplyManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Offer> Offers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Category (1) -> Offers (Many)
            modelBuilder.Entity<Offer>()
                .HasOne(o => o.Category)
                .WithMany(c => c.Offers)
                .HasForeignKey(o => o.CategoryId);

            // Supplier (1) -> Offer (1)
            modelBuilder.Entity<Offer>()
                .HasOne(o => o.Supplier)
                .WithOne(s => s.Offer)
                .HasForeignKey<Offer>(o => o.SupplierId);

            // UnitPrice precision
            modelBuilder.Entity<Offer>()
                .Property(o => o.UnitPrice)
                .HasPrecision(10, 2);



            // =====================================
            // PHASE 3 - SEED CATEGORIES
            // =====================================

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Graduation Certificates" },
                new Category { Id = 2, Name = "Hospitality" },
                new Category { Id = 3, Name = "Graduation Gifts" }
            );

            // =====================================
            // PHASE 3 - SEED SUPPLIERS
            // =====================================

            modelBuilder.Entity<Supplier>().HasData(
                new Supplier { Id = 1, Name = "Supplier A" },
                new Supplier { Id = 2, Name = "Supplier B" },
                new Supplier { Id = 3, Name = "Supplier C" },
                new Supplier { Id = 4, Name = "Supplier D" },
                new Supplier { Id = 5, Name = "Supplier E" },
                new Supplier { Id = 6, Name = "Supplier F" },
                new Supplier { Id = 7, Name = "Supplier G" },
                new Supplier { Id = 8, Name = "Supplier H" },
                new Supplier { Id = 9, Name = "Supplier I" }
            );

            // =====================================
            // PHASE 3 - SEED OFFERS
            // =====================================

            modelBuilder.Entity<Offer>().HasData(
                new Offer { Id = 1, CategoryId = 1, SupplierId = 1, OfferName = "Premium Certificate Package", Quantity = 35, UnitPrice = 2m },
                new Offer { Id = 2, CategoryId = 1, SupplierId = 2, OfferName = "Standard Certificate Package", Quantity = 35, UnitPrice = 1.5m },
                new Offer { Id = 3, CategoryId = 1, SupplierId = 3, OfferName = "Luxury Certificate Package", Quantity = 35, UnitPrice = 2.5m },

                new Offer { Id = 4, CategoryId = 2, SupplierId = 4, OfferName = "Coffee & Drinks Package", Quantity = 100, UnitPrice = 2m },
                new Offer { Id = 5, CategoryId = 2, SupplierId = 5, OfferName = "Sandwiches & Snacks Package", Quantity = 100, UnitPrice = 3m },
                new Offer { Id = 6, CategoryId = 2, SupplierId = 6, OfferName = "Full Hospitality Package", Quantity = 100, UnitPrice = 4m },

                new Offer { Id = 7, CategoryId = 3, SupplierId = 7, OfferName = "Customized Mugs Package", Quantity = 35, UnitPrice = 2m },
                new Offer { Id = 8, CategoryId = 3, SupplierId = 8, OfferName = "Graduation Bags Package", Quantity = 35, UnitPrice = 3m },
                new Offer { Id = 9, CategoryId = 3, SupplierId = 9, OfferName = "Premium Gift Boxes Package", Quantity = 35, UnitPrice = 4m }
            );

        }
    }
}
