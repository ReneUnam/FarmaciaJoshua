using System.Threading.Tasks;
using WebApi.Model;

public interface IMetricService
{
    Task SaveLog(LogEntry log);
    Task SaveMetric(Metric metric);
}
