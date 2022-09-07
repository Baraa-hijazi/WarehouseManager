// namespace WarehouseManager.Middleware;
//
// public class TimeZoneResponseMiddleware : IMiddleware
// {
//     private readonly ITimeZoneManager _timeZoneManager;
//
//     public TimeZoneResponseMiddleware(ITimeZoneManager timeZoneManager)
//     {
//         _timeZoneManager = timeZoneManager;
//     }
//
//     public async Task InvokeAsync(HttpContext context, RequestDelegate next)
//     {
//         await next(context);
//         await _timeZoneManager.UseResponseTimeZoneModifier(context.Response);
//     }
// }

