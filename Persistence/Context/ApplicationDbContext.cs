using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using WarehouseManager.Core.Entities;

namespace WarehouseManager.Persistence.Context;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<WarehouseItem> WarehouseItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        FluentApi();
        SeedData(builder);
    }


    private void SeedData(ModelBuilder builder)
    {
        var hash = new PasswordHasher<User>();

        builder.Entity<User>().HasData(new User
        {
            Id = new Guid().ToString(),
            UserName = "admin",
            NormalizedUserName = "admin".ToUpper(),
            Email = "admin@admin.com",
            NormalizedEmail = "admin@admin.com".ToUpper(),
            EmailConfirmed = true,
            PhoneNumber = "+9999999999",
            PhoneNumberConfirmed = true,
            PasswordHash = hash.HashPassword(null!, "P@$$w0rd")
        });
    }

    private void FluentApi()
    {
    }
}

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer("Server=b-hejazi;Database=WarehouseManager;Trusted_Connection=True;");
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}