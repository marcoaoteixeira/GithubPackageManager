using System.Reflection;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nameless.Bootstrapping;
using Nameless.Impl.Bootstrapping;
using Nameless.Impl.Infrastructure;
using Nameless.Impl.MessageBox;
using Nameless.Infrastructure;
using Nameless.MessageBox;
using NLog.Extensions.Logging;
using Wpf.Ui;

namespace Nameless.Impl {
    public static class ServiceCollectionExtension {
        #region Public Static Methods

        public static IServiceCollection RegisterApplicationContext(this IServiceCollection self, string appName, Version appVersion, bool useSpecialFolder = true)
            => self.AddSingleton<IApplicationContext>(new ApplicationContext(appName, appVersion, useSpecialFolder));

        public static IServiceCollection RegisterAppConfigurationManager<TAppConfiguration>(this IServiceCollection self)
            where TAppConfiguration : AppConfiguration, new()
            => self.AddSingleton<IAppConfigurationManager<TAppConfiguration>, AppConfigurationManager<TAppConfiguration>>();

        public static IServiceCollection RegisterBootstrap(this IServiceCollection self, Assembly[] supportAssemblies) {
            var types = supportAssemblies.SelectMany(assembly => assembly.GetExportedTypes())
                                         .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                                                        typeof(IStep).IsAssignableFrom(type))
                                         .ToArray();

            foreach (var type in types) {
                self.AddTransient(typeof(IStep), type);
            }

            return self.AddTransient<IBootstrap>(provider => {
                var steps = provider.GetRequiredService<IEnumerable<IStep>>();
                var logger = provider.GetLogger<Bootstrap>();

                return new Bootstrap([.. steps], logger);
            });
        }

        public static IServiceCollection RegisterLogging(this IServiceCollection self)
            => self
                .AddLogging(configure => {
                    configure.ClearProviders();
                    configure.AddNLog();
                });


        public static IServiceCollection RegisterMessageBoxService(this IServiceCollection self)
            => self.AddSingleton<IMessageBoxService, MessageBoxService>();

        public static IServiceCollection RegisterNavigationService(this IServiceCollection self)
            => self.AddSingleton<INavigationService, NavigationService>();

        public static IServiceCollection RegisterPageService(this IServiceCollection self)
            => self.AddSingleton<IPageService, PageService>();

        public static IServiceCollection RegisterPerformanceWatcher(this IServiceCollection self)
            => self.AddSingleton<IPerformanceReporter, PerformanceReporter>();

        public static IServiceCollection RegisterPubSubService(this IServiceCollection self)
            => self.AddSingleton<IPubSubService>(new PubSubService(new WeakReferenceMessenger()));

        public static IServiceCollection RegisterSnackbarService(this IServiceCollection self)
            => self.AddSingleton<ISnackbarService, SnackbarService>();

        public static IServiceCollection RegisterWindowFactory(this IServiceCollection self)
            => self.AddSingleton<IWindowFactory, WindowFactory>();

        #endregion
    }
}
