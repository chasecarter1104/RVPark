using ApplicationCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser, Role, string>
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Reservation>()
            .HasMany(r => r.Fees)
            .WithMany(f => f.Reservations)
            .UsingEntity<Dictionary<string, object>>(
                "ReservationFee",
                j => j
                    .HasOne<Fee>()
                    .WithMany()
                    .HasForeignKey("FeeId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Reservation>()
                    .WithMany()
                    .HasForeignKey("ReservationId")
                    .OnDelete(DeleteBehavior.Cascade));

        // Ensuring the Discriminator is set for the custom Role class
        modelBuilder.Entity<Role>()
            .ToTable("AspNetRoles")
            .HasDiscriminator<string>("Discriminator")
            .HasValue<Role>("Role");
    }

}
