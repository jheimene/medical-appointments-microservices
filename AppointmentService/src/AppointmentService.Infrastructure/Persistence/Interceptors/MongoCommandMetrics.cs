using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace OrderService.Infrastructure.Persistence.Interceptors
{
    public class MongoCommandMetrics
    {
        private static readonly Meter Meter = new("App.Mongo", "1.0.0");
        private static readonly Counter<long> Commands = Meter.CreateCounter<long>("mongo_commands_total");
        private static readonly Histogram<double> DurationMs = Meter.CreateHistogram<double>("mongo_command_duration_ms");

        private readonly ILogger<MongoCommandMetrics>? _logger;
        private readonly ConcurrentDictionary<int, Stopwatch> _timers = new();

        public MongoCommandMetrics(ILogger<MongoCommandMetrics>? logger = null) => _logger = logger;

        public void Configure(MongoClientSettings settings)
        {
            settings.ClusterConfigurator = cb =>
            {
                cb.Subscribe<CommandStartedEvent>(e =>
                {
                    var sw = Stopwatch.StartNew();
                    _timers[e.RequestId] = sw;
                    // (Opcional) log minimal
                    _logger?.LogDebug("Mongo START {Cmd} db={Db}", e.CommandName, e.DatabaseNamespace.DatabaseName);
                    Console.WriteLine($"Mongo START {e.CommandName} db={e.DatabaseNamespace.DatabaseName}");
                });

                cb.Subscribe<CommandSucceededEvent>(e =>
                {
                    if (_timers.TryRemove(e.RequestId, out var sw))
                    {
                        sw.Stop();
                        Record(e.CommandName, e.DatabaseNamespace.DatabaseName, success: true, exceptionType: "none", sw.Elapsed.TotalMilliseconds);
                    }
                });

                cb.Subscribe<CommandFailedEvent>(e =>
                {
                    if (_timers.TryRemove(e.RequestId, out var sw))
                    {
                        sw.Stop();
                        Record(e.CommandName, e.DatabaseNamespace.DatabaseName, success: false, exceptionType: e.Failure?.GetType().Name ?? "unknown", sw.Elapsed.TotalMilliseconds);
                    }
                    _logger?.LogError(e.Failure, "Mongo FAIL {Cmd} db={Db}", e.CommandName, e.DatabaseNamespace.DatabaseName);
                });
            };
        }

        private static void Record(string commandName, string dbName, bool success, string exceptionType, double ms)
        {
            var tags = new TagList
        {
            { "db.system", "mongodb" },
            { "db.name", dbName },
            { "db.operation", commandName },
            { "success", success },
            { "exception_type", exceptionType }
        };
            Commands.Add(1, tags);
            DurationMs.Record(ms, tags);
        }
    }
}
