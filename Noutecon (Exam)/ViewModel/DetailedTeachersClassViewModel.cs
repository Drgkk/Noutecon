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

		public ICommand ShowStudentDetails { get; }
		public ICommand DeleteStudent { get; }
		public ICommand ShowStudentCreationView { get; }
		public ICommand GoBack { get; }

		public DetailedTeachersClassViewModel(object c)
		{
			object[] args = c as object[];
            CurrentClass = (ClassModel)args[0];
            teacherViewViewModel = (TeacherViewViewModel)args[1];
			studentRepository = new StudentRepository();
			Students = studentRepository.GetStudentsAccountsByClassId(currentClass.Id);
			ShowStudentDetails = new ViewModelCommand(ExecuteShowStudentDetails);
			DeleteStudent = new ViewModelCommand(ExecuteDeleteStudent);
			ShowStudentCreationView = new ViewModelCommand(ExecuteShowStudentCreationView);
			GoBack = new ViewModelCommand(ExecuteGoBack);
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
			studentRepository.RemoveById(currentStudent.Id);
            Students = studentRepository.GetStudentsAccountsByClassId(currentClass.Id);
        }

        private void ExecuteShowStudentDetails(object obj)
        {
            StudentAccountModel currentStudent = (StudentAccountModel)obj;

        }
    }
}
