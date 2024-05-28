using System.Globalization;
using System.Windows.Data;

namespace Nameless.Impl.Converters {
    public sealed class EnumToStringConverter : IValueConverter {
        #region IValueConverter Members

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
            return value is Enum enumValue
                ? enumValue.ToString()
                : string.Empty;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
            if (value is null || !targetType.IsEnum) { return null; }

            return Enum.TryParse(targetType, value.ToString(), ignoreCase: false, out var result)
                ? result
                : null;
        }

        #endregion
    }
}
