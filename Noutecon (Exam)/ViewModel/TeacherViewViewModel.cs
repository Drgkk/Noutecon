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
    public class TeacherViewViewModel : ViewModelBase
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
        public ICommand ShowClassesView { get; }    
        public ICommand ShowClassesRegisterView { get; }    
        public ICommand ShowDetailedClassView { get; }
        public ICommand ShowTeachersStudentCreationView { get; }
        public ICommand ShowTestsCreationView { get; }
        public ICommand ShowTestsSettingsView { get; }
       public ICommand ShowTestsAssignClassesView { get; }
        public ICommand ShowDetailedStudentsSelectionView { get; }
        public ICommand ShowStudentDetailedTestsView { get; }

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
            ShowClassesView = new ViewModelCommand(ExecuteShowClassesView);
            ShowClassesRegisterView = new ViewModelCommand(ExecuteShowClassesRegisterView);
            ShowDetailedClassView = new ViewModelCommand(ExecuteShowDetailedClassView);
            ShowTeachersStudentCreationView = new ViewModelCommand(ExecuteShowTeachersStudentCreationView);
            ShowTestsCreationView = new ViewModelCommand(ExecuteShowTestsCreationView);
            ShowTestsSettingsView = new ViewModelCommand(ExecuteShowTestsSettingsView);
            ShowTestsAssignClassesView = new ViewModelCommand(ExecuteShowTestsAssignClassesView);
            ShowDetailedStudentsSelectionView = new ViewModelCommand(ExecuteShowDetailedStudentsSelectionView);
            ShowStudentDetailedTestsView = new ViewModelCommand(ExecuteShowStudentDetailedTestsView);
            ExecuteShowHomeView(null);
        }

        private void ExecuteShowStudentDetailedTestsView(object obj)
        {
            object[] ar = obj as object[];
            CurrentChildView = new StudentDetailedTestsViewModel(this, ar[0] as StudentAccountModel, ar[1] as ClassModel);
        }

        private void ExecuteShowDetailedStudentsSelectionView(object obj)
        {
            object[] ar = obj as object[];
            CurrentChildView = new DetailedStudentsSelectionViewModel(ar[0] as TeacherViewViewModel, ar[1] as List<AssignedClassWithStudentsClass>, ar[2] as ClassModel, ar[3] as TestModel, ar[4] as TestModel);
        }

        private void ExecuteShowTestsAssignClassesView(object obj)
        {
            object[] ar = obj as object[];
            CurrentChildView = new TeacherTestAssignViewModel(ar[0] as TeacherViewViewModel, ar[1] as List<AssignedClassWithStudentsClass>, ar[2] as TestModel, ar[3] as TestModel);
        }

        private void ExecuteShowTestsSettingsView(object obj)
        {
            CurrentChildView = new TestSettingsViewModel(this, obj as TestModel);
        }

        private void ExecuteShowTestsCreationView(object obj)
        {
            object[] ar = obj as object[];
            CurrentChildView = new TestCreationViewModel(this, ar[0] as TestModel, ar[1] as TestModel);
        }

        private void ExecuteShowTeachersStudentCreationView(object obj)
        {
            CurrentChildView = new TeachersStudentCreationViewModel(new object[2] { this, obj });
        }

        private void ExecuteShowDetailedClassView(object obj)
        {
            CurrentChildView = new DetailedTeachersClassViewModel(new object[2] { obj, this } );
        }

        private void ExecuteShowClassesRegisterView(object obj)
        {
            CurrentChildView = new ClassRegisterViewModel(this);
        }

        private void ExecuteShowClassesView(object obj)
        {
            CurrentChildView = new TeacherClassesViewModel(this);
            Caption = "Manage Classes";
            Icon = IconChar.UserGroup;
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
            CurrentChildView = new TeacherProfileViewModel(this);
            Caption = "Your Profile";
            Icon = IconChar.UserAlt;
        }

        private void ExecuteShowTestsView(object obj)
        {
            CurrentChildView = new TeacherTestsViewModel(this);
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
                    Id = teacher.Id,
                    Username = teacher.Username,
                    FirstName = teacher.FirstName,
                    LastName = teacher.LastName,
                    School = teacher.School,
                    ProfilePicturePath = teacher.ProfilePicturePath
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
