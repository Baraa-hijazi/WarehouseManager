using System.IO.Pipelines;

namespace WarehouseManager.Services.Interfaces;

public interface ICurrentRequestService
{
    string? UserId { get; set; }
    string TimeZone { get; set; }
}