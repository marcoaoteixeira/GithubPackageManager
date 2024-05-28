using System.Diagnostics;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Nameless.Infrastructure;
using Wpf.Ui.Appearance;

namespace Nameless.Client.ViewModels.Pages {
    public partial class AppConfigurationViewModel : ObservableObject {
        #region Private Read-Only Fields

        private readonly IApplicationContext _applicationContext;
        private readonly IAppConfigurationManager<AppConfiguration> _appConfigurationManager;

        #endregion

        #region Private Fields

        private bool _initialized;

        #endregion

        #region Private Fields for Observables

        [ObservableProperty]
        private ApplicationTheme _currentTheme = ApplicationTheme.Light;

        [ObservableProperty]
        private bool _confirmBeforeExit = true;

        #endregion

        #region Public Properties

        public ApplicationTheme[] AvailableThemes { get; } = [
            ApplicationTheme.Light,
            ApplicationTheme.Dark,
            ApplicationTheme.HighContrast
        ];

        #endregion

        #region Public Constructors

        public AppConfigurationViewModel(IApplicationContext applicationContext, IAppConfigurationManager<AppConfiguration> appConfigurationManager) {
            ArgumentNullException.ThrowIfNull(applicationContext, nameof(applicationContext));
            ArgumentNullException.ThrowIfNull(appConfigurationManager, nameof(appConfigurationManager));

            _applicationContext = applicationContext;
            _appConfigurationManager = appConfigurationManager;

            Initialize();
        }

        #endregion

        #region Private Methods

        private void Initialize() {
            if (_initialized) { return; }

            
            ConfirmBeforeExit = _appConfigurationManager.Current.ConfirmBeforeExit;
            CurrentTheme = Enum.Parse<ApplicationTheme>(_appConfigurationManager.Current.Theme);

            _initialized = true;
        }

        partial void OnCurrentThemeChanged(ApplicationTheme oldValue, ApplicationTheme newValue) {
            if (oldValue == newValue) { return; }

            ApplicationThemeManager.Apply(newValue);
            _appConfigurationManager.Current.Theme = newValue.ToString();
        }

        #endregion

        #region Private Methods for Commands

        [RelayCommand]
        private Task OpenAppDataDirectoryAsync() {
            using var process = Process.Start(fileName: "explorer.exe",
                                              arguments: _applicationContext.AppDataDirectory);

            return Task.CompletedTask;
        }

        [RelayCommand]
        private Task OpenAppLogFileAsync() {
            var logFilePath = Path.Combine(typeof(Root).Assembly.GetDirectoryPath(Root.App.LOG_FILE_NAME));
            using var process = Process.Start(fileName: Root.App.DEFAULT_TEXT_EDITOR,
                                              arguments: logFilePath);

            return Task.CompletedTask;
        }

        #endregion
    }
}
