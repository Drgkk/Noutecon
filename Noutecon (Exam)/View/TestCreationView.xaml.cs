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
            RichTextBox richTextBox = sender as RichTextBox;
            if(richTextBox.Visibility == Visibility.Visible)
            {
                richTextBox.Focus();
            }
        }

        private void RichTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            RichTextBox richTextBox = sender as RichTextBox;
            richTextBox.Visibility = Visibility.Collapsed;
        }
    }
}
