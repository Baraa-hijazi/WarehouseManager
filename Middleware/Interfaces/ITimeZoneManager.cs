namespace WarehouseManager.Middleware.Interfaces;

public interface ITimeZoneManager
{
    Task UseRequestTimeZoneModifier(HttpContext context);
    Task UseResponseTimeZoneModifier(HttpContext context, RequestDelegate next);
}