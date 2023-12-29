using Noutecon__Exam_.Model;
using Noutecon__Exam_.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Noutecon__Exam_.ViewModel
{
    public class TeacherRegisterViewModel : ViewModelBase
    {
        private string? _username;
        private SecureString? _password;
        private string? _firstName;
        private string? _lastName;
        private string? _school;
        private string? _usernameErrorMessage;

        public string? Username { get => _username; set { _username = value; OnPropertyChanged(nameof(Username)); } }
        public SecureString? Password { get => _password; set { _password = value; OnPropertyChanged(nameof(Password)); } }
        public string? FirstName { get => _firstName; set { _firstName = value; OnPropertyChanged(nameof(FirstName)); } }
        public string? LastName { get => _lastName; set { _lastName = value; OnPropertyChanged(nameof(LastName)); } }
        public string? School { get => _school; set { _school = value; OnPropertyChanged(nameof(School)); } }
        public string? UsernameErrorMessage { get => _usernameErrorMessage; set { _usernameErrorMessage = value; OnPropertyChanged(nameof(UsernameErrorMessage)); } }

        private LoginViewModel loginViewModel;

        private ITeacherRepository teacherRepository;
        private IStudentRepository studentRepository;

        public ICommand RegisterStudent { get; }
        public ICommand CancelRegistry { get; }

        public TeacherRegisterViewModel(LoginViewModel lvm)
        {
            loginViewModel = lvm;
            teacherRepository = new TeacherRepository();
            studentRepository = new StudentRepository();
            RegisterStudent = new ViewModelCommand(ExecuteRegisterStudent, CanExecuteRegisterStudent);
            CancelRegistry = new ViewModelCommand(ExecuteCancelRegistry);
        }

        private void ExecuteCancelRegistry(object obj)
        {
            loginViewModel.ShowLoginViewCommand.Execute(obj);
        }

        private bool CanExecuteRegisterStudent(object obj)
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(Username) || Username.Length <= 3 || Password == null || Password.Length <= 3 ||
                string.IsNullOrEmpty(FirstName) || FirstName.Length <= 3 ||
                string.IsNullOrEmpty(LastName) || LastName.Length <= 3 ||
                string.IsNullOrEmpty(School) || School.Length <= 3)
            {
                isValid = false;
            }
            return isValid;
        }

        private void ExecuteRegisterStudent(object obj)
        {
            if (teacherRepository.GetByUsername(Username) != null || studentRepository.GetByUsername(Username) != null)
            {
                UsernameErrorMessage = "User with the same username already exists!";
                return;
            }
            NetworkCredential nc = new NetworkCredential(Username, Password);
            TeacherModel teacher = new TeacherModel()
            {
                Username = nc.UserName,
                Password = nc.Password,
                FirstName = this.FirstName,
                LastName = this.LastName,
                School = this.School,
                ProfilePicturePath = "/Images/TeacherIcon.png"
            };
            teacherRepository.Add(teacher);
            ExecuteCancelRegistry(null);
        }

    }
}
