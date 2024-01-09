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
    public class DetailedStudentsSelectionViewModel : ViewModelBase
    {
        private ObservableCollection<StudentAccountModel> students;

        public ObservableCollection<StudentAccountModel> Students
        {
            get { return students; }
            set { students = value; OnPropertyChanged(nameof(Students)); }
        }

        private TeacherViewViewModel teacherViewViewModel;
        private List<AssignedClassWithStudentsClass> assignedClassWithStudentsClasses;
        //private List<StudentAccountModel>? selectedStudents;
        //private List<ClassModel>? alreadySelectedClasses;
        private ClassModel currentClass;
        private TestModel testModel;

        private IStudentRepository studentRepository;

        public ICommand AssignStudentsToTest { get; }

        public DetailedStudentsSelectionViewModel(TeacherViewViewModel tvvm, List<AssignedClassWithStudentsClass> assignedClassWithStudentsClasses/*List<StudentAccountModel>? selectedStudents, List<ClassModel>? alreadySelectedClasses*/, ClassModel currentClass, TestModel testModel)
        {
            teacherViewViewModel = tvvm;
            //this.selectedStudents = selectedStudents;
            //this.alreadySelectedClasses = alreadySelectedClasses;
            this.assignedClassWithStudentsClasses = assignedClassWithStudentsClasses;
            this.currentClass = currentClass;
            this.testModel = testModel;
            studentRepository = new StudentRepository();
            Students = studentRepository.GetStudentsAccountsByClassId(currentClass.Id);
            AssignStudentsToTest = new ViewModelCommand(ExecuteAssignStudentsToTest);
        }

        private void ExecuteAssignStudentsToTest(object obj)
        {
            ObservableCollection<StudentAccountModel> selectedStudentsHere = obj as ObservableCollection<StudentAccountModel>;
            if (selectedStudentsHere.Count == 0)
            {
                assignedClassWithStudentsClasses.Remove(assignedClassWithStudentsClasses.Where(o => o.AlreadySelectedClass == currentClass).First());
                teacherViewViewModel.ShowTestsAssignClassesView.Execute(new object[] { teacherViewViewModel, assignedClassWithStudentsClasses, testModel });
                return;
            }
            assignedClassWithStudentsClasses.Where(o => o.AlreadySelectedClass == currentClass).First().SelectedStudents.Clear();
            foreach (var st in selectedStudentsHere)
            {
                assignedClassWithStudentsClasses.Where(o => o.AlreadySelectedClass == currentClass).First().SelectedStudents.Add(st);
            }
            teacherViewViewModel.ShowTestsAssignClassesView.Execute(new object[] { teacherViewViewModel, assignedClassWithStudentsClasses, testModel });
        }


    }
}
