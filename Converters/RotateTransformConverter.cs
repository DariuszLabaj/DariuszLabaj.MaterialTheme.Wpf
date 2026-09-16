using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace DariuszLabaj.MaterialTheme.Wpf.Converters
{
    internal class RotateTransformConverter : MarkupExtension, IMultiValueConverter
    {
        public RotateTransformConverter() { }
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3 ||
                values[0] == null ||
                values[1] == null ||
                values[2] == null)
            {
                return 0.0;
            }
            try
            {
                double value = System.Convert.ToDouble(values[0]);
                double minimum = System.Convert.ToDouble(values[1]);
                double maximum = System.Convert.ToDouble(values[2]);
                double normalizedValue = (value - minimum) / (maximum - minimum);
                double angle = normalizedValue * 360;
                return angle;
            }
            catch
            {
                return 0.0;
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
    }
}
