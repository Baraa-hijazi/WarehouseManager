namespace WarehouseManager.Middleware;

public interface ITimeZoneManager
{
    Task ModifyTimeZones(HttpContext context);
}