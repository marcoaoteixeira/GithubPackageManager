using Nameless.MessageBox;
using MS_MessageBox = System.Windows.MessageBox;

namespace Nameless.Impl.MessageBox {
    public sealed class MessageBoxService : IMessageBoxService {
        #region MessageBoxService Members

        public MessageBoxResult Show(string title, string message, MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Information, object? owner = null) {
            var result = owner is System.Windows.Window window
                ? MS_MessageBox.Show(window, message, title, buttons.Convert(), icon.Convert())
                : MS_MessageBox.Show(message, title, buttons.Convert(), icon.Convert());

            return result.Convert();
        }

        #endregion
    }
}
