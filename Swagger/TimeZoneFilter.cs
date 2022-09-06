// using Microsoft.AspNetCore.Mvc.Filters;
//
// namespace WarehouseManager;
//
// public class TimeZoneFilter : IActionFilter
// {
//     private MemoryStream _responseBody = null!;
//
//     public void OnActionExecuting(ActionExecutingContext context)
//     {
//         _responseBody = new MemoryStream();
//         context.HttpContext.Response.Body = _responseBody;
//     }
//
//     public void OnActionExecuted(ActionExecutedContext context)
//     {
//     }
//
//     private MemoryStream _responseBody = null!;
//
//     public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
//     {
//     _responseBody = new MemoryStream();
//     context.HttpContext.Response.Body = _responseBody;
//     await next();
//     }
//
//     public override void OnResultExecuted(ResultExecutedContext context)
//     {
//     string currTimezone = context.HttpContext.Request.Headers["Time-Zone"];
//     _responseBody.Seek(0, SeekOrigin.Begin);
//     
//     using var sr = new StreamReader(_responseBody);
//     var actionResult = sr.ReadToEnd();
//     
//     var bodyJObj = JObject.Parse(actionResult);
//     
//     foreach (var jToken in bodyJObj.Properties())
//     {
//         if (!DateTime.TryParse(jToken.Value.ToString(), out _)) continue;
//     
//         var clientZone = TimeZoneInfo.FindSystemTimeZoneById("Jordan Standard Time");
//     
//         var localTime = TimeZoneInfo.ConvertTime(DateTime.Parse(jToken.Value.ToString()), clientZone)
//             .ToUniversalTime();
//     
//         jToken.Value = localTime;
//     }
//     
//     actionResult = JsonConvert.SerializeObject(bodyJObj);
//     var requestData = Encoding.UTF8.GetBytes(actionResult);
//     
//     // context.HttpContext.Response.Body = new MemoryStream(requestData);
//     
//     var rere = new MemoryStream(requestData);
//     rere.CopyTo(context.HttpContext.Response.Body);
//
//
//     context.HttpContext.Response.Body.CopyTo();
//
//     context.HttpContext.Response.ContentLength = context.HttpContext.Response.Body.Length;
//     context.HttpContext.Response.Body.Dispose();
//
//     base.OnResultExecuted(context);
//
//     Log("OnResultExecuted", context.RouteData);
//     }
//
//     private void Log(string methodName, RouteData routeData)
//     {
//         var controllerName = routeData.Values["controller"];
//         var actionName = routeData.Values["action"];
//         var message = $"{methodName} controller:{controllerName} action:{actionName}";
//         Debug.WriteLine(message, "Action Filter Log");
//     }
// }