namespace WarehouseManager.Middleware;

public interface ITimeZoneManager
{
    Task ModifyRequestTimeZones(HttpContext context);
    Task ModifyResponseTimeZones(HttpResponse context);
}