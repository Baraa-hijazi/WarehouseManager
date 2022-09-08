using WarehouseManager.Middleware.Interfaces;

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
        if (context.Request.Method == HttpMethod.Post.ToString())
        {
            await _timeZoneManager.UseRequestTimeZoneModifier(context);
            await next(context);
        }

        if (context.Request.Method == HttpMethod.Get.ToString())
            await _timeZoneManager.UseResponseTimeZoneModifier(context, next);
    }
}