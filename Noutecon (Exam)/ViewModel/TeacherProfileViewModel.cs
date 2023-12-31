using Noutecon__Exam_.Model;
using Noutecon__Exam_.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Noutecon__Exam_.ViewModel
{
    public class TeacherProfileViewModel : ViewModelBase
    {
        private TeacherAccountModel currentTeacher;

        public TeacherAccountModel CurrentTeacher
        {
            get { return currentTeacher; }
            set { currentTeacher = value; OnPropertyChanged(nameof(CurrentTeacher)); }
        }

        private bool usernameTextBoxVisibility;

        public bool UsernameTextBoxVisibility
        {
            get { return usernameTextBoxVisibility; }
            set { usernameTextBoxVisibility = value; OnPropertyChanged(nameof(UsernameTextBoxVisibility)); UsernameTextBoxLostFocus(); }
        }

        private bool usernameTextVisibility;

        public bool UsernameTextVisibility
        {
            get { return usernameTextVisibility; }
            set { usernameTextVisibility = value; OnPropertyChanged(nameof(UsernameTextVisibility)); }
        }

        private string usernameErrorMessage;

        public string UsernameErrorMessage
        {
            get { return usernameErrorMessage; }
            set { usernameErrorMessage = value; OnPropertyChanged(nameof(UsernameErrorMessage)); }
        }

        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; OnPropertyChanged(nameof(Username)); }
        }

        //
        private bool firstnameTextBoxVisibility;

        public bool FirstnameTextBoxVisibility
        {
            get { return firstnameTextBoxVisibility; }
            set { firstnameTextBoxVisibility = value; OnPropertyChanged(nameof(FirstnameTextBoxVisibility)); FirstNameTextBoxLostFocus(); }
        }

        private bool firstNameTextVisibility;

        public bool FirstNameTextVisibility
        {
            get { return firstNameTextVisibility; }
            set { firstNameTextVisibility = value; OnPropertyChanged(nameof(FirstNameTextVisibility)); }
        }

        private string firstNameErrorMessage;

        public string FirstNameErrorMessage
        {
            get { return firstNameErrorMessage; }
            set { firstNameErrorMessage = value; OnPropertyChanged(nameof(FirstNameErrorMessage)); }
        }

        private string firstName;

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; OnPropertyChanged(nameof(FirstName)); }
        }


        private TeacherViewViewModel teacherViewViewModel;
        private ITeacherRepository teacherRepository;
        private IStudentRepository studentRepository;

        public ICommand ChangePFP { get; }
        public ICommand ChangeUsername { get; }
        public ICommand ChangeUsernameVisibility { get; }

        public ICommand ChangeFirstName { get; }
        public ICommand ChangeFirstNameVisibility { get; }

        public TeacherProfileViewModel(TeacherViewViewModel tvvm)
        {
            teacherViewViewModel = tvvm;
            CurrentTeacher = teacherViewViewModel.CurrentTeacher;
            teacherRepository = new TeacherRepository();
            studentRepository = new StudentRepository();
            ChangePFP = new ViewModelCommand(ExecuteChangePFP);
            ChangeUsername = new ViewModelCommand(ExecuteChangeUsername);
            ChangeUsernameVisibility = new ViewModelCommand(ExecuteChangeUsernameVisibility);
            ChangeFirstName = new ViewModelCommand(ExecuteChangeFirstName);
            ChangeFirstNameVisibility = new ViewModelCommand(ExecuteChangeFirstNameVisibility);
            UsernameTextBoxVisibility = false;
            UsernameTextVisibility = true;
        }

        private void FirstNameTextBoxLostFocus()
        {
            if (firstnameTextBoxVisibility == false)
            {
                firstnameTextBoxVisibility = false;
                FirstNameTextVisibility = true;
            }
        }
        private void ExecuteChangeFirstNameVisibility(object obj)
        {
            FirstNameErrorMessage = "";
            FirstName = currentTeacher.FirstName;
            FirstNameTextVisibility = false;
            FirstnameTextBoxVisibility = true;
        }

        private void ExecuteChangeFirstName(object obj)
        {
            FirstNameTextVisibility = true;
            FirstnameTextBoxVisibility = false;
            teacherRepository.EditTeacherFirstName(currentTeacher.Id, FirstName);
            TeacherAccountModel model = teacherRepository.GetAccountById(teacherViewViewModel.CurrentTeacher.Id);
            teacherViewViewModel.CurrentTeacher = model;
            CurrentTeacher = model;
        }

        private void ExecuteChangeUsernameVisibility(object obj)
        {
            UsernameErrorMessage = "";
            Username = currentTeacher.Username;
            UsernameTextVisibility = false;
            UsernameTextBoxVisibility = true;
        }

        private void UsernameTextBoxLostFocus()
        {
            if(usernameTextBoxVisibility ==  false)
            {
                usernameTextBoxVisibility = false;
                UsernameTextVisibility = true;
            }
        }

        private void ExecuteChangeUsername(object obj)
        {
            UsernameTextVisibility = true;
            UsernameTextBoxVisibility = false;
            if(teacherRepository.GetByUsername(Username) != null || studentRepository.GetByUsername(Username) != null)
            {
                UsernameErrorMessage = "Account with that username already exists!";
                return;
            }
            teacherRepository.EditTeacherUsername(currentTeacher.Id, Username);
            TeacherAccountModel model = teacherRepository.GetAccountById(teacherViewViewModel.CurrentTeacher.Id);
            teacherViewViewModel.CurrentTeacher = model;
            CurrentTeacher = model;
        }

        private void ExecuteChangePFP(object obj)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png";
            if(fileDialog.ShowDialog()  == DialogResult.OK)
            {
                teacherRepository.EditPfpById(teacherViewViewModel.CurrentTeacher.Id, fileDialog.FileName);
                TeacherAccountModel model = teacherRepository.GetAccountById(teacherViewViewModel.CurrentTeacher.Id);
                teacherViewViewModel.CurrentTeacher = model;
                CurrentTeacher = model;
            }

        }


    }
}
