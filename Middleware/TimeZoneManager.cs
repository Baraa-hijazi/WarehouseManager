using System.Diagnostics;
using System.Text;
using Microsoft.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        try
        {
            context.Request.EnableBuffering();

            context.Request.Body.Seek(0, SeekOrigin.Begin);

            var reqBodyStr = await new StreamReader(context.Request.Body).ReadToEndAsync();

            context.Request.Body.Seek(0, SeekOrigin.Begin);

            if (!string.IsNullOrEmpty(reqBodyStr))
            {
                var reqBody = JObject.Parse(reqBodyStr);

                WalkNode(reqBody, null, prop =>
                {
                    if (DateTime.TryParse(prop.Value.ToString(), out _))
                    {
                        var clientZone = TimeZoneInfo.FindSystemTimeZoneById(_currentRequestService.TimeZone);

                        var localTime = TimeZoneInfo.ConvertTime(DateTime.Parse(prop.Value.ToString()), clientZone)
                            .ToUniversalTime();

                        prop.Value = localTime;
                    }
                });

                reqBodyStr = JsonConvert.SerializeObject(reqBody);
                var requestData = Encoding.UTF8.GetBytes(reqBodyStr);
                context.Request.Body = Manager.GetStream(requestData);
                context.Request.ContentLength = context.Request.Body.Length;
            }
        }
        catch (Exception)
        {
            Debug.WriteLine(await new StreamReader(context.Request.Body).ReadToEndAsync());
        }
    }

    public async Task UseResponseTimeZoneModifier(HttpContext context, RequestDelegate next)
    {
        try
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
                var responseBody = JObject.Parse(responseBodyStr);

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

                responseBodyStr = JsonConvert.SerializeObject(responseBody);
                var responseData = Encoding.UTF8.GetBytes(responseBodyStr);
                context.Response.Body = Manager.GetStream(responseData);
                await context.Response.Body.CopyToAsync(originalBodyStream);
            }
        }
        catch (Exception)
        {
            Debug.WriteLine(await new StreamReader(context.Request.Body).ReadToEndAsync());
        }
    }

    private static void WalkNode(JToken node,
        Action<JObject>? objectAction = null,
        Action<JProperty>? propertyAction = null)
    {
        if (node.Type == JTokenType.Object)
        {
            objectAction?.Invoke((JObject)node);

            foreach (var child in node.Children<JProperty>())
            {
                propertyAction?.Invoke(child);
                WalkNode(child.Value, objectAction, propertyAction);
            }
        }
        else if (node.Type == JTokenType.Array)
        {
            foreach (var child in node.Children())
            {
                WalkNode(child, objectAction, propertyAction);
            }
        }
    }
}