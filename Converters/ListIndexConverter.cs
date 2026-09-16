using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace DariuszLabaj.MaterialTheme.Wpf.Converters
{
    internal class ListIndexConverter : MarkupExtension, IMultiValueConverter
    {
        public ListIndexConverter() { }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is ListBoxItem tabItem && values[1] is ListBox tabControl)
            {
                int index = tabControl.ItemContainerGenerator.IndexFromContainer(tabItem);
                int count = tabControl.Items.Count;
                double radius = 8;
                if (index == 0)
                {
                    return new CornerRadius(radius, radius, 0, 0);
                }
                else if (index == count - 1)
                {
                    return new CornerRadius(0, 0, radius, radius);
                }
                else
                {
                    return new CornerRadius(0);
                }
            }
            return new CornerRadius(0);
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
