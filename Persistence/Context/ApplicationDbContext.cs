using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WarehouseManager.Core.Entities;
using WarehouseManager.Core.Entities.Interfaces;
using WarehouseManager.Services.Interfaces;

namespace WarehouseManager.Persistence.Context;

public class ApplicationDbContext : IdentityDbContext<User>
{
    private readonly ICurrentRequestService _currentRequestService;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        ICurrentRequestService currentRequestService) : base(options)
    {
        _currentRequestService = currentRequestService;
    }

    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Warehouse> Warehouses { get; set; } = null!;
    public DbSet<WarehouseItem> WarehouseItems { get; set; } = null!;

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
            UserName = "admin",
            EmailConfirmed = true,
            Email = "admin@admin.com",
            PhoneNumber = "+9999999999",
            PhoneNumberConfirmed = true,
            Id = Guid.NewGuid().ToString(),
            NormalizedUserName = "admin".ToUpper(),
            NormalizedEmail = "admin@admin.com".ToUpper(),
            PasswordHash = hash.HashPassword(null!, "P@$$w0rd")
        });
    }

    private void FluentApi()
    {
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        // AddAuditInfo();

        return await base.SaveChangesAsync(cancellationToken);
    }

    private void AddAuditInfo()
    {
        foreach (var entry in ChangeTracker.Entries<IBaseEntity>())
        {
            var clientZone = TimeZoneInfo.FindSystemTimeZoneById(_currentRequestService.TimeZone);
            var utcTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, clientZone).ToUniversalTime();

            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentRequestService.UserId;
                    entry.Entity.CreatedOn = utcTime;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedBy = _currentRequestService.UserId;
                    entry.Entity.UpdatedOn = utcTime;
                    break;
                case EntityState.Deleted:
                    entry.Entity.UpdatedBy = _currentRequestService.UserId;
                    entry.Entity.UpdatedOn = utcTime;
                    entry.Entity.IsDeleted = true;
                    break;
            }
        }
    }
}