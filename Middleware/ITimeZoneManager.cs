namespace WarehouseManager.Middleware;

public interface ITimeZoneManager
{
    Task ModifyRequestTimeZones(HttpContext context);
    Task ModifyResponseTimeZones(HttpContext context);
}