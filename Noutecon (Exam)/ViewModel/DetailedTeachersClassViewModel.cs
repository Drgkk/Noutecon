using Noutecon__Exam_.Model;
using Noutecon__Exam_.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Noutecon__Exam_.ViewModel
{
    public class DetailedTeachersClassViewModel : ViewModelBase
    {
		private ObservableCollection<StudentAccountModel> _students;

		public ObservableCollection<StudentAccountModel> Students
		{
			get { return _students; }
			set { _students = value; OnPropertyChanged(nameof(Students)); }
		}



		private ClassModel currentClass;
		private TeacherViewViewModel teacherViewViewModel;
		public ClassModel CurrentClass {  get { return currentClass; } set { currentClass = value; OnPropertyChanged(nameof(CurrentClass)); } }
		private IStudentRepository studentRepository;
		private IClassRepository classRepository;

		public ICommand ShowStudentDetails { get; }
		public ICommand DeleteStudent { get; }
		public ICommand ShowStudentCreationView { get; }
		public ICommand GoBack { get; }
		public ICommand GenerateNewUniqueCode { get; }

		public DetailedTeachersClassViewModel(object c)
		{
			object[] args = c as object[];
            CurrentClass = (ClassModel)args[0];
            teacherViewViewModel = (TeacherViewViewModel)args[1];
			studentRepository = new StudentRepository();
			classRepository = new ClassRepository();
			Students = studentRepository.GetStudentsAccountsByClassId(currentClass.Id);
			ShowStudentDetails = new ViewModelCommand(ExecuteShowStudentDetails);
			DeleteStudent = new ViewModelCommand(ExecuteDeleteStudent);
			ShowStudentCreationView = new ViewModelCommand(ExecuteShowStudentCreationView);
			GoBack = new ViewModelCommand(ExecuteGoBack);
			GenerateNewUniqueCode = new ViewModelCommand(ExecuteGenerateNewUniqueCode);
        }

        private void ExecuteGenerateNewUniqueCode(object obj)
        {
            ClassCodeGenerator generator = new ClassCodeGenerator();
			string newUniqueId = generator.GenerateCode();
            classRepository.UpdateUniqueIdOfClassWithId(CurrentClass.Id, newUniqueId);
			ClassModel newClassModel = CurrentClass;
			newClassModel.UniqueId = newUniqueId;
            CurrentClass = newClassModel;
        }

        private void ExecuteGoBack(object obj)
        {
			teacherViewViewModel.ShowClassesView.Execute(null);
        }

        private void ExecuteShowStudentCreationView(object obj)
        {
			teacherViewViewModel.ShowTeachersStudentCreationView.Execute(currentClass);
        }

        private void ExecuteDeleteStudent(object obj)
        {
			StudentAccountModel currentStudent = (StudentAccountModel)obj;
			if(MessageBox.Show($"Are you sure you want to delete student {currentStudent.Username} from class {currentClass.Name}?", "Delete a Student", MessageBoxButton.YesNo) == MessageBoxResult.No)
			{
				return;
			}
			studentRepository.RemoveFromClassById(currentStudent.Id, currentClass.Id);
            Students = studentRepository.GetStudentsAccountsByClassId(currentClass.Id);
        }

        private void ExecuteShowStudentDetails(object obj)
        {
            StudentAccountModel currentStudent = (StudentAccountModel)obj;
			throw new NotImplementedException();
        }
    }
}
