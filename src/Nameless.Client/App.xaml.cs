using System.Reflection;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nameless.Bootstrapping;
using Nameless.Impl;
using Nameless.Infrastructure;
using Wpf.Ui;
using ClientRoot = Nameless.Client.Root;
using CoreRoot = Nameless.Root;

namespace Nameless.Client {
    public partial class App : Application {
        #region Private Static Read-Only Fields

        private static readonly Assembly[] SupportAssemblies = [
            typeof(ClientRoot).Assembly,
            typeof(CoreRoot).Assembly,
        ];

        #endregion

        #region Private Read-Only Fields

        private readonly IHost _host = HostFactory
                                       .Create($"--applicationName={ClientRoot.App.NAME}")
                                       .SetConfigureServices(ConfigureServices)
                                       .Build();

        #endregion

        #region Protected Override Methods

        protected override async void OnStartup(StartupEventArgs e) {
            await _host.StartAsync();

            // Execute bootstrap steps.
            await _host.Services
                 .GetRequiredService<IBootstrap>()
                 .RunAsync(CancellationToken.None);

            // Open the main window.
            _host.Services
                 .GetRequiredService<INavigationWindow>()
                 .ShowWindow();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e) {
            await _host.StopAsync();

            _host.Dispose();

            base.OnExit(e);
        }

        #endregion

        #region Private Static Methods

        private static void ConfigureServices(IServiceCollection services)
            => services.RegisterAppConfigurationManager<AppConfiguration>()
                       .RegisterApplicationContext(
                           appName: ClientRoot.App.NAME,
                           appVersion: typeof(App).Assembly
                                                  .GetName()
                                                  .Version ?? new Version())
                       .RegisterBootstrap(SupportAssemblies)
                       .RegisterLogging()
                       .RegisterMessageBoxService()
                       .RegisterNavigationService()
                       .RegisterPageService()
                       .RegisterPerformanceWatcher()
                       .RegisterPubSubService()
                       .RegisterSnackbarService()
                       .RegisterWindowFactory()
                       .RegisterViews()
                       .RegisterViewModels();

        #endregion
    }
}
