using System.Linq.Expressions;
using EMGATA.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EMGATA.API.Data;
public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Brand> Brands { get; set; }
    public DbSet<Model> Models { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<CarImage> CarImages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Brand configuration
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
        });

        // Model configuration
        modelBuilder.Entity<Model>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);

            entity.HasOne(d => d.Brand)
                .WithMany(p => p.Models)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Car configuration
        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Year).IsRequired();
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(2000);

            // Year validation (2010 minimum)
            entity.HasCheckConstraint("CK_Car_Year", "Year >= 2010");

            entity.HasOne(d => d.Model)
                .WithMany(p => p.Cars)
                .HasForeignKey(d => d.ModelId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.User)
                .WithMany(p => p.Cars)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // CarImage configuration
        modelBuilder.Entity<CarImage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ImageUrl).IsRequired().HasMaxLength(500);

            entity.HasOne(d => d.Car)
                .WithMany(p => p.Images)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity && (
                e.State == EntityState.Added ||
                e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            var entity = (BaseEntity)entityEntry.Entity;

            if (entityEntry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }
            entity.UpdatedAt = DateTime.UtcNow;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}