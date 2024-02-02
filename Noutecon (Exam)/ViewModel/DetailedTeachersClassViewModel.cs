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
		private ObservableCollection<StudentAccountModelForClassDetailedListView> _students;

		public ObservableCollection<StudentAccountModelForClassDetailedListView> Students
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
			Students = new ObservableCollection<StudentAccountModelForClassDetailedListView>();
			foreach(var st in studentRepository.GetStudentsAccountsByClassId(currentClass.Id))
			{
				Students.Add(new StudentAccountModelForClassDetailedListView(teacherViewViewModel.CurrentTeacher.Id) { Student = st });
			}
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
            foreach (var st in studentRepository.GetStudentsAccountsByClassId(currentClass.Id))
            {
                Students.Add(new StudentAccountModelForClassDetailedListView(teacherViewViewModel.CurrentTeacher.Id) { Student = st });
            }
        }

        private void ExecuteShowStudentDetails(object obj)
        {
            StudentAccountModelForClassDetailedListView currentStudent = (StudentAccountModelForClassDetailedListView)obj;
			teacherViewViewModel.ShowStudentDetailedTestsView.Execute(new object[] { currentStudent.Student, currentClass });
        }
    }

	public class StudentAccountModelForClassDetailedListView
	{
		private StudentAccountModel student;

		public StudentAccountModel Student
		{
			get { return student; }
			set { student = value; OnStudentChanged(); }
		}

        

        private double averageGrade;

		public double AverageGrade
		{
			get { return averageGrade; }
			set { averageGrade = value; }
		}

		private IStudentRepository studentRepository;
		private int currentTeacherOfStudent;
        public StudentAccountModelForClassDetailedListView(int currentTeacherOfStudent)
        {
			studentRepository = new StudentRepository();
            this.currentTeacherOfStudent = currentTeacherOfStudent;
        }

        private void OnStudentChanged()
        {
			ObservableCollection<double> grades = studentRepository.GetAllGradesFromStudentOfTeacher(Student.Id, currentTeacherOfStudent);
		    if(grades.Count > 0)
			{
                AverageGrade = Math.Round(grades.Average(), 1);
            }
            
        }

    }

}
