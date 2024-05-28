using Nameless.Objects;

namespace Nameless.Client.Objects {
    public sealed record SnackbarNotification : Notification {
        #region Public Properties

        public string? Title { get; init; }

        #endregion
    }
}
