using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace DariuszLabaj.MaterialTheme.Wpf.Helpers
{
    public static class TextBoxHelper
    {
        public static string GetLabel(DependencyObject obj) =>
            (string)obj.GetValue(LabelProperty);

        public static void SetLabel(DependencyObject obj, string value) =>
            obj.SetValue(LabelProperty, value);

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.RegisterAttached(
                "Label",
                typeof(string),
                typeof(TextBoxHelper),
                new FrameworkPropertyMetadata(
                    defaultValue: null,
                    propertyChangedCallback: OnLabelChanged)
                );

        private static void OnLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBoxControl)
            {
                if (!textBoxControl.IsLoaded)
                {
                    textBoxControl.Loaded -= TextBoxControl_Loaded;
                    textBoxControl.Loaded += TextBoxControl_Loaded;
                }

                textBoxControl.TextChanged -= TextBoxControl_TextChanged;
                textBoxControl.TextChanged += TextBoxControl_TextChanged;

                if (GetOrCreateAdorner(textBoxControl, out LabelAdorner? adorner) && adorner != null)
                {
                    UpdateAdornerVisibility(textBoxControl, adorner);
                }
            }
        }

        private static void TextBoxControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBoxControl)
            {
                textBoxControl.Loaded -= TextBoxControl_Loaded;
                if (GetOrCreateAdorner(textBoxControl, out LabelAdorner? adorner) && adorner != null)
                {
                    UpdateAdornerVisibility(textBoxControl, adorner);
                }
            }
        }

        private static void TextBoxControl_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBoxControl
                && GetOrCreateAdorner(textBoxControl, out LabelAdorner? adorner)
                && adorner != null)
            {
                UpdateAdornerVisibility(textBoxControl, adorner);
            }
        }

        private static void UpdateAdornerVisibility(TextBox textBoxControl, LabelAdorner adorner)
        {
            adorner.Visibility = string.IsNullOrEmpty(textBoxControl.Text)
                ? Visibility.Visible
                : Visibility.Hidden;
        }

        private static bool GetOrCreateAdorner(TextBox textBoxControl, out LabelAdorner? adorner)
        {
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(textBoxControl);

            if (layer == null)
            {
                adorner = null;
                return false;
            }

            adorner = layer.GetAdorners(textBoxControl)?.OfType<LabelAdorner>().FirstOrDefault();

            if (adorner == null)
            {
                adorner = new LabelAdorner(textBoxControl);
                layer.Add(adorner);
            }

            return true;
        }

        public class LabelAdorner : Adorner
        {
            public LabelAdorner(TextBox textBox) : base(textBox) { }

            protected override void OnRender(DrawingContext drawingContext)
            {
                TextBox textBoxControl = (TextBox)AdornedElement;

                string labelValue = TextBoxHelper.GetLabel(textBoxControl);

                if (string.IsNullOrEmpty(labelValue))
                    return;

                FormattedText text = new FormattedText(
                                            labelValue,
                                            System.Globalization.CultureInfo.CurrentCulture,
                                            textBoxControl.FlowDirection,
                                            new Typeface(textBoxControl.FontFamily,
                                                         textBoxControl.FontStyle,
                                                         FontWeights.Normal,
                                                         textBoxControl.FontStretch),
                                            textBoxControl.FontSize,
                                            SystemColors.InactiveCaptionBrush,
                                            VisualTreeHelper.GetDpi(textBoxControl).PixelsPerDip);

                text.MaxTextWidth = System.Math.Max(textBoxControl.ActualWidth - textBoxControl.Padding.Left - textBoxControl.Padding.Right, 10);
                text.MaxTextHeight = System.Math.Max(textBoxControl.ActualHeight, 10);

                Point renderingOffset = new Point(textBoxControl.Padding.Left, textBoxControl.Padding.Top);

                if (textBoxControl.Template.FindName("PART_ContentHost", textBoxControl) is FrameworkElement part)
                {
                    Point partPosition = part.TransformToAncestor(textBoxControl).Transform(new Point(0, 0));
                    renderingOffset.X += partPosition.X;
                    renderingOffset.Y += partPosition.Y;

                    text.MaxTextWidth = System.Math.Max(part.ActualWidth - renderingOffset.X, 10);
                    text.MaxTextHeight = System.Math.Max(part.ActualHeight, 10);
                }

                drawingContext.DrawText(text, renderingOffset);
            }
        }
    }
}
