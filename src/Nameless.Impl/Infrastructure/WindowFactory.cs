using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Nameless.Infrastructure;
using WinMessageBox = System.Windows.MessageBox;

namespace Nameless.Impl.Infrastructure {
    public sealed class WindowFactory : IWindowFactory {
        #region Private Read-Only Fields

        private readonly IServiceProvider _serviceProvider;

        #endregion

        #region Public Constructors

        public WindowFactory(IServiceProvider serviceProvider) {
            ArgumentNullException.ThrowIfNull(serviceProvider, nameof(serviceProvider));

            _serviceProvider = serviceProvider;
        }

        #endregion

        #region IWindowFactory Members

        public bool TryCreate<TWindow>(Window? owner, object? dataContext, [NotNullWhen(returnValue: true)] out TWindow? window)
            where TWindow : Window {
            window = _serviceProvider.GetService<TWindow>();

            if (window is null) {
                WinMessageBox.Show(owner: Application.Current.MainWindow!,
                                   caption: "Serviço Indisponível",
                                   messageBoxText: "O serviço requisitado não foi registrado no contêiner.",
                                   button: MessageBoxButton.OK,
                                   icon: MessageBoxImage.Warning);

                return false;
            }

            window.Owner = owner ?? Application.Current.MainWindow;
            window.DataContext = dataContext;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            return true;
        }

        public TWindow Create<TWindow>(Window? owner, object? dataContext, params object[] parameters)
            where TWindow : Window {
            TWindow? window;

            try {
                window = Activator.CreateInstance(type: typeof(TWindow),
                                                  bindingAttr: BindingFlags.Public,
                                                  binder: null,
                                                  args: parameters,
                                                  culture: null) as TWindow;
            }
            catch (Exception ex) {
                WinMessageBox.Show(owner: Application.Current.MainWindow!,
                                   caption: "Erro",
                                   messageBoxText: ex.Message,
                                   button: MessageBoxButton.OK,
                                   icon: MessageBoxImage.Error);
                throw;
            }

            if (window is null) {
                throw new NullReferenceException($"Window {typeof(TWindow).Name} could not be created.");
            }

            window.Owner = owner;
            window.DataContext = dataContext;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            return window;
        }

        #endregion
    }
}
