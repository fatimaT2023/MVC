using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using task8.Models;

namespace task8.Data
{
    public class ApplicationDbContext
     : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext( 
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }

        public DbSet<SupplyRequest> SupplyRequests { get; set; }

        public DbSet<RequestItem> RequestItems { get; set; }
    }
}