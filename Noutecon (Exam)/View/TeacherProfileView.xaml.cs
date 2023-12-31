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
    /// Interaction logic for TeacherProfileView.xaml
    /// </summary>
    public partial class TeacherProfileView : UserControl
    {
        public TeacherProfileView()
        {
            InitializeComponent();
        }

        private void Box_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox txtBox = sender as TextBox;
            if(txtBox.Visibility == Visibility.Visible)
            {
                txtBox.Focus();
            }
        }

        private void UsernameTxtBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtBox = sender as TextBox;
            txtBox.Visibility = Visibility.Hidden;
        }
    }
}
