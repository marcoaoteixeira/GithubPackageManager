namespace Nameless.Bootstrapping {
    public interface IBootstrap {
        #region Methods

        Task RunAsync(CancellationToken cancellationToken);

        #endregion
    }
}
