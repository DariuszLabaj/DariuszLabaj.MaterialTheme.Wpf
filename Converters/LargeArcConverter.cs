using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace DariuszLabaj.MaterialTheme.Wpf.Converters
{
    internal class LargeArcConverter : MarkupExtension, IMultiValueConverter
    {
        public LargeArcConverter() { }
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3 ||
                values[0] == null ||
                values[1] == null ||
                values[2] == null)
            {
                return false;
            }
            try
            {
                double value = System.Convert.ToDouble(values[0]);
                double minimum = System.Convert.ToDouble(values[1]);
                double maximum = System.Convert.ToDouble(values[2]);
                //double scaleX = System.Convert.ToDouble(values[3]);
                double normalizaedValue = (value - minimum) / (maximum - minimum);
                var val = normalizaedValue > 0.5;
                return val;
            }
            catch
            {
                return false;
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
