using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WarehouseManager;

public class TimeZoneFilter : ActionFilterAttribute
{
    private MemoryStream _responseBody = null!;

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _responseBody = new MemoryStream();
        context.HttpContext.Response.Body = _responseBody;
    }

    public override void OnResultExecuted(ResultExecutedContext context)
    {
        string currTimezone = context.HttpContext.Request.Headers["Time-Zone"];
        _responseBody.Seek(0, SeekOrigin.Begin);

        using var sr = new StreamReader(_responseBody);
        var actionResult = sr.ReadToEnd();


        var bodyJObj = JObject.Parse(actionResult);

        foreach (var jToken in bodyJObj.Properties())
        {
            if (!DateTime.TryParse(jToken.Value.ToString(), out _)) continue;

            var clientZone = TimeZoneInfo.FindSystemTimeZoneById(currTimezone);

            var localTime = TimeZoneInfo.ConvertTime(DateTime.Parse(jToken.Value.ToString()), clientZone)
                .ToUniversalTime();

            jToken.Value = localTime;
        }

        actionResult = JsonConvert.SerializeObject(bodyJObj);
        var requestData = Encoding.UTF8.GetBytes(actionResult);

        context.HttpContext.Response.Body = new MemoryStream(requestData);
        // context.HttpContext.Response.ContentLength = context.HttpContext.Response.Body.Length;

        base.OnResultExecuted(context);
        // return context.Result;
    }
}