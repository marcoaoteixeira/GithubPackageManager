using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace Nameless.Infrastructure {
    public interface IWindowFactory {
        #region Methods

        bool TryCreate<TWindow>(Window? owner, object? dataContext, [NotNullWhen(returnValue: true)]out TWindow? window)
            where TWindow : Window;

        TWindow Create<TWindow>(Window? owner, object? dataContext, params object[] parameters)
            where TWindow : Window;

        #endregion
    }
}
