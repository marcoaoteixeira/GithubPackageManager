using System.IO;
using System.Text.Json;
using Nameless.Infrastructure;
using Wpf.Ui.Appearance;

namespace Nameless.Impl.Infrastructure {
    public sealed class AppConfigurationManager<TAppConfiguration> : IAppConfigurationManager<TAppConfiguration>, IDisposable
        where TAppConfiguration : AppConfiguration, new() {
        #region Private Read-Only Fields

        private readonly IApplicationContext _applicationContext;

        #endregion

        #region Private Fields

        private bool _disposed;

        #endregion

        #region Private Static Read-Only Properties

        private static TAppConfiguration DefaultAppConfiguration => new() {
            Theme = nameof(ApplicationTheme.Light),
            ConfirmBeforeExit = true
        };

        #endregion

        #region Public Constructors

        public AppConfigurationManager(IApplicationContext applicationContext) {
            ArgumentNullException.ThrowIfNull(applicationContext, nameof(applicationContext));

            _applicationContext = applicationContext;

            EnsureAppConfigurationFileExistence();
            Initialize();
        }

        #endregion

        #region Destructor

        ~AppConfigurationManager() {
            Dispose(disposing: false);
        }

        #endregion

        #region Private Methods

        private void Initialize() {
            Current = FetchFromFileSystem();
        }

        private void EnsureAppConfigurationFileExistence() {
            var filePath = Path.Combine(_applicationContext.AppDataDirectory, Root.Constants.APP_CONFIG_FILE_NAME);

            if (File.Exists(filePath)) { return; }

            var json = JsonSerializer.Serialize(new TAppConfiguration {
                Theme = nameof(ApplicationTheme.Light),
                ConfirmBeforeExit = true
            });

            File.WriteAllText(filePath, json);
        }

        private TAppConfiguration FetchFromFileSystem() {
            var file = _applicationContext.FileProvider.GetFileInfo(Root.Constants.APP_CONFIG_FILE_NAME);

            using var fileStream = file.CreateReadStream();
            using var streamReader = new StreamReader(fileStream);
            var content = streamReader.ReadToEnd();

            return JsonSerializer.Deserialize<TAppConfiguration>(content)
                ?? DefaultAppConfiguration;
        }

        private void StoreIntoFileSystem() {
            var file = _applicationContext.FileProvider.GetFileInfo(Root.Constants.APP_CONFIG_FILE_NAME);
            var json = JsonSerializer.Serialize(Current);

            if (string.IsNullOrWhiteSpace(file.PhysicalPath)) {
                return;
            }

            using var fileStream = new FileStream(file.PhysicalPath, FileMode.Create, FileAccess.Write);
            using var streamWriter = new StreamWriter(fileStream);
            streamWriter.Write(json);
        }

        private void Dispose(bool disposing) {
            if (_disposed) { return; }

            if (disposing) {
                StoreIntoFileSystem();
            }

            _disposed = true;
        }

        #endregion

        #region IAppConfigurationManager<TAppConfiguration> Members

        public TAppConfiguration Current { get; private set; } = DefaultAppConfiguration;

        #endregion

        #region IDisposable Members

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
