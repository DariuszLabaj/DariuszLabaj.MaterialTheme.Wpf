using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace DariuszLabaj.MaterialTheme.Wpf.Converters
{
    internal class ArcEndPointConverter : MarkupExtension, IMultiValueConverter
    {
        public ArcEndPointConverter() { }
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 4 ||
                values[0] == null ||
                values[1] == null ||
                values[2] == null ||
                values[3] == null)
            {
                return new Point(0, 0);
            }
            try
            {
                double actualWidth = System.Convert.ToDouble(values[0]);
                double value = System.Convert.ToDouble(values[1]);
                double minimum = System.Convert.ToDouble(values[2]);
                double maximum = System.Convert.ToDouble(values[3]);
                //double scaleX = System.Convert.ToDouble(values[4]);
                double normalizedValue = (value - minimum) / (maximum - minimum);
                double angle = normalizedValue * 360;
                if (angle >= 360) angle = 359.999;
                double radius = (actualWidth / 2) - (actualWidth * 0.05);
                var pt = GetPointFromAngle(degrees: angle, size: actualWidth, radius: radius);
                return pt;
            }
            catch
            {
                return new Point(0, 0);
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
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
    }
}
