using System.Security.Claims;
using WarehouseManager.Services.Interfaces;

namespace WarehouseManager.Services.CurrentRequestService;

public class CurrentRequestService : ICurrentRequestService
{
    public CurrentRequestService(IHttpContextAccessor context)
    {
        UserId = context.HttpContext?.User.FindFirstValue("Id") ?? string.Empty;
        TimeZone = context.HttpContext?.Request.Headers["Time-Zone"] ?? string.Empty; //"Jordan Standard Time";
    }

    public string? UserId { get; set; }
    public string TimeZone { get; set; }
}