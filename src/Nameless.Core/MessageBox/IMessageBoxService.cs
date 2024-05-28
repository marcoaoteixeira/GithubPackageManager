namespace Nameless.MessageBox {
    public interface IMessageBoxService {
        #region Methods

        MessageBoxResult Show(string title, string message, MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxIcon icon = MessageBoxIcon.Information, object? owner = null);

        #endregion
    }
}
