using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Nameless {
    public static class ServiceProviderExtension {
        #region Public Static Methods

        public static ILogger<TCategory> GetLogger<TCategory>(this IServiceProvider self) {
            var loggerFactory = self.GetService<ILoggerFactory>();

            return loggerFactory is not null
                ? loggerFactory.CreateLogger<TCategory>()
                : NullLogger<TCategory>.Instance;
        }

        #endregion
    }
}
