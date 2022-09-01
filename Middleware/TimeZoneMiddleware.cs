namespace WarehouseManager.Middleware;

public class TimeZoneMiddleware : IMiddleware
{
    private readonly ITimeZoneManager _timeZoneManager;

    public TimeZoneMiddleware(ITimeZoneManager timeZoneManager)
    {
        _timeZoneManager = timeZoneManager;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await _timeZoneManager.ModifyTimeZones(context);
        await next(context);
    }
}