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

    public TimeZoneManager(ICurrentRequestService currentRequestService)
    {
        _currentRequestService = currentRequestService;
    }

    public async Task UseRequestTimeZoneModifier(HttpContext context)
    {
        context.Request.EnableBuffering();

        var bodyStr = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Position = 0;

        if (string.IsNullOrEmpty(bodyStr))
        {
            // _responseBody = Manager.GetStream();
            // context.Response.Body = _responseBody;
            return;
        }

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
    }

    public async Task UseResponseTimeZoneModifier(HttpContext context, RequestDelegate next)
    {
        var originalBodyStream = context.Response.Body;

        await using var responseBody = Manager.GetStream( );
        context.Response.Body = responseBody;

        await next(context);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        
       var responseBodyStr = await new StreamReader(context.Response.Body).ReadToEndAsync();

        context.Response.Body.Seek(0, SeekOrigin.Begin);

        if (string.IsNullOrEmpty(responseBodyStr)) return;


        var bodyJObj = JObject.Parse(responseBodyStr);


        // var tokens = AllTokens(jsonObj);
        // var titles = tokens.Where(t => t.Type == JTokenType.Date && (DateTime)((JProperty)t).Value == DateTime.Now);

        // WalkNode(node);

        // WalkNode(node, n =>
        // {
        //     var token = n;
        //
        //     if (token is not { Type: JTokenType.Date }) return;
        //
        //     var clientZone = TimeZoneInfo.FindSystemTimeZoneById(_currentRequestService.TimeZone);
        //
        //     var localTime = TimeZoneInfo.ConvertTime(DateTime.Parse(token.ToString()), clientZone)
        //         .ToUniversalTime();
        //     
        //     ((JProperty)token.Last!).Value = localTime;
        // });


        foreach (var jToken in bodyJObj.Properties())
        {
            if (!DateTime.TryParse(jToken.Value.ToString(), out _)) continue;

            var clientZone = TimeZoneInfo.FindSystemTimeZoneById(_currentRequestService.TimeZone);

            var localTime = TimeZoneInfo.ConvertTime(DateTime.Parse(jToken.Value.ToString()), clientZone)
                .ToUniversalTime();

            jToken.Value = localTime;
        }

        responseBodyStr = JsonConvert.SerializeObject(bodyJObj);

        var responseData = Encoding.UTF8.GetBytes(responseBodyStr);
        
        context.Response.Body = Manager.GetStream(responseData);

        await context.Response.Body.CopyToAsync(originalBodyStream);
    }

    // IEnumerable<JToken> AllTokens(JObject obj)
    // {
    //     var toSearch = new Stack<JToken>(obj.Children());
    //     while (toSearch.Count > 0)
    //     {
    //         var inspected = toSearch.Pop();
    //         yield return inspected;
    //         foreach (var child in inspected)
    //         {
    //             toSearch.Push(child);
    //         }
    //     }
    // }
    //
    // private static JObject WalkNode(JToken node)
    // {
    //     JObject result;
    //     if (node.Type is JTokenType.Property or JTokenType.Object)
    //     {
    //         foreach (var child in node)
    //             if (child.HasValues || child.Type == JTokenType.Date)
    //             {
    //                 result = WalkNode(child);
    //             }
    //     }
    //     else if (node.Type == JTokenType.Date)
    //     {
    //         var clientZone = TimeZoneInfo.FindSystemTimeZoneById("Jordan Standard Time");
    //
    //         var localTime = TimeZoneInfo.ConvertTime(DateTime.Parse(node.ToString()), clientZone)
    //             .ToUniversalTime();
    //
    //         // JValue.Parse(result).//LastOrDefault() = localTime;
    //     }
    //
    //     return (JObject)node;
    // }
    //
    //
    // private void ConvertJObjectToLocalTimeZone(JToken bodyJObj)
    // {
    //     if (bodyJObj.Values().ToList().Count == 1)
    //     {
    //         if (!DateTime.TryParse(bodyJObj.Values().SingleOrDefault()?.ToString(), out _)) return;
    //
    //         var clientZone = TimeZoneInfo.FindSystemTimeZoneById("Jordan Standard Time");
    //
    //         var localTime = TimeZoneInfo
    //             .ConvertTime(DateTime.Parse(bodyJObj.Values().SingleOrDefault()?.ToString() ?? string.Empty),
    //                 clientZone)
    //             .ToUniversalTime();
    //
    //         ((JProperty)bodyJObj).Value = localTime;
    //     }
    //
    //     if (bodyJObj.Last != null)
    //     {
    //         ConvertJObjectToLocalTimeZone(bodyJObj.Last);
    //     }
    // }

    // private void ModifyUrlTimeZones(HttpContext context)
    // {
    //     foreach (var query in context.Request.Query)
    //     {
    //         if (!DateTime.TryParse(query.Value, out _)) return;
    //
    //         var clientZone = TimeZoneInfo.FindSystemTimeZoneById(_currentRequestService.TimeZone);
    //
    //         var localTime = TimeZoneInfo.ConvertTime(DateTime.Parse(query.Value.ToString()), clientZone)
    //             .ToUniversalTime();
    //     }
    // }
}