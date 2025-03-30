using ApplicationCore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Reservation> Reservation { get; set; }
    public DbSet<Site> Site { get; set; }
    public DbSet<SiteType> SiteType { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<Fee> Fee { get; set; }

}
