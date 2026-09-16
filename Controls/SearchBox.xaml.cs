// Ignore Spelling: Labaj Dariusz

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class SearchBox : UserControl
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(SearchBox), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty SearchPromptProperty = DependencyProperty.Register("SearchPrompt", typeof(string), typeof(SearchBox), new PropertyMetadata("Search"));
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(Brush), typeof(SearchBox), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
        public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(SearchBox), new PropertyMetadata(new Thickness(0)));
        public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(SearchBox), new FrameworkPropertyMetadata(12.0, OnFontSizeChanged));
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        private static void OnFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (SearchBox)d;
            control.MinHeight = (double)e.NewValue * 3;
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string SearchPrompt
        {
            get { return (string)GetValue(SearchPromptProperty); }
            set { SetValue(SearchPromptProperty, value); }
        }
        public SearchBox()
        {
            InitializeComponent();
        }
    }
}
