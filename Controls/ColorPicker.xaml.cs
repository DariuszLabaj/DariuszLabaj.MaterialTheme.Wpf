using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Automation;
using System.Windows.Media;

namespace DariuszLabaj.MaterialTheme.Wpf.Controls
{
    /// <summary>
    /// A Material 3 color picker with a transactional editing popup.
    /// SelectedColor remains a #RRGGBB string for backwards compatibility.
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        private const string DefaultColor = "#6750A4";
        private static readonly string[] Palette =
        [
            "#B3261E", "#BA1A1A", "#E65100", "#F9A825",
            "#2E7D32", "#006A60", "#0061A4", "#6750A4",
            "#9A25AE", "#C2185B", "#5D4037", "#546E7A",
            "#000000", "#49454F", "#79747E", "#FFFFFF"
        ];
        private bool _isSynchronizing;
        private bool _isDragging;
        private bool _isHexValid = true;
        private double _hue = 267;
        private double _saturation = 0.51;
        private double _value = 0.64;
        public ColorPicker()
        {
            InitializeComponent();
            BuildPalette();
            UpdateSelectedDisplay(SelectedColor);
            SetWorkingColor(SelectedColor, updateHex: true, raisePreviewEvent: false);
        }
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(nameof(SelectedColor), typeof(string), typeof(ColorPicker), new FrameworkPropertyMetadata(DefaultColor, flags:FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, propertyChangedCallback:OnSelectedColorPropertyChanged), IsValidHexObject);
        /// <summary>
        /// Occurs when the committed SelectedColor value changes.
        /// The event bubbles through the WPF element tree.
        /// </summary>
        public static readonly RoutedEvent SelectedColorChangedEvent = EventManager.RegisterRoutedEvent(nameof(SelectedColorChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<string>), typeof(ColorPicker));
        public event RoutedPropertyChangedEventHandler<string> SelectedColorChanged
        {
            add => AddHandler(SelectedColorChangedEvent, value);
            remove => RemoveHandler(SelectedColorChangedEvent, value);
        }
        /// <summary>
        /// Gets or sets a command executed after SelectedColor changes.
        /// The command parameter is the new color in #RRGGBB format.
        /// </summary>
        public static readonly DependencyProperty SelectedColorChangedCommandProperty = DependencyProperty.Register(nameof(SelectedColorChangedCommand), typeof(ICommand), typeof(ColorPicker), new PropertyMetadata(null));
        public ICommand? SelectedColorChangedCommand
        {
            get => (ICommand?)GetValue(SelectedColorChangedCommandProperty);
            set => SetValue(SelectedColorChangedCommandProperty, value);
        }
        /// <summary>
        /// Gets or sets the selected color in #RRGGBB format.
        /// </summary>
        public string SelectedColor
        {
            get => (string)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }
        private static readonly DependencyPropertyKey PreviewColorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(PreviewColor), typeof(string), typeof(ColorPicker), new PropertyMetadata(DefaultColor));
        public static readonly DependencyProperty PreviewColorProperty = PreviewColorPropertyKey.DependencyProperty;
        public string PreviewColor => (string)GetValue(PreviewColorProperty);
        public static readonly RoutedEvent PreviewColorChangedEvent = EventManager.RegisterRoutedEvent(nameof(PreviewColorChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<string>), typeof(ColorPicker));
        public event RoutedPropertyChangedEventHandler<string> PreviewColorChanged
        {
            add => AddHandler(PreviewColorChangedEvent, value);
            remove => RemoveHandler(PreviewColorChangedEvent, value);
        }
        public static readonly DependencyProperty PreviewColorChangedCommandProperty = DependencyProperty.Register(nameof(PreviewColorChangedCommand), typeof(ICommand), typeof(ColorPicker), new PropertyMetadata(null));
        public ICommand? PreviewColorChangedCommand
        {
            get => (ICommand?)GetValue(PreviewColorChangedCommandProperty);
            set => SetValue(PreviewColorChangedCommandProperty, value);
        }
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(nameof(Label), typeof(string), typeof(ColorPicker), new PropertyMetadata("Color"));
        /// <summary>
        /// Gets or sets the label displayed by the picker.
        /// </summary>
        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(ColorPicker), new FrameworkPropertyMetadata(false, propertyChangedCallback: IsOpenChanged));
        /// <summary>
        /// Gets or sets whether the color editor popup is open.
        /// </summary>
        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }
        private static bool IsValidHexObject(object value) => value is string text && IsValidHex(text);
        private static bool IsValidHex(string text)
        {
            if (text == null || text.Length != 7 || text[0] != '#')
                return false;
            for (int i = 1; i < 7; i++)
                if (!Uri.IsHexDigit(text[i]))
                    return false;
            return true;
        }
        private static void OnSelectedColorPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var picker = (ColorPicker)dependencyObject;
            var oldColor = (string)e.OldValue;
            var selectedColor = (string)e.NewValue;
            picker.UpdateSelectedDisplay(selectedColor);
            if (!picker.IsOpen && picker.HexTextBox != null)
                picker.SetWorkingColor(selectedColor, updateHex: true, raisePreviewEvent: true);
            picker.RaiseColorChanged(SelectedColorChangedEvent, oldColor, selectedColor, picker.SelectedColorChangedCommand);
        }
        private static void IsOpenChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var picker = (ColorPicker)dependencyObject;
            if (picker.PickerPopup == null)
                return;
            if ((bool)e.NewValue)
            {
                picker.SetWorkingColor(picker.SelectedColor, updateHex: true, raisePreviewEvent: true);
                picker.PickerPopup.IsOpen = true;
            }
            else
            {
                if (picker.PickerPopup.IsOpen)
                    picker.PickerPopup.IsOpen = false;
                picker.SetWorkingColor(picker.SelectedColor, updateHex: true, raisePreviewEvent: true);
            }
        }
        private void BuildPalette()
        {
            foreach (string hex in Palette)
            {
                var color = ParseColor(hex);
                var button = new Button
                {
                    Style = (Style) FindResource("ColorPickerSwatchButtonStyle"),
                    Background = new SolidColorBrush(color),
                    Tag = hex,
                    ToolTip = hex,
                    Content = null
                };

                AutomationProperties.SetName(button, $"Wybierz kolor {hex}");
                button.Click += PaletteButton_Click;
                PaletteGrid.Children.Add(button);
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
            if (!_isHexValid || !IsValidHex(HexTextBox.Text))
            {
                HexErrorText.Visibility = Visibility.Visible;
                HexTextBox.Focus();
                HexTextBox.SelectAll();
                return;
            }
            string colorToApply = PreviewColor;
            SetCurrentValue(SelectedColorProperty, colorToApply);
            IsOpen = false;
            OpenButton.Focus();
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
            OpenButton.Focus();
        }


        private void PickerPopup_Closed(object? sender, EventArgs e)
        {
            if (IsOpen)
                SetCurrentValue(IsOpenProperty, false);
        }
        private void HexTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isSynchronizing || HexTextBox == null)
                return;
            _isHexValid = IsValidHex(HexTextBox.Text);
            HexErrorText?.Visibility = _isHexValid ? Visibility.Collapsed : Visibility.Visible;
            if (_isHexValid)
                SetWorkingColor(HexTextBox.Text, false);
        }
        private void HueSlide_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_isSynchronizing || SvSurface == null) 
                return;
            _hue = e.NewValue;
            UpdateSvBackground();
            UpdateWorkingFromHsv();
        }

        private void SvCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDragging = true;
            SvCanvas.CaptureMouse();
            SvCanvas.Focus();
            UpdateSvFromPointer(e.GetPosition(SvCanvas));
            e.Handled = true;
        }

        private void SvCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging && e.LeftButton == MouseButtonState.Pressed)
                UpdateSvFromPointer(e.GetPosition(SvCanvas));
        }

        private void SvCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!_isDragging)
                return;
            UpdateSvFromPointer(e.GetPosition(SvCanvas));
            _isDragging = false;
            SvCanvas.ReleaseMouseCapture();
            e.Handled = true;
        }
        private void SvCanvas_LostMouseCapture(object sender, MouseEventArgs e)
        {
            _isDragging = false;
        }
        private void SvCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            double step = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift ? 0.05 : 0.01;
            bool handled = true;

            switch (e.Key)
            {
                case Key.Left:
                    _saturation -= step;
                    break;
                case Key.Right:
                    _saturation += step;
                    break;
                case Key.Up:
                    _value += step;
                    break;
                case Key.Down:
                    _value -= step;
                    break;
                default:
                    handled = false;
                    break;
            }

            if (!handled)
                return;

            _saturation = Clamp(_saturation, 0, 1);
            _value = Clamp(_value, 0, 1);
            UpdateWorkingFromHsv();
            e.Handled = true;
        }
        private void SvCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            PositionMarker();
        }
        private void UpdateSvFromPointer(Point point)
        {
            _saturation = Clamp(point.X / Math.Max(1, SvCanvas.ActualWidth), 0, 1);
            _value = 1 - Clamp(point.Y / Math.Max(1, SvCanvas.ActualHeight), 0, 1);
            UpdateWorkingFromHsv();
        }
        private void SetWorkingColor(string hex, bool updateHex, bool raisePreviewEvent = true)
        {
            if (!IsValidHex(hex))
                return;
            string newColor = hex.ToUpperInvariant();
            ColorToHsv(ParseColor(newColor), out _hue, out _saturation, out _value);
            _isSynchronizing = true;
            try
            {
                HueSlide?.Value = _hue;
                if (updateHex && HexTextBox != null)
                    HexTextBox.Text = newColor;
                HexErrorText?.Visibility = Visibility.Collapsed;
                _isHexValid = true;
                UpdateSvBackground();
                PositionMarker();
            }
            finally
            {
                _isSynchronizing = false;
            }
            PublishPreviewColorChanged(newColor, raisePreviewEvent);
        }
        private void UpdateWorkingFromHsv()
        {
            string newColor = ColorToHex(HsvToColor(_hue, _saturation, _value));
            _isSynchronizing = true;
            try
            {
                HexTextBox?.Text = newColor;
                HexErrorText?.Visibility = Visibility.Collapsed;
                _isHexValid = true;
                PositionMarker();
            }
            finally
            {
                _isSynchronizing = false;
            }
            PublishPreviewColorChanged(newColor, raisePreviewEvent: true);
        }
        private void PublishPreviewColorChanged(string newColor, bool raisePreviewEvent)
        {
            string previousColor = PreviewColor;
            bool hasChanged = !string.Equals(previousColor, newColor, StringComparison.OrdinalIgnoreCase);
            if (hasChanged)
                SetValue(PreviewColorPropertyKey, newColor);
            UpdatePreview();
            UpdatePaletteSelection();
            if (hasChanged && raisePreviewEvent)
                RaiseColorChanged(PreviewColorChangedEvent, previousColor, newColor, PreviewColorChangedCommand);
        }
        private void RaiseColorChanged(RoutedEvent routedEvent, string oldColor, string newColor, ICommand? command)
        {
            if (string.Equals(oldColor, newColor, StringComparison.OrdinalIgnoreCase))
                return;
            var eventArgs = new RoutedPropertyChangedEventArgs<string>(oldColor, newColor, routedEvent);
            RaiseEvent(eventArgs);
            if (command is not null && command.CanExecute(newColor))
                command.Execute(newColor);
        }
        private void UpdateSvBackground()
        {
            SvHueLayer?.Background = new SolidColorBrush(HsvToColor(_hue, 1, 1));
        }
        private void PositionMarker()
        {
            if (SvMarker is null || SvCanvas is null)
                return;

            double markerRadius = SvMarker.Width / 2;
            double left = _saturation * SvCanvas.ActualWidth - markerRadius;
            double top = (1 - _value) * SvCanvas.ActualHeight - markerRadius;

            Canvas.SetLeft(SvMarker, Clamp(left, 0, Math.Max(0, SvCanvas.ActualWidth - SvMarker.Width)));
            Canvas.SetTop(SvMarker, Clamp(top, 0, Math.Max(0, SvCanvas.ActualHeight - SvMarker.Height)));
        }
        private void UpdatePreview()
        {
            PreviewSwatch?.Background = new SolidColorBrush(ParseColor(PreviewColor));
        }
        private void UpdatePaletteSelection()
        {
            if (PaletteGrid == null)
                return;

            foreach (UIElement element in PaletteGrid.Children)
            {
                if (element is not Button button || button.Tag is not string hex)
                    continue;

                bool isSelected = string.Equals(hex, PreviewColor, StringComparison.OrdinalIgnoreCase);
                button.Content = isSelected? "✓" : null;
                button.Foreground = GetReadableForeground(ParseColor(hex));
                button.BorderThickness = isSelected? new Thickness(2) : new Thickness(1);
                button.SetResourceReference(
                    Control.BorderBrushProperty,
                    isSelected? "primaryBrush" : "outlineVariantBrush");
            }
        }
        private void UpdateSelectedDisplay(string hex)
        {
            if (!IsValidHex(hex))
                return;
            SelectedHexText?.Text = hex;
            SelectedSwatch?.Background = new SolidColorBrush(ParseColor(hex));
        }
        private static Brush GetReadableForeground(Color backgroundColor)
        {
            double luminance = (0.2126 * backgroundColor.R + 0.7152 * backgroundColor.G + 0.0722 * backgroundColor.B) / 255.0;
            return luminance > 0.58 ? Brushes.Black : Brushes.White;
        }
        private static Color ParseColor(string hex)
        {
            byte red = byte.Parse(hex.Substring(1, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            byte green = byte.Parse(hex.Substring(3, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            byte blue = byte.Parse(hex.Substring(5, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            return Color.FromRgb(red, green, blue);
        }
        private static string ColorToHex(Color color) => string.Format(CultureInfo.InvariantCulture, "#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
        private static Color HsvToColor(double hue, double saturation, double value)
        {
            double chroma = value * saturation;
            double intermediate = chroma * (1 - Math.Abs((hue / 60.0) % 2 - 1));
            double offset = value - chroma;
            double red = 0;
            double green = 0;
            double blue = 0;

            if (hue < 60)
            {
                red = chroma;
                green = intermediate;
            }
            else if (hue < 120)
            {
                red = intermediate;
                green = chroma;
            }
            else if (hue < 180)
            {
                green = chroma;
                blue = intermediate;
            }
            else if (hue < 240)
            {
                green = intermediate;
                blue = chroma;
            }
            else if (hue < 300)
            {
                red = intermediate;
                blue = chroma;
            }
            else
            {
                red = chroma;
                blue = intermediate;
            }

            return Color.FromRgb(
                ToByte((red + offset) * 255),
                ToByte((green + offset) * 255),
                ToByte((blue + offset) * 255));
        }
        private static void ColorToHsv(Color color, out double hue, out double saturation, out double value)
        {
            double red = color.R / 255.0;
            double green = color.G / 255.0;
            double blue = color.B / 255.0;
            double maximum = Math.Max(red, Math.Max(green, blue));
            double minimum = Math.Min(red, Math.Min(green, blue));
            double delta = maximum - minimum;

            if (delta == 0)
                hue = 0;
            else if (maximum == red)
                hue = 60 * (((green - blue) / delta) % 6);
            else if (maximum == green)
                hue = 60 * (((blue - red) / delta) + 2);
            else
                hue = 60 * (((red - green) / delta) + 4);

            if (hue < 0)
                hue += 360;

            saturation = maximum == 0 ? 0 : delta / maximum;
            value = maximum;
        }
        private static byte ToByte(double value) => (byte)Math.Round(Clamp(value, 0, 255));
        private static double Clamp(double value, double min, double max) => Math.Max(min, Math.Min(max, value));
    }
}
