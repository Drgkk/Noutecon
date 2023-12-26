using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Noutecon__Exam_.ViewModel
{
    class RegisterUserControlViewModel : ViewModelBase
    {
        private LoginViewModel loginViewModel;


        public ICommand GoBackToLoginView { get; }
        public ICommand ShowTeacherRegisterView { get; }
        public ICommand ShowStudentRegisterView { get; }

        public RegisterUserControlViewModel() 
        {
            GoBackToLoginView = new ViewModelCommand(ExecuteGoBackToLoginView);
            ShowTeacherRegisterView = new ViewModelCommand(ExecuteShowTeacherRegisterView);
            ShowStudentRegisterView = new ViewModelCommand(ExecuteShowStudentRegisterView);
        }               

        public RegisterUserControlViewModel(LoginViewModel lvm)
        {
            GoBackToLoginView = new ViewModelCommand(ExecuteGoBackToLoginView);
            ShowTeacherRegisterView = new ViewModelCommand(ExecuteShowTeacherRegisterView);
            ShowStudentRegisterView = new ViewModelCommand(ExecuteShowStudentRegisterView);
            loginViewModel = lvm;
        }

        private void ExecuteShowStudentRegisterView(object obj)
        {
            loginViewModel.ShowStudentRegisterView.Execute(obj);
        }

        private void ExecuteShowTeacherRegisterView(object obj)
        {
            loginViewModel.ShowTeacherRegisterView.Execute(obj);
        }

        private void ExecuteGoBackToLoginView(object obj)
        {
            loginViewModel.ShowLoginViewCommand.Execute(null);
        }

    }
}
