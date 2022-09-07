namespace WarehouseManager.Middleware;

public interface ITimeZoneManager
{
    Task UseRequestTimeZoneModifier(HttpContext context);
    Task UseResponseTimeZoneModifier(HttpContext context, RequestDelegate next);
}