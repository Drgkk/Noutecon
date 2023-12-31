using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FontAwesome.Sharp;
using Noutecon__Exam_.Model;
using Noutecon__Exam_.Repositories;

namespace Noutecon__Exam_.ViewModel
{
    public class MainViewViewModel : ViewModelBase
    {
        private ViewModelBase currentChildView;

        public ViewModelBase CurrentChildView
        {
            get { return currentChildView; }
            set { currentChildView = value; OnPropertyChanged(nameof(CurrentChildView)); }
        }

        private IconChar icon;

        public IconChar Icon
        {
            get { return icon; }
            set { icon = value; OnPropertyChanged(nameof(Icon)); }
        }

        private string label;

        public string Label
        {
            get { return label; }
            set { label = value; OnPropertyChanged(nameof(Label)); }
        }



        private StudentAccountModel currentStudentAccount;
        private IStudentRepository studentRepository;

        public StudentAccountModel CurrentStudentAccount { get => currentStudentAccount; set { currentStudentAccount = value; OnPropertyChanged(nameof(CurrentStudentAccount)); } }

        public ICommand ShowClassesView { get; }
        public ICommand ShowHomeView { get; }
        public ICommand ShowTestsView { get; }
        public ICommand ShowProfileView { get; }
        public ICommand ShowSettingsView { get; }

        public MainViewViewModel()
        {
            studentRepository = new StudentRepository();
            CurrentStudentAccount = new StudentAccountModel();
            LoadCurrentUserData();
            ShowClassesView = new ViewModelCommand(ExecuteShowClassesView);
            ShowHomeView = new ViewModelCommand(ExecuteShowHomeView);
            ShowTestsView = new ViewModelCommand(ExecuteShowTestsView);
            ShowProfileView = new ViewModelCommand(ExecuteShowProfileView);
            ShowSettingsView = new ViewModelCommand(ExecuteShowSettingsView);
            ExecuteShowHomeView(null);
            //Thread validateStudentThread = new Thread(new ThreadStart(ThreadWorkValidateUser));
            //validateStudentThread.IsBackground = true;
            //validateStudentThread.Start();  
        }

        private void ExecuteShowSettingsView(object obj)
        {
            CurrentChildView = new StudentSettingsViewModel();
            Icon = IconChar.Gear;
            Label = "Settings";
        }

        private void ExecuteShowProfileView(object obj)
        {
            CurrentChildView = new StudentProfileViewModel();
            Icon = IconChar.UserAlt;
            Label = "Your Profile";
        }

        private void ExecuteShowTestsView(object obj)
        {
            CurrentChildView = new StudentTestsViewModel();
            Icon = IconChar.Book;
            Label = "Tests";
        }

        private void ExecuteShowHomeView(object obj)
        {
            CurrentChildView = new StudentHomeViewModel();
            Icon = IconChar.Home;
            Label = "Home";
        }

        private void ExecuteShowClassesView(object obj)
        {
            CurrentChildView = new StudentClassesViewModel(this);
            Icon = IconChar.UserGroup;
            Label = "My Classes";
        }

        private void LoadCurrentUserData()
        {
            var student = studentRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if(student != null)
            {
                CurrentStudentAccount.Id = student.Id;
                CurrentStudentAccount.Username = student.Username;
                CurrentStudentAccount.FirstName = student.FirstName;
                CurrentStudentAccount.LastName = student.LastName;             
                //CurrentStudentAccount.ClassId = student.ClassId;
                CurrentStudentAccount.ProfilePicturePath = student.ProfilePicturePath;
            }
            else
            {
                MessageBox.Show("Invalid student, not logged in!");
                Application.Current.Shutdown();
            }
        }

        private void ThreadWorkValidateUser()
        {
            while (true)
            {
                var student = studentRepository.GetByUsername(CurrentStudentAccount.Username);
                if (student == null)
                {
                    Application.Current.Shutdown();
                }
                Thread.Sleep(1000);
            }
        }

    }
}
