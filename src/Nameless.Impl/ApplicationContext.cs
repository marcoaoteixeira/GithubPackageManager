using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;

namespace Nameless.Impl {
    public sealed class ApplicationContext : IApplicationContext {
        #region Public Constructors

        public ApplicationContext(string appName, Version appVersion, bool useSpecialFolder = true) {
            ArgumentException.ThrowIfNullOrWhiteSpace(appName, nameof(appName));
            ArgumentNullException.ThrowIfNull(appVersion, nameof(appVersion));

            AppName = appName;
            AppVersion = appVersion;
            AppDataDirectory = useSpecialFolder
                ? Path.Combine(Environment.GetFolderPath(folder: Environment.SpecialFolder.LocalApplicationData,
                                                         option: Environment.SpecialFolderOption.Create),
                               AppName)
                : typeof(ApplicationContext).Assembly.GetDirectoryPath(Root.Constants.APP_DATA_FOLDER_NAME);

            IOHelper.EnsureDirectory(AppDataDirectory);

            FileProvider = new PhysicalFileProvider(AppDataDirectory, ExclusionFilters.None);
        }

        #endregion

        #region IApplicationContext Members

        public string AppName { get; }
        public Version AppVersion { get; }
        public string AppDataDirectory { get; }
        public IFileProvider FileProvider { get; }

        #endregion
    }
}
