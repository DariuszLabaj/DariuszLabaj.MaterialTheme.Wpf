using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace DariuszLabaj.MaterialTheme.Wpf.Converters
{
    public class DesaturateColorConverter : IValueConverter
    {
        public double SaturationFactor { get; set; } = 0.3;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color;
            if (value is SolidColorBrush brush)
            {
                color = brush.Color;
                var hsl = RgbToHsl(color);
                hsl.S = Math.Max(0, hsl.S * SaturationFactor);
                var desaturatedColor = HslToRgb(hsl);
                return new SolidColorBrush(desaturatedColor);
            }
            else if (value is Color)
            {
                color = (Color)value;
                var hsl = RgbToHsl(color);
                hsl.S = Math.Max(0, hsl.S * SaturationFactor);
                var desaturatedColor = HslToRgb(hsl);
                return desaturatedColor;
            }
            else
                return value; // Fallback
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static (double H, double S, double L) RgbToHsl(Color color)
        {
            double r = color.R / 255.0;
            double g = color.G / 255.0;
            double b = color.B / 255.0;
            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));
            double delta = max - min;
            double h = 0, s = 0, l = (max + min) / 2.0;
            if (delta > 0)
            {
                s = l > 0.5 ? delta / (2.0 - max - min) : delta / (max + min);

                if (max == r) h = (g - b) / delta;
                else if (max == g) h = 2.0 + (b - r) / delta;
                else h = 4.0 + (r - g) / delta;

                h *= 60.0;

                if (h < 0) h += 360;

            }
            return (h, s, l);
        }
        private static Color HslToRgb((double H, double S, double L) hsl)
        {
            byte r, g, b;

            if (hsl.S == 0)
            {
                r = g = b = (byte)(hsl.L * 255);
            }
            else
            {
                double q = hsl.L < 0.5 ? hsl.L * (1 + hsl.S) : hsl.L + hsl.S - hsl.L * hsl.S;
                double p = 2 * hsl.L - q;

                r = HueToRgb(p, q, hsl.H + 1.0 / 3);
                g = HueToRgb(p, q, hsl.H);
                b = HueToRgb(p, q, hsl.H - 1.0 / 3);
            }

            return Color.FromRgb(r, g, b);
        }
        private static byte HueToRgb(double p, double q, double t)
        {
            if (t < 0) t += 1;
            if (t > 1) t -= 1;
            if (t < 1.0 / 6) return (byte)((p + (q - p) * 6 * t) * 255);
            if (t < 1.5 / 6) return (byte)((q) * 255);
            if (t < 2.0 / 3) return (byte)((p + (q - p) * (2.0 / 3 - t) * 6) * 255);
            return (byte)(p * 255);
        }
    }
}
