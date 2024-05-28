using Microsoft.Extensions.Logging;
using Nameless.Bootstrapping;

namespace Nameless.Impl.Bootstrapping {
    public sealed class Bootstrap : IBootstrap {
        #region Private Read-Only Fields

        private readonly IStep[] _steps;
        private readonly ILogger _logger;

        #endregion

        #region Public Constructors

        public Bootstrap(IStep[] steps, ILogger<Bootstrap> logger) {
            ArgumentNullException.ThrowIfNull(steps, nameof(steps));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));

            _steps = steps;
            _logger = logger;
        }

        #endregion

        #region IBootstrap Members

        public async Task RunAsync(CancellationToken cancellationToken) {
            var currentSteps = _steps.Where(step => !step.Skip)
                                     .OrderBy(item => item.Order);
            foreach (var step in currentSteps) {
                if (cancellationToken.IsCancellationRequested) {
                    break;
                }

                try { await step.ExecuteAsync(cancellationToken); }
                catch (Exception ex) {
                    _logger.LogError(exception: ex,
                                     message: "Error while executing step \"{StepName}\"",
                                     args: step.Name);

                    if (step.ThrowOnFailure) {
                        throw;
                    }
                }
            }
        }

        #endregion
    }
}
