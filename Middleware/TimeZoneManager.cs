using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WarehouseManager.Services.Interfaces;

namespace WarehouseManager.Middleware;

public class TimeZoneManager : ITimeZoneManager
{
    private readonly ICurrentRequestService _currentRequestService;
    private MemoryStream _responseBody;

    public TimeZoneManager(ICurrentRequestService currentRequestService)
    {
        _currentRequestService = currentRequestService;
        _responseBody = new MemoryStream();
    }

    public async Task ModifyRequestTimeZones(HttpContext context)
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
        context.Request.Body = new MemoryStream(requestData);
        context.Request.ContentLength = context.Request.Body.Length;

        ModifyUrlTimeZones(context);
    }

    private void ModifyUrlTimeZones(HttpContext context)
    {
        foreach (var query in context.Request.Query)
        {
            if (!DateTime.TryParse(query.Value, out _)) return;

            // var clientZone = TimeZoneInfo.FindSystemTimeZoneById(_currentRequestService.TimeZone);
            //
            // var localTime = TimeZoneInfo.ConvertTime(DateTime.Parse(query.Value.ToString()), clientZone)
            //     .ToUniversalTime();

            // query.Value = localTime;
        }
    }

    public async Task ModifyResponseTimeZones(HttpContext context)
    {
        context.Response.Body = _responseBody;

        _responseBody.Seek(0, SeekOrigin.Begin);

        using var sr = new StreamReader(context.Response.Body);
        
        var bodyStr = await sr.ReadToEndAsync();
        
        context.Response.Body.Position = 0;
        
        var bodyJObj = JObject.Parse((new JObject().ToString()));


        // foreach (var jToken in bodyJObj.Properties())
        // {
        //     if (!DateTime.TryParse(jToken.Value.ToString(), out _)) continue;
        //
        //     var clientZone = TimeZoneInfo.FindSystemTimeZoneById(_currentRequestService.TimeZone);
        //
        //     var localTime = TimeZoneInfo.ConvertTime(DateTime.Parse(jToken.Value.ToString()), clientZone)
        //         .ToUniversalTime();
        //
        //     jToken.Value = localTime;
        // }
        
        // bodyStr = JsonConvert.SerializeObject(bodyJObj);
        // var requestData = Encoding.UTF8.GetBytes(bodyStr);
        // context.Response.Body = new MemoryStream(requestData);
        // context.Response.ContentLength = context.Response.Body.Length;
    }
}