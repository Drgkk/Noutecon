using Noutecon__Exam_.Enums;
using Noutecon__Exam_.Model;
using Noutecon__Exam_.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Noutecon__Exam_.ViewModel
{
    class LoginUserControlViewModel : ViewModelBase
    {
        private string? _username;
        private SecureString? _password;
        private string? _errorMessage;
        

        private IStudentRepository studentRepository;
        private ITeacherRepository teacherRepository;
        private LoginViewModel loginViewModel;

        public string Username { get => _username; set { _username = value; OnPropertyChanged(nameof(Username)); } }
        public SecureString Password { get => _password; set { _password = value; OnPropertyChanged(nameof(Password)); } }
        public string ErrorMessage { get => _errorMessage; set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); } }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public LoginUserControlViewModel()
        {
            studentRepository = new StudentRepository();
            teacherRepository = new TeacherRepository();
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            RegisterCommand = new ViewModelCommand(ExecuteRegisterCommand);
        }

        public LoginUserControlViewModel(LoginViewModel lvm)
        {
            studentRepository = new StudentRepository();
            teacherRepository = new TeacherRepository();
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            RegisterCommand = new ViewModelCommand(ExecuteRegisterCommand);
            loginViewModel = lvm;
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData = true;
            if (string.IsNullOrEmpty(Username) || Username.Length <= 3 || Password == null || Password.Length <= 3)
            {
                validData = false;
            }
            return validData;
        }

        private void ExecuteLoginCommand(object obj)
        {
            NetworkCredential nc = new NetworkCredential(Username, Password);
            var isValidUser = studentRepository.AuthenticateUser(nc);
            if (isValidUser)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(
                    new GenericIdentity(Username), null);

                loginViewModel.ViewChange = LoginViewEnum.StudentView;
                loginViewModel.IsVisible = false;
                
                return;
            }
            else
            {
                ErrorMessage = "* Invalid username or password";
            }
            var isValidTeacher = teacherRepository.AuthenticateUser(nc);
            if (isValidTeacher)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(
                    new GenericIdentity(Username), null);

                loginViewModel.ViewChange = LoginViewEnum.TeacherView;
                loginViewModel.IsVisible = false;
                return;
            }
            else
            {
                ErrorMessage = "* Invalid username or password";
            }
        }

        private void ExecuteRegisterCommand(object obj)
        {
            loginViewModel.ShowRegisterViewCommand.Execute(null);
        }
    }
}
