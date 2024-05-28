using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Nameless {
    public static class ServiceCollectionExtension {
        #region Public Static Methods

        /// <summary>
        /// Registers an object to act like an option that will get its values
        /// from <see cref="IConfiguration"/> (using the Bind method).
        /// </summary>
        /// <typeparam name="TOptions">The type of the object.</typeparam>
        /// <param name="self">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection RegisterPocoOptions<TOptions>(this IServiceCollection self, IConfiguration configuration)
            where TOptions : class, new()
            => RegisterPocoOptions(self, configuration, () => new TOptions());

        /// <summary>
        /// Registers an object to act like an option that will get its values
        /// from <see cref="IConfiguration"/> (using the Bind method).
        /// </summary>
        /// <typeparam name="TOptions">The type of the object.</typeparam>
        /// <param name="self">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="optionsProvider">The option provider.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection RegisterPocoOptions<TOptions>(this IServiceCollection self, IConfiguration configuration, Func<TOptions> optionsProvider)
            where TOptions : class {
            if (configuration is null) {
                throw new ArgumentNullException(nameof(configuration));
            }
            if (optionsProvider is null) {
                throw new ArgumentNullException(nameof(optionsProvider));
            }

            var opts = optionsProvider();
            var key = typeof(TOptions).Name
                                      .Replace(Root.Constants.OPTIONS_TOKEN, string.Empty);

            configuration.Bind(key, opts);
            self.AddSingleton(opts);

            return self;
        }

        #endregion
    }
}
