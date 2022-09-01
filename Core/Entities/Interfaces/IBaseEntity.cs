namespace WarehouseManager.Core.Entities.Interfaces;

public interface IBaseEntity
{
    string? CreatedBy { get; set; }

    DateTime CreatedOn { get; set; }

    string? UpdatedBy { get; set; }

    DateTime? UpdatedOn { get; set; }

    bool IsDeleted { get; set; }
}