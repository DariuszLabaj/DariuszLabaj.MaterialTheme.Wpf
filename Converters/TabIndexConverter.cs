using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace DariuszLabaj.MaterialTheme.Wpf.Converters
{
    internal class TabIndexConverter : MarkupExtension, IMultiValueConverter
    {
        public TabIndexConverter() { }
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is TabItem tabItem && values[1] is TabControl tabControl)
            {
                int index = tabControl.ItemContainerGenerator.IndexFromContainer(tabItem);
                int count = tabControl.Items.Count;
                double radius = tabItem.ActualHeight / 2;
                if (index == 0)
                {
                    return new CornerRadius(radius, 0, 0, radius);
                }
                else if (index == count - 1)
                {
                    return new CornerRadius(0, radius, radius, 0);
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
