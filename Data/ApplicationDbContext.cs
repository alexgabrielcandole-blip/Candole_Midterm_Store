using Lastname_Midterm_Store.Models;
using Microsoft.EntityFrameworkCore;

namespace Candole_Midterm_Store.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
    }
}