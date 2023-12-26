using Noutecon__Exam_.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Noutecon__Exam_.Repositories;
using System.Net;
using System.Threading;
using System.Security.Principal;
using System.Windows;
using Noutecon__Exam_.Enums;
using Noutecon__Exam_.View;

namespace Noutecon__Exam_.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private ViewModelBase _currentChildView;
        private bool isVisible;
        private LoginViewEnum _viewChange = LoginViewEnum.None;

        public ViewModelBase CurrentChildView { get => _currentChildView; set { _currentChildView = value; OnPropertyChanged(nameof(CurrentChildView)); } }
        public bool IsVisible { get => isVisible; set { isVisible = value; OnPropertyChanged(nameof(isVisible)); } }
        public LoginViewEnum ViewChange { get => _viewChange; set { _viewChange = value; OnPropertyChanged(nameof(ViewChange)); } }
        public ICommand ShowLoginViewCommand { get; }
        public ICommand ShowRegisterViewCommand { get; }
        public ICommand ShowTeacherRegisterView { get; }
        public ICommand ShowStudentRegisterView { get; }


        public LoginViewModel()
        {
            ShowLoginViewCommand = new ViewModelCommand(ExecuteShowLoginViewCommand);
            ShowRegisterViewCommand = new ViewModelCommand(ExecuteShowRegisterViewCommand);
            ShowTeacherRegisterView = new ViewModelCommand(ExecuteShowTeacherRegisterView);
            ShowStudentRegisterView = new ViewModelCommand(ExecuteShowStudentRegisterView);
            ExecuteShowLoginViewCommand(null);
        }

       

        private void ExecuteShowRegisterViewCommand(object obj)
        {
            CurrentChildView = new RegisterUserControlViewModel(this);
        }

        private void ExecuteShowLoginViewCommand(object obj)
        {
            CurrentChildView = new LoginUserControlViewModel(this);
        }
        private void ExecuteShowStudentRegisterView(object obj)
        {
            CurrentChildView = new StudentRegisterViewModel(this);
        }

        private void ExecuteShowTeacherRegisterView(object obj)
        {
            CurrentChildView = new TeacherRegisterViewModel(this);
        }

        //private string _username;
        //private SecureString _password;
        //private string _errorMessage;
        //private bool _isViewVisible = true;
        //private LoginViewEnum _viewChange = LoginViewEnum.None;

        //private IStudentRepository studentRepository;
        //private ITeacherRepository teacherRepository;

        //public string Username { get => _username; set { _username = value; OnPropertyChanged(nameof(Username)); } }
        //public SecureString Password { get => _password; set { _password = value; OnPropertyChanged(nameof(Password)); } }
        //public string ErrorMessage { get => _errorMessage; set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); } }
        //public bool IsViewVisible { get => _isViewVisible; set { _isViewVisible = value; OnPropertyChanged(nameof(IsViewVisible)); } }
        //public LoginViewEnum ViewChange { get => _viewChange; set { _viewChange = value; OnPropertyChanged(nameof(ViewChange)); } }

        //public ICommand LoginCommand { get; }
        //public ICommand RegisterCommand { get; }

        //public LoginViewModel()
        //{
        //    studentRepository = new StudentRepository();
        //    teacherRepository = new TeacherRepository();
        //    LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
        //    RegisterCommand = new ViewModelCommand(ExecuteRegisterCommand);
        //}

        //private bool CanExecuteLoginCommand(object obj)
        //{
        //    bool validData = true;
        //    if(string.IsNullOrEmpty(Username) || Username.Length <= 3 || Password == null || Password.Length <= 3)
        //    {
        //        validData = false;
        //    }
        //    return validData;
        //}

        //private void ExecuteLoginCommand(object obj)
        //{
        //    NetworkCredential nc = new NetworkCredential(Username, Password);
        //    var isValidUser = studentRepository.AuthenticateUser(nc);
        //    if(isValidUser)
        //    {
        //        Thread.CurrentPrincipal = new GenericPrincipal(
        //            new GenericIdentity(Username), null);

        //        ViewChange = LoginViewEnum.StudentView;
        //        IsViewVisible = false;
        //        return;
        //    }
        //    else
        //    {
        //        ErrorMessage = "* Invalid username or password";
        //    }
        //    var isValidTeacher = teacherRepository.AuthenticateUser(nc);
        //    if (isValidTeacher)
        //    {
        //        Thread.CurrentPrincipal = new GenericPrincipal(
        //            new GenericIdentity(Username), null);

        //        ViewChange = LoginViewEnum.TeacherView;
        //        IsViewVisible = false;
        //        return;
        //    }
        //    else
        //    {
        //        ErrorMessage = "* Invalid username or password";
        //    }
        //}

        //private void ExecuteRegisterCommand(object obj)
        //{
        //    throw new NotImplementedException();
        //}

    }
}
