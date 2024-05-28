using System.ComponentModel;
using System.Windows.Media.Imaging;
using Microsoft.Extensions.Logging;
using Nameless.Client.Objects;
using Nameless.Client.ViewModels.Windows;
using Nameless.Infrastructure;
using Nameless.MessageBox;
using Nameless.Objects;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace Nameless.Client.Views.Windows {
    public partial class MainWindow : INavigationWindow {
        #region Private Read-Only Fields

        private readonly IAppConfigurationManager<AppConfiguration> _appConfigurationManager;
        private readonly IMessageBoxService _messageBoxService;
        private readonly INavigationService _navigationService;
        private readonly IPageService _pageService;
        private readonly IPubSubService _pubSubService;
        private readonly ISnackbarService _snackbarService;
        private readonly ILogger<MainWindow> _logger;

        #endregion

        #region Private Fields

        private bool _initialized;

        #endregion

        #region Public Properties

        public MainViewModel ViewModel { get; }

        #endregion

        #region Public Constructors

        public MainWindow(MainViewModel viewModel,
            IAppConfigurationManager<AppConfiguration> appConfigurationManager,
            IMessageBoxService messageBoxService,
            INavigationService navigationService,
            IPageService pageService,
            IPubSubService pubSubService,
            ISnackbarService snackbarService,
            ILogger<MainWindow> logger) {
            ArgumentNullException.ThrowIfNull(viewModel, nameof(viewModel));
            ArgumentNullException.ThrowIfNull(appConfigurationManager, nameof(appConfigurationManager));
            ArgumentNullException.ThrowIfNull(messageBoxService, nameof(messageBoxService));
            ArgumentNullException.ThrowIfNull(navigationService, nameof(navigationService));
            ArgumentNullException.ThrowIfNull(pageService, nameof(pageService));
            ArgumentNullException.ThrowIfNull(pubSubService, nameof(pubSubService));
            ArgumentNullException.ThrowIfNull(snackbarService, nameof(snackbarService));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));

            ViewModel = viewModel;

            _appConfigurationManager = appConfigurationManager;
            _messageBoxService = messageBoxService;
            _navigationService = navigationService;
            _pageService = pageService;
            _pubSubService = pubSubService;
            _snackbarService = snackbarService;
            _logger = logger;

            InitializeComponent();
            Initialize();
        }

        #endregion

        #region Private Static Methods

        private static SnackbarInfo CreateSnackbarInfo(SnackbarNotification notification)
            => notification.Level switch {
                Level.Information => new SnackbarInfo(notification.Title ?? "Informação", notification.Message, ControlAppearance.Info, new SymbolIcon(SymbolRegular.Info28)),
                Level.Error => new SnackbarInfo(notification.Title ?? "Erro", notification.Message, ControlAppearance.Danger, new SymbolIcon(SymbolRegular.ThumbDislike24)),
                Level.Success => new SnackbarInfo(notification.Title ?? "Sucesso", notification.Message, ControlAppearance.Success, new SymbolIcon(SymbolRegular.ThumbLike48)),
                Level.Warning => new SnackbarInfo(notification.Title ?? "Aviso", notification.Message, ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning28)),
                _ => new SnackbarInfo(notification.Title ?? "Notificação", notification.Message, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.HandWave24)),
            };

        #endregion

        #region Private Methods

        private void Initialize() {
            if (_initialized) { return; }

            SystemThemeWatcher.Watch(window: this);

            
            ApplicationThemeManager.Apply(
                applicationTheme: Enum.Parse<ApplicationTheme>(value: _appConfigurationManager.Current.Theme)
            );

            _navigationService.SetNavigationControl(navigation: RootNavigationView);
            _snackbarService.SetSnackbarPresenter(contentPresenter: RootSnackbarPresenter);

            _pubSubService.Subscribe<SnackbarNotification>(recipient: this, handler: (_, notification) => SnackbarNotificationHandler(notification: notification));

            SetPageService(pageService: _pageService);

            try { Icon = new BitmapImage(uriSource: new Uri(uriString: "pack://application:,,,/Resources/Images/windows_64x64.png")); }
            catch (Exception ex) { _logger.LogError(exception: ex, message: "Could not load resource windows_64x64.png"); }

            _initialized = true;
        }

        private void SnackbarNotificationHandler(SnackbarNotification notification)
            => Dispatcher.InvokeAsync(() => {
                var snackbarInfo = CreateSnackbarInfo(notification);

                _snackbarService
                    .Show(snackbarInfo.Title,
                          snackbarInfo.Content,
                          snackbarInfo.Appearance,
                          snackbarInfo.Icon,
                          Root.UI.SnackbarTimeout);
            });

        private void ClosingHandler(object? sender, CancelEventArgs args) {
            if (!_appConfigurationManager.Current.ConfirmBeforeExit) { return; }

            var result = _messageBoxService.Show(title: "Sair",
                                                 message: "Perguntar sempre que sair?",
                                                 buttons: MessageBox.MessageBoxButton.YesNoCancel,
                                                 icon: MessageBoxIcon.Question,
                                                 owner: this);

            if (result == MessageBox.MessageBoxResult.No) {
                _appConfigurationManager.Current.ConfirmBeforeExit = false;
            }

            args.Cancel = result == MessageBox.MessageBoxResult.Cancel;
        }

        #endregion

        #region Private Inner Records

        private sealed record SnackbarInfo(
            string Title,
            string Content,
            ControlAppearance Appearance,
            IconElement Icon
        );

        #endregion

        #region INavigationWindow Members

        public INavigationView GetNavigation()
            => RootNavigationView;

        public bool Navigate(Type pageType)
            => RootNavigationView.Navigate(pageType);

        public void SetServiceProvider(IServiceProvider serviceProvider) { }

        public void SetPageService(IPageService pageService)
            => RootNavigationView.SetPageService(pageService);

        public void ShowWindow()
            => Show();

        public void CloseWindow()
            => Close();

        #endregion
    }
}