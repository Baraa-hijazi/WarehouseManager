using System.Text;
using Microsoft.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WarehouseManager.Services.Interfaces;

namespace WarehouseManager.Middleware;

public class TimeZoneManager : ITimeZoneManager
{
    private static readonly RecyclableMemoryStreamManager Manager = new();
    private readonly ICurrentRequestService _currentRequestService;
    private MemoryStream _responseBody;

    public TimeZoneManager(ICurrentRequestService currentRequestService)
    {
        _currentRequestService = currentRequestService;
        _responseBody = Manager.GetStream();
    }

    public async Task UseRequestTimeZoneModifier(HttpContext context)
    {
        var bodyStr = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Position = 0;

        var bodyJObj = JObject.Parse(bodyStr);

        foreach (var jToken in bodyJObj.Properties())
        {
            if (!DateTime.TryParse(jToken.Value.ToString(), out _)) continue;

            var clientZone = TimeZoneInfo.FindSystemTimeZoneById(_currentRequestService.TimeZone);

            var localTime = TimeZoneInfo.ConvertTime(DateTime.Parse(jToken.Value.ToString()), clientZone)
                .ToUniversalTime();

            jToken.Value = localTime;
        }

        bodyStr = JsonConvert.SerializeObject(bodyJObj);
        var requestData = Encoding.UTF8.GetBytes(bodyStr);
        context.Request.Body = Manager.GetStream(requestData);
        context.Request.ContentLength = context.Request.Body.Length;

        _responseBody = Manager.GetStream();
        context.Response.Body = _responseBody;
    }

    public async Task UseResponseTimeZoneModifier(HttpResponse response)
    {
        response.Body = _responseBody;
        _responseBody.Seek(0, SeekOrigin.Begin);

        using var sr = new StreamReader(_responseBody);
        var actionResult = await sr.ReadToEndAsync();

        var bodyJObj = JObject.Parse(actionResult);

        foreach (var jToken in bodyJObj.Properties())
        {
            if (!DateTime.TryParse(jToken.Value.ToString(), out _)) continue;

            var clientZone = TimeZoneInfo.FindSystemTimeZoneById("Jordan Standard Time");

            var localTime = TimeZoneInfo.ConvertTime(DateTime.Parse(jToken.Value.ToString()), clientZone)
                .ToUniversalTime();

            jToken.Value = localTime;
        }

        actionResult = JsonConvert.SerializeObject(bodyJObj);
        var requestData = Encoding.UTF8.GetBytes(actionResult);

        response.ContentType = "application/json";
        // response.ContentLength = response.Body.Length;

        response.Body = Manager.GetStream();

        await response.Body.WriteAsync(requestData);
    }

    // private void ModifyUrlTimeZones(HttpContext context)
    // {
    //     foreach (var query in context.Request.Query)
    //     {
    //         // if (!DateTime.TryParse(query.Value, out _)) return;
    //
    //         // var clientZone = TimeZoneInfo.FindSystemTimeZoneById(_currentRequestService.TimeZone);
    //         //
    //         // var localTime = TimeZoneInfo.ConvertTime(DateTime.Parse(query.Value.ToString()), clientZone)
    //         //     .ToUniversalTime();
    //
    //         // query.Value = localTime;
    //     }
    // }


    // private void Log(string methodName, RouteData routeData)
    // {
    //     var controllerName = routeData.Values["controller"];
    //     var actionName = routeData.Values["action"];
    //     var message = $"{methodName} controller:{controllerName} action:{actionName}";
    //     Debug.WriteLine(message, "Action Filter Log");
    // }
}