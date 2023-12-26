using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Noutecon__Exam_.View;

namespace Noutecon__Exam_
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected void ApplicationStart(object sender, StartupEventArgs e)
        {
            var loginView = new LoginWindow();
            loginView.Show();
            loginView.IsVisibleChanged += (s, ev) =>
            {
                if (loginView.IsVisible == false && loginView.IsLoaded && loginView.viewChangeTextBlock.Text == "STUDENTVIEW")
                {
                    var mainView = new MainWindow();
                    mainView.Show();
                }
                else if (loginView.IsVisible == false && loginView.IsLoaded && loginView.viewChangeTextBlock.Text == "TEACHERVIEW")
                {
                    var teacherView = new TeacherView();
                    teacherView.Show();
                }
            };
        }
    }
}
