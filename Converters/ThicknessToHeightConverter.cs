using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DariuszLabaj.MaterialTheme.Wpf.Converters
{
    internal class ThicknessToHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Thickness thickness)
            {
                return thickness.Bottom;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
