namespace WarehouseManager.Middleware;

public class TimeZoneRequestMiddleware : IMiddleware
{
    private readonly ITimeZoneManager _timeZoneManager;

    public TimeZoneRequestMiddleware(ITimeZoneManager timeZoneManager)
    {
        _timeZoneManager = timeZoneManager;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await _timeZoneManager.UseRequestTimeZoneModifier(context);
        await next(context);
    }
}