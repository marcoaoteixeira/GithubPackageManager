using Microsoft.Extensions.FileProviders;

namespace Nameless {
    public interface IApplicationContext {
        #region Properties

        string AppName { get; }
        Version AppVersion { get; }
        string AppDataDirectory { get; }
        IFileProvider FileProvider { get; }

        #endregion
    }
}
