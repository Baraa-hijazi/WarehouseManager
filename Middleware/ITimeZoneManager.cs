namespace WarehouseManager.Middleware;

public interface ITimeZoneManager
{
    Task UseRequestTimeZoneModifier(HttpContext context);
    Task UseResponseTimeZoneModifier(HttpResponse context);
}