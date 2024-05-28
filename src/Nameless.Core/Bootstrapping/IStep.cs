namespace Nameless.Bootstrapping {
    public interface IStep {
        #region Properties

        string Name { get; }
        int Order { get; }
        bool Skip { get; }
        bool ThrowOnFailure { get; }

        #endregion

        #region Methods

        Task ExecuteAsync(CancellationToken cancellationToken);

        #endregion
    }
}
