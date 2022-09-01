using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using WarehouseManager.Services.CurrentRequestService;

namespace WarehouseManager.Persistence.Context;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer("Server=b-hejazi;Database=WarehouseManager;Trusted_Connection=True;");
        return new ApplicationDbContext(optionsBuilder.Options, new CurrentRequestService(new HttpContextAccessor()));
    }
}