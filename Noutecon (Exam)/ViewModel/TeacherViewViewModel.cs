using FontAwesome.Sharp;
using Noutecon__Exam_.Model;
using Noutecon__Exam_.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Noutecon__Exam_.ViewModel
{
    class TeacherViewViewModel : ViewModelBase
    {
        private TeacherAccountModel currentTeacher;
        private ITeacherRepository teacherRepository;
        private ViewModelBase currentChildView;
        private string caption;
        private IconChar icon;
        private bool isStartViewChecked = true;

        public TeacherAccountModel CurrentTeacher { get => currentTeacher; set { currentTeacher = value; OnPropertyChanged(nameof(CurrentTeacher)); } }

        public ViewModelBase CurrentChildView { get => currentChildView; set { currentChildView = value; OnPropertyChanged(nameof(CurrentChildView)); } }

        public string Caption { get => caption; set { caption = value; OnPropertyChanged(nameof(Caption)); } }
        public IconChar Icon { get => icon; set { icon = value; OnPropertyChanged(nameof(Icon)); } }
        public bool IsStartViewChecked { get => isStartViewChecked; set { isStartViewChecked = value; OnPropertyChanged(nameof(IsStartViewChecked)); } }

        public ICommand ShowHomeView { get; }
        public ICommand ShowTestsView { get; }
        public ICommand ShowProfileView { get; }
        public ICommand ShowExploreView { get; }
        public ICommand ShowSettingsView { get; }
       

        public TeacherViewViewModel()
        {
            CurrentTeacher = new TeacherAccountModel();
            teacherRepository = new TeacherRepository();
            LoadCurrentTeacherData();
            ShowHomeView = new ViewModelCommand(ExecuteShowHomeView);
            ShowTestsView = new ViewModelCommand(ExecuteShowTestsView);
            ShowProfileView = new ViewModelCommand(ExecuteShowProfileView);
            ShowExploreView = new ViewModelCommand(ExecuteShowExploreView);
            ShowSettingsView = new ViewModelCommand(ExecuteShowSettingsView);
            ExecuteShowHomeView(null);
        }

        private void ExecuteShowSettingsView(object obj)
        {
            CurrentChildView = new TeacherSettingsViewModel();
            Caption = "Settings";
            Icon = IconChar.Gear;
        }

        private void ExecuteShowExploreView(object obj)
        {
            CurrentChildView = new TeacherExploreViewModel();
            Caption = "Explore";
            Icon = IconChar.MagnifyingGlass;
        }

        private void ExecuteShowProfileView(object obj)
        {
            CurrentChildView = new TeacherProfileViewModel();
            Caption = "Your Profile";
            Icon = IconChar.UserAlt;
        }

        private void ExecuteShowTestsView(object obj)
        {
            CurrentChildView = new TeacherTestsViewModel();
            Caption = "Tests";
            Icon = IconChar.Book;
        }

        private void ExecuteShowHomeView(object obj)
        {
            CurrentChildView = new TeacherHomeViewModel();
            Caption = "Home";
            Icon = IconChar.Home;
        }

        private void LoadCurrentTeacherData()
        {
            var teacher = teacherRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if(teacher != null)
            {
                CurrentTeacher = new TeacherAccountModel()
                {
                    Username = teacher.Username,
                    FirstName = teacher.FirstName,
                    LastName = teacher.LastName,
                };
            }
            else
            {
                MessageBox.Show("Invalid teacher, not logged in!");
                Application.Current.Shutdown();
            }
        }


    }
}
