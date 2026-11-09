using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace task7.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<task7.Models.Category7> Categories { get; set; }
        public DbSet<task7.Models.Product7> Products { get; set; }
        public DbSet<task7.Models.Supplier7> Suppliers { get; set; }
        public DbSet<task7.Models.Employee7> Employees { get; set; }
        public DbSet<task7.Models.Order> Orders { get; set; }
        
    }
}
