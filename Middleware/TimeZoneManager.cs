using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.IO;
using WarehouseManager.Middleware.Interfaces;
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

        context.Request.Body.Seek(0, SeekOrigin.Begin);

        var reqBodyStr = await new StreamReader(context.Request.Body).ReadToEndAsync();

        context.Request.Body.Seek(0, SeekOrigin.Begin);

        if (!string.IsNullOrEmpty(reqBodyStr))
        {
            var reqBody = JsonSerializer.Deserialize<Dictionary<string, object?>>(reqBodyStr);

            // WalkNode(reqBody, null, prop =>
            // {
            //     if (DateTime.TryParse(prop.Value.ToString(), out _))
            //     {
            //         var clientZone = TimeZoneInfo.FindSystemTimeZoneById(_currentRequestService.TimeZone);
            //
            //         var localTime = TimeZoneInfo.ConvertTime(DateTime.Parse(prop.Value.ToString()), clientZone)
            //             .ToUniversalTime();
            //
            //         prop.Value = localTime;
            //     }
            // });

            reqBodyStr = JsonSerializer.Serialize(reqBody);
            var requestData = Encoding.UTF8.GetBytes(reqBodyStr);
            context.Request.Body = Manager.GetStream(requestData);
            context.Request.ContentLength = context.Request.Body.Length;
        }
    }

    // TODO: PUT IN TRY CATCH
    public async Task UseResponseTimeZoneModifier(HttpContext context, RequestDelegate next)
    {
        var originalBodyStream = context.Response.Body;

        await using var memoryStream = Manager.GetStream();
        context.Response.Body = memoryStream;

        await next(context);

        context.Response.Body.Seek(0, SeekOrigin.Begin);

        var responseBodyStr = await new StreamReader(context.Response.Body).ReadToEndAsync();

        context.Response.Body.Seek(0, SeekOrigin.Begin);

        if (!string.IsNullOrEmpty(responseBodyStr))
        {

            // var aa = JsonDocument.Parse(responseBodyStr);
            // var roE = aa.RootElement;
            // var responseBody = JsonNode.Parse(responseBodyStr);
            // var ro = responseBody?.Root.Root.Root.AsObject();
            // var pa = responseBody?.AsObject(); //.AsValue();
            // var chapters = jsonObj.RootElement.EnumerateObject().ToList();

            var responseBody = JsonSerializer.Deserialize<Dictionary<string, object?>>(responseBodyStr);
            var io = responseBody.GetEnumerator();
            var po = io.Current;


            var jsonObj = JsonDocument.Parse(responseBodyStr);

            var el = jsonObj.RootElement;


            WalkNode(responseBody, null, prop =>
            {
                if (DateTime.TryParse(prop.Value.ToString(), out _))
                {
                    var clientZone = TimeZoneInfo.FindSystemTimeZoneById(_currentRequestService.TimeZone);

                    var localTime = TimeZoneInfo.ConvertTime(DateTime.Parse(prop.Value.ToString()), clientZone)
                        .ToUniversalTime();

                    prop.Value = localTime;
                }
            });

            responseBodyStr = JsonSerializer.Serialize(responseBody);
            var responseData = Encoding.UTF8.GetBytes(responseBodyStr);
            context.Response.Body = Manager.GetStream(responseData);
            await context.Response.Body.CopyToAsync(originalBodyStream);
        }
    }

    private static void WalkNode(JsonProperty node,
        Action<JsonProperty>? objectAction = null,
        Action<JsonProperty>? propertyAction = null)
    {
        if (node.GetType().Assembly.ToString() == JTokenType.Object.ToString())
        {
            objectAction?.Invoke(node);

            foreach (var child in node.Value.EnumerateObject().ToList())
            {
                propertyAction?.Invoke(child);
                WalkNode(child, objectAction, propertyAction);
            }
        }
        else if (node.GetType().Assembly.ToString() == JTokenType.Array.ToString())
        {
            foreach (var child in node.Value.EnumerateObject().ToList())
            {
                WalkNode(child, objectAction, propertyAction);
            }
        }
    }
}