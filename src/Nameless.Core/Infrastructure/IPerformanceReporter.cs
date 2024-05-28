namespace Nameless.Infrastructure {
    public interface IPerformanceReporter {
        #region Methods

        IDisposable ReportExecutionTime(string? tag);

        #endregion
    }
}
