using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace DariuszLabaj.MaterialTheme.Wpf.Converters
{
    internal class RectConverter : MarkupExtension, IValueConverter
    {
        public RectConverter() { }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FrameworkElement element)
            {
                double width = element.ActualWidth;
                double height = element.ActualHeight;
                return new Rect(0, 0, width, height);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
