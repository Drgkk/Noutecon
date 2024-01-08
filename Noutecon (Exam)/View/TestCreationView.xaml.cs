using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Noutecon__Exam_.View
{
    /// <summary>
    /// Interaction logic for TestCreationView.xaml
    /// </summary>
    public partial class TestCreationView : UserControl
    {
        public TestCreationView()
        {
            InitializeComponent();
        }

        private void RichTextBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox richTextBox = sender as TextBox;
            if(richTextBox.Visibility == Visibility.Visible)
            {
                richTextBox.Focus();
            }
        }

        private void RichTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox richTextBox = sender as TextBox;
            richTextBox.Visibility = Visibility.Collapsed;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static readonly Regex _regex = new Regex("[^0-9]+");
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
    }
}
