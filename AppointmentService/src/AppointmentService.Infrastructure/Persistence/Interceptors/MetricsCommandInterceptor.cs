using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using System.Diagnostics;
using System.Threading;

namespace OrderService.Infrastructure.Persistence.Interceptors
{
    public sealed class MetricsCommandInterceptor : DbCommandInterceptor
    {
        private readonly ILogger<MetricsCommandInterceptor> _logger;

        public MetricsCommandInterceptor(ILogger<MetricsCommandInterceptor> logger)
        {
            this._logger = logger;
        }

        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result)
        {
            var sw = Stopwatch.StartNew();
            try { 
                return base.ReaderExecuting(command, eventData, result); 
            }
            finally
            {
                sw.Stop();
                _logger.LogInformation("SQL {Elapsed}ms: {CommandText}", sw.ElapsedMilliseconds, command.CommandText);
                // aquí podrías emitir métricas OTEL
            }
        }

        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
            }
            finally
            {
                sw.Stop();
                _logger.LogInformation("SQL {Elapsed}ms: {CommandText}", sw.ElapsedMilliseconds, command.CommandText);
                // aquí podrías emitir métricas OTEL
            }
        }

        public override int NonQueryExecuted(
            DbCommand command, 
            CommandExecutedEventData eventData, 
            int result)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                return base.NonQueryExecuted(command, eventData, result);
            }
            finally
            {
                sw.Stop();
                _logger.LogInformation("SQL {Elapsed}ms: {CommandText}", sw.ElapsedMilliseconds, command.CommandText);
                // aquí podrías emitir métricas OTEL
            }
        }

        public override async ValueTask<int> NonQueryExecutedAsync(
            DbCommand command,
            CommandExecutedEventData eventData,
            int result,
            CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                return await base.NonQueryExecutedAsync(command, eventData, result, cancellationToken);
            }
            finally
            {
                sw.Stop();
                _logger.LogInformation("SQL {Elapsed}ms: {CommandText}", sw.ElapsedMilliseconds, command.CommandText);
                // aquí podrías emitir métricas OTEL
            }
        }

        public override InterceptionResult<int> NonQueryExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<int> result)
        {
            _logger.LogInformation("Executing command: {CommandText}", command.CommandText);
            return base.NonQueryExecuting(command, eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Executing command: {CommandText}", command.CommandText);
            return base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
        }
    }
}
