using System.Threading.Tasks;
using WebApi.Model;

public interface IMetricService
{
    Task SaveLog(LogEntry log);
    Task SaveMetric(Metric metric);
    Task SaveLoginLog(string username, string result, long durationMs, int? roleId, string message, object requestObj, object responseObj, Exception? ex = null);
}
