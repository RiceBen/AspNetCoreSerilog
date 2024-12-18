using Serilog.Core;
using Serilog.Events;

namespace AspNetCoreSerilog.Filters;

public class UtcTimestampEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UtcTimestamp",
            logEvent.Timestamp.ToUniversalTime()));
    }
}