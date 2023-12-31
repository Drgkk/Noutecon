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
    public class StudentRegisterViewModel : ViewModelBase
    {
        private string? _username;
        private SecureString? _password;
        private string? _firstName;
        private string? _lastName;
        private string? _inviteCode;
        private string? _usernameErrorMessage;
        //private string? _inviteCodeErrorMessage;

        public string? Username { get => _username; set { _username = value; OnPropertyChanged(nameof(Username)); } }
        public SecureString? Password { get => _password; set { _password = value; OnPropertyChanged(nameof(Password)); } }
        public string? FirstName { get => _firstName; set { _firstName = value; OnPropertyChanged(nameof(FirstName)); } }
        public string? LastName { get => _lastName; set { _lastName = value; OnPropertyChanged(nameof(LastName)); } }
        public string? InviteCode { get => _inviteCode; set { _inviteCode = value; OnPropertyChanged(nameof(InviteCode)); } }
        public string? UsernameErrorMessage { get => _usernameErrorMessage; set { _usernameErrorMessage = value; OnPropertyChanged(nameof(UsernameErrorMessage)); } }
        //public string? InviteCodeErrorMessage { get => _inviteCodeErrorMessage; set { _inviteCodeErrorMessage = value; OnPropertyChanged(nameof(InviteCodeErrorMessage)); } }

        private LoginViewModel loginViewModel;

        private IStudentRepository studentRepository;
        private ITeacherRepository teacherRepository;
        private IClassRepository classRepository;

        public ICommand RegisterStudent { get; }
        public ICommand CancelRegistry { get; }
        

        public StudentRegisterViewModel(LoginViewModel lvm)
        {
            loginViewModel = lvm;
            studentRepository = new StudentRepository();
            teacherRepository = new TeacherRepository();
            classRepository = new ClassRepository();
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
                string.IsNullOrEmpty(FirstName) || FirstName.Length <=3 ||
                string.IsNullOrEmpty(LastName) || LastName.Length <= 3 //||
                /*string.IsNullOrEmpty(InviteCode) || InviteCode.Length != 6*/)
            {
                isValid = false;
            }
            return isValid;
        }

        private void ExecuteRegisterStudent(object obj)
        {
            UsernameErrorMessage = "";
            //InviteCodeErrorMessage = "";
            if(studentRepository.GetByUsername(Username) != null || teacherRepository.GetByUsername(Username) != null)
            {
                UsernameErrorMessage = "User with the same username already exists!";
                return;
            }
            //if(!classRepository.ValidateClass(InviteCode))
            //{
            //    InviteCodeErrorMessage = "Invalid Invite Code!";
            //    return;
            //}
            NetworkCredential nc = new NetworkCredential(Username, Password);
            StudentModel student = new StudentModel()
            {
                Username = nc.UserName,
                Password = nc.Password,
                FirstName = this.FirstName,
                LastName = this.LastName,
                //ClassId = classRepository.GetId(InviteCode),
                ProfilePicturePath = "/Images/StudentIcon.png"
            };
            studentRepository.Add(student);
            ExecuteCancelRegistry(null);
        }
    }
}
