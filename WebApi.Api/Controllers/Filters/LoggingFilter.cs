using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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
        // Saltar login (lo registra AuthController)
        if (context.Controller.GetType().Name == "AuthController" &&
            context.ActionDescriptor.RouteValues.TryGetValue("action", out var actName) &&
            actName == "Authenticate")
        {
            await next();
            return;
        }

        var sw = Stopwatch.StartNew();
        var user = context.HttpContext.User.Identity?.Name ?? "Anonymous";
        var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString();

        var executed = await next();
        sw.Stop();

        var controllerName = context.Controller.GetType().Name.Replace("Controller", "");
        var actionName = context.ActionDescriptor.RouteValues.TryGetValue("action", out var a) ? a : "unknown";

        var log = new LogEntry
        {
            Level = executed.Exception == null ? "info" : "error",
            Category = controllerName,
            Event = actionName,
            Result = executed.Exception == null ? "success" : "error",
            Username = user,
            Message = executed.Exception == null ? "OK" : executed.Exception.Message,
            RequestData = JsonSerializer.Serialize(context.ActionArguments),
            ResponseData = JsonSerializer.Serialize((executed.Result as ObjectResult)?.Value),
            DurationMs = sw.ElapsedMilliseconds,
            ErrorMessage = executed.Exception?.Message,
            StackTrace = executed.Exception?.StackTrace
        };

        await _loggingService.SaveLog(log);
    }
}