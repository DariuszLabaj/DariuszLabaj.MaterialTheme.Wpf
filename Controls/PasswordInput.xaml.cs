// Ignore Spelling: Labaj Dariusz

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security;
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
    /// Interaction logic for PasswordInput.xaml
    /// </summary>
    public partial class PasswordInput : UserControl
    {
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(SecureString), typeof(PasswordInput), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(PasswordInput), new PropertyMetadata(null));
        public static readonly DependencyProperty SupportingTextProperty = DependencyProperty.Register("SupportingText", typeof(string), typeof(PasswordInput), new PropertyMetadata(null));
        public static readonly DependencyProperty AccentColorProperty = DependencyProperty.Register("AccentColor", typeof(Brush), typeof(PasswordInput), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0xB0))));
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(Brush), typeof(PasswordInput), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0xF0, 0xF0, 0xF0))));
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        public static readonly DependencyProperty SupportingTextForegroundProperty = DependencyProperty.Register("SupportingTextForeground", typeof(Brush), typeof(PasswordInput), new PropertyMetadata(Brushes.DimGray));
        public static readonly DependencyProperty SupportingTextBackgroundProperty = DependencyProperty.Register("SupportingTextBackground", typeof(Brush), typeof(PasswordInput), new PropertyMetadata(Brushes.Transparent));
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(PasswordInput), new FrameworkPropertyMetadata(12.0, OnFontSizeChanged));
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        private static void OnFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (PasswordInput)d;
            control.MinHeight = (double)e.NewValue * 3;
        }
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public Brush Background
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }
        public SecureString Password
        {
            get { return (SecureString)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
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


        private readonly Storyboard focusedStoryboard;
        private readonly Storyboard lostFocusStoryboard;
        public PasswordInput()
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
            TextBoxField.PasswordChanged += TextBox_PasswordChanged;
        }
        private void TextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = ConvertToSecureString(TextBoxField.Password);
            if (!string.IsNullOrWhiteSpace(TextBoxField.Password))
            {
                focusedStoryboard.Begin(label);
            }
        }

        private SecureString ConvertToSecureString(string value)
        {
            var secure = new SecureString();
            foreach (char c in value)
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Begin the focusedStoryboard when TextBox gets focus
            focusedStoryboard.Begin(label);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Begin the lostFocusStoryboard when TextBox loses focus
            if (string.IsNullOrWhiteSpace(TextBoxField.Password)) lostFocusStoryboard.Begin(label);
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

        public void ClearPassword()
        {
            TextBoxField.Password = "";
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextBoxField.Password = "";
            TextBox_LostFocus(sender, e);
        }
        new public void Focus()
        {
            TextBoxField.Focus();
        }
        public event KeyEventHandler PasswordBoxKeyDown
        {
            add { TextBoxField.KeyDown += value; }
            remove { TextBoxField.KeyDown -= value; }
        }
    }
}
