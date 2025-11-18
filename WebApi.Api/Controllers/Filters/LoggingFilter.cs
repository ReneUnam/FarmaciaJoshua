using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApi.Model;
using System.Text.Json;

public class LoggingFilter : IAsyncActionFilter
{
    private readonly IMetricService _loggingService;

    public LoggingFilter(IMetricService loggingService)
    {
        _loggingService = loggingService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var sw = Stopwatch.StartNew();
        var user = context.HttpContext.User.Identity?.Name ?? "Anonymous";
        var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString();

        var executed = await next();

        sw.Stop();

        var log = new LogEntry
        {
            Level = executed.Exception == null ? "info" : "error",
            Controller = context.Controller.GetType().Name,
            Action = context.ActionDescriptor.DisplayName,
            User = user,
            IpAddress = ip,
            RequestData = JsonSerializer.Serialize(context.ActionArguments),
            ResponseData = JsonSerializer.Serialize((executed.Result as ObjectResult)?.Value),
            DurationMs = sw.ElapsedMilliseconds,
            ErrorMessage = executed.Exception?.Message,
            StackTrace = executed.Exception?.StackTrace
        };

        await _loggingService.SaveLog(log);
    }
}
