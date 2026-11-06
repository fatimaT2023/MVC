
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CafeteriaCodeFirst.Models;

namespace CafeteriaCodeFirst.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PurchaseOrderItem>()
                .HasOne(p => p.PurchaseOrder)
                .WithMany()
                .HasForeignKey(p => p.PurchaseOrderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PurchaseOrderItem>()
                .HasOne(p => p.Product)
                .WithMany()
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(p => p.UnitPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Product>()
                .ToTable(t =>
                {
                    t.HasCheckConstraint("CK_Products_Price", "[Price] >= 0");
                    t.HasCheckConstraint("CK_Products_Quantity", "[Quantity] >= 0");
                });

            modelBuilder.Entity<PurchaseOrderItem>()
                .ToTable(t =>
                {
                    t.HasCheckConstraint("CK_OrderItems_Quantity", "[Quantity] > 0");
                    t.HasCheckConstraint("CK_OrderItems_UnitPrice", "[UnitPrice] >= 0");
                });
        }
    }
}
