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
    public class TeachersStudentCreationViewModel : ViewModelBase
    {
		private string _username;

		public string Username
		{
			get { return _username; }
			set { _username = value; OnPropertyChanged(nameof(Username)); }
		}

		private SecureString _password;

		public SecureString Password
		{
			get { return _password; }
			set { _password = value; OnPropertyChanged(nameof(Password)); }
		}

		private string _firstName;

		public string FirstName
		{
			get { return _firstName; }
			set { _firstName = value; OnPropertyChanged(nameof(FirstName)); }
		}

		private string _lastName;

		public string LastName
		{
			get { return _lastName; }
			set { _lastName = value; OnPropertyChanged(nameof(LastName)); }
		}

		private string _usernameErrorMessage;

		public string UsernameErrorMessage
		{
			get { return _usernameErrorMessage; }
			set { _usernameErrorMessage = value; OnPropertyChanged(nameof(UsernameErrorMessage)); }
		}

        private ClassModel currentClass;
        private TeacherViewViewModel teacherViewViewModel;

        private IStudentRepository studentRepository;
        private ITeacherRepository teacherRepository;
        private IClassRepository classRepository;

        public ICommand RegisterStudent { get; }
        public ICommand CancelRegistry { get; }

		public TeachersStudentCreationViewModel(object c)
		{
            object[] args = c as object[];
            teacherViewViewModel = (TeacherViewViewModel)args[0];
            currentClass = (ClassModel)args[1];
            studentRepository = new StudentRepository();
            teacherRepository = new TeacherRepository();
            classRepository = new ClassRepository();
            RegisterStudent = new ViewModelCommand(ExecuteRegisterStudent, CanExecuteRegisterStudent);
            CancelRegistry = new ViewModelCommand(ExecuteCancelRegistry);
        }

        private void ExecuteCancelRegistry(object obj)
        {
            teacherViewViewModel.ShowDetailedClassView.Execute(currentClass);
        }

        private bool CanExecuteRegisterStudent(object obj)
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(Username) || Username.Length <= 3 || Password == null || Password.Length <= 3 ||
                string.IsNullOrEmpty(FirstName) || FirstName.Length <= 3 ||
                string.IsNullOrEmpty(LastName) || LastName.Length <= 3)
            {
                isValid = false;
            }
            return isValid;
        }

        private void ExecuteRegisterStudent(object obj)
        {
            UsernameErrorMessage = "";
            if (studentRepository.GetByUsername(Username) != null || teacherRepository.GetByUsername(Username) != null)
            {
                UsernameErrorMessage = "User with the same username already exists!";
                return;
            }
            NetworkCredential nc = new NetworkCredential(Username, Password);
            StudentModel student = new StudentModel()
            {
                Username = nc.UserName,
                Password = nc.Password,
                FirstName = this.FirstName,
                LastName = this.LastName,
                ProfilePicturePath = "/Images/StudentIcon.png"
            };
            studentRepository.Add(student);
            studentRepository.AddStudentToClassById(studentRepository.GetStudentIdByUsername(student.Username), currentClass.Id);
            ExecuteCancelRegistry(null);
        }

    }
}
