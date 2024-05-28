using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Nameless.Infrastructure;

namespace Nameless.Impl.Infrastructure {
    public sealed class PerformanceReporter : IPerformanceReporter {
        #region Private Read-Only Fields

        private readonly ILogger _logger;

        #endregion

        #region Public Constructors

        public PerformanceReporter(ILogger<PerformanceReporter> logger) {
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));

            _logger = logger;
        }

        #endregion

        #region Private Inner Classes

        private class StopWatcher : IDisposable {
            #region Private Read-Only Fields
            
            private readonly string _tag;
            private readonly ILogger _logger;

            #endregion

            #region Private Fields
            
            private Stopwatch? _stopwatch;
            private bool _disposed;

            #endregion

            #region Public Constructors
            
            public StopWatcher(string tag, ILogger logger) {
                _tag = tag;
                _logger = logger;

                _stopwatch = Stopwatch.StartNew();

                _logger.LogInformation("[{Tag}]: START! Counting time...", tag);
            }

            #endregion

            #region Private Methods
            
            private void Dispose(bool disposing) {
                if (_disposed) { return; }
                if (disposing && _stopwatch is not null) {
                    _stopwatch.Stop();
                    _logger.LogInformation(message: "[{Tag}]: STOP! Execution time: {ElapsedTime}",
                                           args: new object[] { _tag, _stopwatch.Elapsed });
                }

                _stopwatch = null;
                _disposed = true;
            }

            #endregion

            #region IDisposable Members
            
            public void Dispose() {
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            } 

            #endregion
        }

        #endregion

        #region PerformanceReporter Members

        public IDisposable ReportExecutionTime([CallerMemberName] string? tag = null)
            => new StopWatcher(tag ?? "NULL", _logger);

        #endregion
    }
}
