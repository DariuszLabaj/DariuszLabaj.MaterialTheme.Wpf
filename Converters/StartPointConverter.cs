using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace DariuszLabaj.MaterialTheme.Wpf.Converters
{
    internal class StartPointConverter : MarkupExtension, IValueConverter
    {
        public StartPointConverter() { }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double actualWidth)
            {
                var radius = (actualWidth / 2) - (actualWidth * 0.05);
                var pt = GetPointFromAngle(degrees: 0.0, size: actualWidth, radius: radius);
                return pt;
            }
            return new Point(0, 0);
        }

        private Point GetPointFromAngle(double degrees, double size, double radius)
        {
            var radians = degrees * (Math.PI / 180);
            //int x = (int)Math.Round((size / 2) + (radius * (Math.Cos(radians))));
            //int y = (int)Math.Round((size / 2) + (radius * (Math.Sin(radians))));
            double x = (size / 2) + (radius * (Math.Cos(radians)));
            double y = (size / 2) + (radius * (Math.Sin(radians)));
            return new Point(x: x, y: y);
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
