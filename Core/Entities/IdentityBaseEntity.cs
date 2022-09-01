using Microsoft.AspNetCore.Identity;
using WarehouseManager.Core.Entities.Interfaces;

namespace WarehouseManager.Core.Entities;

public abstract class IdentityBaseEntity : IdentityUser, IBaseEntity
{
    public string? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.Now;

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool IsDeleted { get; set; }
}