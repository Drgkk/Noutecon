using Noutecon__Exam_.Model;
using Noutecon__Exam_.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Noutecon__Exam_.ViewModel
{
    public class StudentClassesViewModel : ViewModelBase
    {
		private string searchText;

		public string SearchText
		{
			get { return searchText; }
			set { searchText = value; OnPropertyChanged(nameof(SearchText)); ExecuteRefresh(null); }
		}


		private ObservableCollection<ClassModel> classes;
		private ObservableCollection<ClassModel> allClasses;

		public ObservableCollection<ClassModel> Classes
		{
			get { return classes; }
			set { classes = value; OnPropertyChanged(nameof(Classes)); }
		}

		private ClassModel selectedClass;

		public ClassModel SelectedClass
		{
			get { return selectedClass; }
			set { selectedClass = value; OnPropertyChanged(nameof(SelectedClass)); }
		}

		private string errorMessage;

		public string ErrorMessage
		{
			get { return errorMessage; }
			set { errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); }
		}

		private string inviteCode;

		public string InviteCode
		{
			get { return inviteCode; }
			set { inviteCode = value; OnPropertyChanged(nameof(InviteCode)); }
		}


		private IClassRepository classRepository;
		private IStudentRepository studentRepository;
		private MainViewViewModel mainViewViewModel;

		public ICommand RegisterClass { get; }
		public ICommand Refresh { get; }

		public StudentClassesViewModel(MainViewViewModel mvvm)
		{
			mainViewViewModel = mvvm;
            RegisterClass = new ViewModelCommand(ExecuteRegisterClass, CanExecuteRegisterClass);
			Refresh = new ViewModelCommand(ExecuteRefresh);
			classRepository = new ClassRepository();
			studentRepository = new StudentRepository();
			allClasses = classRepository.GetClassesByStudentId(mainViewViewModel.CurrentStudentAccount.Id);
			Classes = allClasses;
        }

        private bool CanExecuteRegisterClass(object obj)
        {
			bool isValid = true;
			if (string.IsNullOrEmpty(InviteCode) || InviteCode.Length != 6)
				isValid = false;
			return isValid;
        }

        private void ExecuteRefresh(object obj)
        {
			SelectedClass = null;
			Classes = new ObservableCollection<ClassModel>(allClasses.Where(o => o.Name.ToLower().Contains(SearchText.ToLower())));
        }

        private void ExecuteRegisterClass(object obj)
        {
            if(!classRepository.ValidateClass(InviteCode))
			{
				ErrorMessage = "Wrong Invite Code!";
				return;
			}
			if(studentRepository.IsStudentInClass(mainViewViewModel.CurrentStudentAccount.Id, classRepository.GetId(InviteCode)))
			{
                ErrorMessage = "You're already in this class!";
                return;
            }
			studentRepository.AddStudentToClassById(mainViewViewModel.CurrentStudentAccount.Id, classRepository.GetId(InviteCode));
            allClasses = classRepository.GetClassesByStudentId(mainViewViewModel.CurrentStudentAccount.Id);
            Classes = allClasses;
        }
    }
}
