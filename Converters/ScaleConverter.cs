using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace DariuszLabaj.MaterialTheme.Wpf.Converters
{
    internal class ScaleConverter : MarkupExtension, IValueConverter
    {
        private readonly double _scale;
        public ScaleConverter(double scale)
        {
            _scale = scale;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return DependencyProperty.UnsetValue;
            if (double.TryParse(value.ToString(), out double inputValue))
            {
                return inputValue * _scale;
            }
            return DependencyProperty.UnsetValue;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return DependencyProperty.UnsetValue;
            if (double.TryParse(value.ToString(), out double inputValuie))
            {
                return inputValuie / _scale;
            }
            return DependencyProperty.UnsetValue;
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
