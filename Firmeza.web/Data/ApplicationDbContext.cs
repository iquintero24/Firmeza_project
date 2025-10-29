using Firmeza.web.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.web.Data;

public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
{
    public DbSet<Customer>  Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Sale>  Sales { get; set; }
    public DbSet<SaleDetail> SaleDetails { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
    {
        
    }
}