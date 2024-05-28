using System.Windows;
using System.Windows.Controls;
using Nameless.Client.Views.Pages;
using Wpf.Ui.Controls;

namespace Nameless.Client.ViewModels.Windows {
    public sealed class MainViewModel {
        #region Private Read-Only Fields

        private readonly IApplicationContext _applicationContext;

        #endregion

        #region Private Fields

        private bool _initialized;

        #endregion

        #region Public Properties

        public string AppTitle { get; private set; } = string.Empty;
        public string AppVersion { get; private set; } = string.Empty;
        public NavigationViewItem[] MenuItemsSource { get; private set; } = [];
        public NavigationViewItem[] FooterMenuItemsSource { get; private set; } = [];

        #endregion

        #region Public Constructors

        public MainViewModel(IApplicationContext applicationContext) {
            _applicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));

            Initialize();
        }

        #endregion

        #region Private Static Methods

        private static NavigationViewItem CreateNavigation<TPage>(string title, SymbolRegular icon, int marginBottom = 10)
            where TPage : Page {
            return new NavigationViewItem {
                Content = title,
                Icon = new SymbolIcon(icon, 20d),
                TargetPageType = typeof(TPage),
                Margin = new Thickness(0, 0, 0, marginBottom),
                ToolTip = title,
                FontSize = 14
            };
        }

        #endregion

        #region Private Methods

        private void Initialize() {
            if (_initialized) { return; }

            AppTitle = _applicationContext.AppName;
            AppVersion = _applicationContext.AppVersion.ToSemVer();

            MenuItemsSource = [];

            FooterMenuItemsSource = [
                CreateNavigation<AppConfigurationPage>(title: Root.UI.Navigation.CONFIGURATIONS,
                                                       icon: SymbolRegular.Settings48,
                                                       marginBottom: 0)
            ];

            _initialized = true;
        }

        #endregion
    }
}
