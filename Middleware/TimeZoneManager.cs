using Newtonsoft.Json.Linq;
using WarehouseManager.Services.Interfaces;

namespace WarehouseManager.Middleware;

public class TimeZoneManager : ITimeZoneManager
{
    private readonly ICurrentRequestService _currentRequestService;

    public TimeZoneManager(ICurrentRequestService currentRequestService)
    {
        _currentRequestService = currentRequestService;
    }

    public async Task ModifyTimeZones(HttpContext context)
    {
        var bodyAsText = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Position = 0;
        
        var bodyJObj = JObject.Parse(bodyAsText);

        foreach (var jToken in bodyJObj.Properties())
        {
            if (!DateTime.TryParse(jToken.Value.ToString(), out _)) continue;

            var clientZone = TimeZoneInfo.FindSystemTimeZoneById(_currentRequestService.TimeZone);

            var localTime = TimeZoneInfo.ConvertTime(DateTime.Parse(jToken.Value.ToString()), clientZone)
                .ToUniversalTime();
            
            // context.Request.Body.WriteAsync();

            jToken.Value = localTime;

            return;
        }
    }
}