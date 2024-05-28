namespace Nameless.Infrastructure {
    public interface IAppConfigurationManager<out TAppConfiguration>
        where TAppConfiguration : AppConfiguration, new() {
        #region Properties

        TAppConfiguration Current { get; }

        #endregion
    }
}
