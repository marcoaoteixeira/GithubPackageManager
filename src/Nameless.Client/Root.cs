namespace Nameless.Client {
    /// <summary>
    /// This class was defined to be an entrypoint for this project assembly.
    /// 
    /// *** DO NOT IMPLEMENT ANYTHING HERE ***
    /// 
    /// But, it's OK to use this class for all constants or default values
    /// that you'll use throughout this project.
    /// </summary>
    public static class Root {
        #region Public Static Inner Classes

        public static class App {
            #region Public Constants

            public const string NAME = "WPF_Application";
            public const string LOG_FILE_NAME = "app.log";
            public const string DEFAULT_TEXT_EDITOR = "notepad.exe";

            #endregion
        }

        public static class UI {
            #region Public Static Properties

            public static TimeSpan SnackbarTimeout { get; } = TimeSpan.FromSeconds(5);

            #endregion

            public static class Navigation {
                #region Public Constants

                public const string CONFIGURATIONS = "Configurações";

                #endregion
            }
        }

        #endregion
    }
}
