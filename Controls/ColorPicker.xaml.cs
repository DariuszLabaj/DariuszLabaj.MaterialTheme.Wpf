using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DariuszLabaj.MaterialTheme.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        private static readonly string[] Palette =
        {
            "#B3261E", "#BA1A1A", "#E65100", "#F9A825", "#2E7D32", "#006A60", "#0061A4", "#6750A4",
            "#9A25AE", "#C2185B", "#5D4037", "#546E7A", "#000000", "#49454F", "#79747E", "#FFFFFF"
        };
        private bool _updating;
        private bool _dragging;
        private bool _hexValid = true;
        private string _workingColor = "#6750A4";
        private string _openingColor = "#6750A4";
        private double _hue = 267, _saturation = 0.51, _value = 0.64;
        public ColorPicker()
        {
            InitializeComponent();
            foreach (string hex in Palette)
            {
                var button = new Button
                {
                    Style = (Style)Resources["SwatchStyle"],
                    Background = new SolidColorBrush(ParseColor(hex)),
                    Tag = hex,
                    ToolTip = hex
                };
                button.Click += PaletteButton_Click;
            }
            SetWorkingColor(SelectedColor, true);
        }
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(nameof(SelectedColor), typeof(string), typeof(ColorPicker), new FrameworkPropertyMetadata("#6750A4", flags:FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, propertyChangedCallback:SelectedColorChanged), IsValidHexObject);


        public string SelectedColor
        {
            get => (string)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(nameof(Label), typeof(string), typeof(ColorPicker), new PropertyMetadata("Color"));
        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(ColorPicker), new FrameworkPropertyMetadata(false, propertyChangedCallback: IsOpenChanged));

        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }
        private static bool IsValidHexObject(object value) => value is string text && IsValidHex(text);
        private static bool IsValidHex(string text)
        {
            if (text == null || text.Length != 7 || text[0] != '#') return false;
            for (int i = 1; i < 7; i++)
                if (!Uri.IsHexDigit(text[i])) return false;
            return true;
        }
        private static void SelectedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var picker = (ColorPicker)d;
            picker.UpdateSelectedDisplay((string)e.NewValue);
            if (picker.PickerPopup != null && !picker.PickerPopup.IsOpen)
                picker.SetWorkingColor((string)e.NewValue, true);

        }
        private static void IsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var picker = (ColorPicker)d;
            if ((bool)e.NewValue)
            {
                picker._openingColor = picker.SelectedColor;
                picker.SetWorkingColor(picker.SelectedColor, true);
                picker.PickerPopup.IsOpen = true;
            }
            else if (picker.PickerPopup.IsOpen)
            {
                picker.PickerPopup.IsOpen = false;
            }
        }
        private void OpenButton_Click(object sender, RoutedEventArgs e) => IsOpen = true;
        private void PaletteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string hex)
                SetWorkingColor(hex, true);
        }
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_hexValid || !IsValidHex(HexTextBox.Text))
            {
                HexErrorText.Visibility = Visibility.Visible;
                return;
            }
            SetCurrentValue(SelectedColorProperty, HexTextBox.Text.ToUpperInvariant());
            IsOpen = false;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            SetWorkingColor(_openingColor, true);
            IsOpen = false;
        }


        private void PickerPopup_Closed(object sender, EventArgs e)
        {
            if (IsOpen) SetCurrentValue(IsOpenProperty, false);
        }
        private void HexTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_updating || HexTextBox == null) return;
            _hexValid = IsValidHex(HexTextBox.Text);
            HexErrorText.Visibility = _hexValid ? Visibility.Collapsed : Visibility.Visible;
            if (_hexValid) SetWorkingColor(HexErrorText.Text, false);
        }
        private void HueSlide_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_updating || SvCanvas == null) return;
            _hue = HueSlide.Value;
            UpdateSvBackground();
            UpdateWorkingFromHsv();
        }

        private void SvCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _dragging = true;
            SvCanvas.CaptureMouse();
            UpdateSvFromPointer(e.GetPosition(SvCanvas));
        }

        private void SvCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging && e.LeftButton == MouseButtonState.Pressed)
                UpdateSvFromPointer(e.GetPosition(SvCanvas));
        }

        private void SvCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _dragging = false;
            SvCanvas.ReleaseMouseCapture();
            UpdateSvFromPointer(e.GetPosition(SvCanvas));
        }
        private void UpdateSvFromPointer(Point point)
        {
            _saturation = Clamp(point.X / Math.Max(1, SvCanvas.ActualWidth), 0, 1);
            _value = 1 - Clamp(point.Y / Math.Max(1, SvCanvas.ActualHeight), 0, 1);
            PositionMarker();
            UpdateWorkingFromHsv();
        }
        private void SetWorkingColor(string hex, bool updateHex)
        {
            if (!IsValidHex(hex)) return;
            _workingColor = hex.ToUpperInvariant();
            ColorToHsv(ParseColor(_workingColor), out _hue, out _saturation, out _value);
            _updating = true;
            try
            {
                if (HueSlide != null) HueSlide.Value = _hue;
                if (updateHex && HexTextBox != null) HexTextBox.Text = _workingColor;
                if (HexErrorText != null) HexErrorText.Visibility = Visibility.Collapsed;
                _hexValid = true;
                if (SvCanvas != null) UpdateSvBackground();
                PositionMarker();
                UpdatePreview();
            }
            finally
            {
                _updating = false;
            }
        }
        private void UpdateWorkingFromHsv()
        {
            _workingColor = ColorToHex(HsvToColor(_hue, _saturation, _value));
            _updating = true;
            try
            {
                HexTextBox.Text = _workingColor;
                HexErrorText.Visibility = Visibility.Collapsed;
                _hexValid = true;
                UpdatePreview();
                PositionMarker();
            }
            finally
            {
                _updating = false;
            }
        }
        private void UpdateSvBackground()
        {
            SvCanvas.Background = new SolidColorBrush(HsvToColor(_hue, 1, 1));
        }
        private void PositionMarker()
        {
            if (SvMarker is null || SvCanvas is null) return;
            Canvas.SetLeft(SvMarker, Clamp(_saturation * SvCanvas.ActualWidth - SvMarker.Width / 2, 0, Math.Max(0, SvCanvas.ActualWidth - SvMarker.Width)));
            Canvas.SetTop(SvMarker, Clamp((1 - _value) * SvCanvas.ActualHeight - SvMarker.Height / 2, 0, Math.Max(0, SvCanvas.ActualHeight - SvMarker.Height)));
        }
        private void UpdatePreview()
        {
            if (PreviewSwatch != null)
                PreviewSwatch.Background = new SolidColorBrush(ParseColor(_workingColor));
            if (SvCanvas != null)
                SvCanvas.Background = new SolidColorBrush(HsvToColor(_hue, 1, 1));
        }
        private void UpdateSelectedDisplay(string hex)
        {
            if (!IsValidHex(hex)) return;
            if (SelectedHexText != null) SelectedHexText.Text = hex;
            if (SelectedSwatch != null) SelectedSwatch.Background = new SolidColorBrush(ParseColor(hex));
        }
        private static Color ParseColor(string hex) => (Color)ColorConverter.ConvertFromString(hex);
        private static string ColorToHex(Color color) => string.Format(CultureInfo.InvariantCulture, "#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
        private static Color HsvToColor(double hue, double saturation, double value)
        {
            hue = ((hue % 360) + 360) % 360;
            saturation = Clamp(saturation, 0, 1);
            value = Clamp(value, 0, 1);
            double c = value * saturation;
            double x = c * (1 - Math.Abs((hue / 60.0) % 2 - 1));
            double m = value - c, r = 0, g = 0, b = 0;
            if (hue < 60) { r = c; g = x; }
            else if (hue < 120) { r = x; g = c; }
            else if (hue < 180) { g = c; b = x; }
            else if (hue < 240) { g = x; b = c; }
            else if (hue < 300) { r = x; b = c; }
            else { r = c; b = x; }
            return Color.FromRgb(ToByte((r + m) * 255), ToByte((g + m) * 255), ToByte((b + m) * 255));
        }
        private static void ColorToHsv(Color color, out double hue, out double saturation, out double value)
        {
            double r = color.R / 255.0, g = color.G / 255.0, b = color.B / 255.0;
            double max = Math.Max(r, Math.Max(g, b)), min = Math.Min(r, Math.Min(g, b));
            double delta = max - min;
            if (delta == 0) hue = 0;
            else if (max == r) hue = 60 * (((g - b) / delta) % 6);
            else if (max == g) hue = 60 * (((b - r) / delta) + 2);
            else hue = 60 * (((r - g) / delta) + 4);
            if (hue < 0) hue += 360;
            saturation = max == 0 ? 0 : delta / max;
            value = max;
        }
        private static byte ToByte(double value) => (byte)Math.Round(Clamp(value, 0, 255));
        private static double Clamp(double value, double min, double max) => Math.Max(min, Math.Min(max, value));
    }
}
