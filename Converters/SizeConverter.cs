using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;


namespace DariuszLabaj.MaterialTheme.Wpf.Converters
{
    internal class SizeConverter : MarkupExtension, IValueConverter
    {
        public SizeConverter() { }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double actualWidth)
            {
                var strokeThickness = actualWidth * 0.1;
                var pathWidth = actualWidth - strokeThickness;
                return new Size(pathWidth / 2, pathWidth / 2);
            }
            return new Size(0, 0);
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
