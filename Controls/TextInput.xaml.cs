// Ignore Spelling: Labaj Dariusz

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DariuszLabaj.MaterialTheme.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for TextInput.xaml
    /// </summary>
    public partial class TextInput : UserControl
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TextInput), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(TextInput), new PropertyMetadata(null));
        public static readonly DependencyProperty SupportingTextProperty = DependencyProperty.Register("SupportingText", typeof(string), typeof(TextInput), new PropertyMetadata(null));
        public static readonly DependencyProperty AccentColorProperty = DependencyProperty.Register("AccentColor", typeof(Brush), typeof(TextInput), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0xB0))));
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(Brush), typeof(TextInput), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0xF0, 0xF0, 0xF0))));
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(TextInput), new PropertyMetadata(TextWrapping.NoWrap));
        public static readonly DependencyProperty SupportingTextForegroundProperty = DependencyProperty.Register("SupportingTextForeground", typeof(Brush), typeof(TextInput), new PropertyMetadata(Brushes.DimGray));
        public static readonly DependencyProperty SupportingTextBackgroundProperty = DependencyProperty.Register("SupportingTextBackground", typeof(Brush), typeof(TextInput), new PropertyMetadata(Brushes.Transparent));
        public static readonly DependencyProperty AcceptsReturnProperty = DependencyProperty.Register("AcceptsReturn", typeof(bool), typeof(TextInput), new PropertyMetadata(false));
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(TextInput), new FrameworkPropertyMetadata(12.0, OnFontSizeChanged));
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        private static void OnFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (TextInput)d;
            control.MinHeight = (double)e.NewValue * 3;
        }
        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public Brush Background
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public string SupportingText
        {
            get { return (string)GetValue(SupportingTextProperty); }
            set { SetValue(SupportingTextProperty, value); }
        }

        public Brush AccentColor
        {
            get { return (Brush)GetValue(AccentColorProperty); }
            set { SetValue(AccentColorProperty, value); }
        }

        public Brush SupportingTextForeground
        {
            get { return (Brush)GetValue(SupportingTextForegroundProperty); }
            set { SetValue(SupportingTextForegroundProperty, value); }
        }

        public Brush SupportingTextBackground
        {
            get { return (Brush)GetValue(SupportingTextBackgroundProperty); }
            set { SetValue(SupportingTextBackgroundProperty, value); }
        }
        public bool AcceptsReturn
        {
            get { return (bool)GetValue(AcceptsReturnProperty); }
            set { SetValue(AcceptsReturnProperty, value); }
        }
        private readonly Storyboard focusedStoryboard;
        private readonly Storyboard lostFocusStoryboard;
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBoxField.Text))
            {
                focusedStoryboard.Begin(label);
            }
        }
        public TextInput()
        {
            InitializeComponent();
            // Initialize storyboards
            focusedStoryboard = new Storyboard();
            lostFocusStoryboard = new Storyboard();
            // Add animations to the focusedStoryboard
            AddAnimation(focusedStoryboard, -2, 0.75, 0.75);

            // Add animations to the lostFocusStoryboard
            AddAnimation(lostFocusStoryboard, 12, 1.0, 1.0);

            // Hook up event handlers
            GotFocus += TextBox_GotFocus;
            LostFocus += TextBox_LostFocus;
            TextBoxField.TextChanged += TextBox_TextChanged;
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Begin the focusedStoryboard when TextBox gets focus
            focusedStoryboard.Begin(label);
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Begin the lostFocusStoryboard when TextBox loses focus
            if (string.IsNullOrWhiteSpace(TextBoxField.Text)) lostFocusStoryboard.Begin(label);
        }
        private void AddAnimation(Storyboard storyboard, double top, double scaleX1, double scaleY1)
        {
            // Create and add DoubleAnimations to the storyboard
            var topAnimation = new DoubleAnimation(top, TimeSpan.FromSeconds(0.2));
            var scaleXAnimation = new DoubleAnimation(scaleX1, TimeSpan.FromSeconds(0.2));
            var scaleYAnimation = new DoubleAnimation(scaleY1, TimeSpan.FromSeconds(0.2));

            Storyboard.SetTarget(topAnimation, label);
            Storyboard.SetTarget(scaleXAnimation, label);
            Storyboard.SetTarget(scaleYAnimation, label);

            Storyboard.SetTargetProperty(topAnimation, new PropertyPath(Canvas.TopProperty));
            Storyboard.SetTargetProperty(scaleXAnimation, new PropertyPath("(RenderTransform).(ScaleTransform.ScaleX)"));
            Storyboard.SetTargetProperty(scaleYAnimation, new PropertyPath("(RenderTransform).(ScaleTransform.ScaleY)"));

            storyboard.Children.Add(topAnimation);
            storyboard.Children.Add(scaleXAnimation);
            storyboard.Children.Add(scaleYAnimation);
        }
    }
}
