using Microsoft.Extensions.DependencyInjection;
using Nameless.Client.ViewModels.Pages;
using Nameless.Client.ViewModels.Windows;
using Nameless.Client.Views.Pages;
using Nameless.Client.Views.Windows;
using Wpf.Ui;

namespace Nameless.Client {
    public static class ServiceCollectionExtension {
        #region Public Static Methods

        public static IServiceCollection RegisterViews(this IServiceCollection self) {
            return self
                .AddSingleton<INavigationWindow, MainWindow>()
                .AddSingleton<AppConfigurationPage>();
        }

        public static IServiceCollection RegisterViewModels(this IServiceCollection self) {
            return self
                .AddSingleton<MainViewModel>()
                .AddSingleton<AppConfigurationViewModel>();
        }

        #endregion
    }
}
