namespace Nameless {
    public static class VersionExtension {
        #region Public Static Methods

        public static string ToSemVer(this Version self)
            => $"v{self.Major}.{self.Minor}.{self.Build}";

        #endregion
    }
}
