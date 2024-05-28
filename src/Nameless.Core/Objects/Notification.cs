namespace Nameless.Objects {
    public abstract record Notification {
        #region Public Properties

        public Level Level { get; init; }
        public string Message { get; init; } = string.Empty;

        #endregion
    }

    public enum Level {
        None,
        Error,
        Information,
        Success,
        Warning
    }
}
