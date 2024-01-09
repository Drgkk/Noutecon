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
    public class TeacherTestAssignViewModel : ViewModelBase
    {

        private ObservableCollection<ClassAssignViewModel> classes;

        public ObservableCollection<ClassAssignViewModel> Classes
        {
            get { return classes; }
            set { classes = value; OnPropertyChanged(nameof(Classes)); }
        }




        private TeacherViewViewModel teacherViewViewModel;
        /*private List<StudentAccountModel>? selectedStudents;
        private List<ClassModel>? alreadySelectedClasses;*/
        private List<AssignedClassWithStudentsClass> assignedClassWithStudentsClasses;
        private TestModel testModel;

        private IClassRepository classRepository;
        private IStudentRepository studentRepository;

        public ICommand AssignStudentsToTest { get; }
        public ICommand DetailedAssignStudents { get; }

        public TeacherTestAssignViewModel(TeacherViewViewModel tvvm, List<AssignedClassWithStudentsClass> assignedClassWithStudentsClasses, TestModel testModel/* List<StudentAccountModel>? selectedStudents, List<ClassModel>? alreadySelectedClasses*/)
        {
            teacherViewViewModel = tvvm;
            /*this.selectedStudents = selectedStudents;
            this.alreadySelectedClasses = alreadySelectedClasses;*/
            this.assignedClassWithStudentsClasses = assignedClassWithStudentsClasses;
            this.testModel = testModel;
            Classes = new ObservableCollection<ClassAssignViewModel>();
            classRepository = new ClassRepository();
            studentRepository = new StudentRepository();
            AssignStudentsToTest = new ViewModelCommand(ExecuteAssignStudentsToTest);
            DetailedAssignStudents = new ViewModelCommand(ExecuteDetailedAssignStudents, CanExecuteDetailedAssignStudents);
            ObservableCollection<ClassModel> tempClasses = classRepository.GetClassesByTeacherId(teacherViewViewModel.CurrentTeacher.Id);
            foreach(ClassModel tempClass in tempClasses )
            {
                bool isCheckedh = false;
                if(assignedClassWithStudentsClasses != null && AssignedClassWithStudentsClassesContainsClass(assignedClassWithStudentsClasses, tempClass))
                {
                    isCheckedh = true;
                }
                Classes.Add(new ClassAssignViewModel { ClassToAssign = tempClass, IsSelected = isCheckedh});
            }
        }

        private bool CanExecuteDetailedAssignStudents(object obj)
        {
            object[] ar = obj as object[];
            ClassAssignViewModel selectedClass = ar[0] as ClassAssignViewModel;
            System.Collections.IList items = (System.Collections.IList)ar[1];
            ObservableCollection<ClassAssignViewModel> selectedClasses = new ObservableCollection<ClassAssignViewModel>(items.Cast<ClassAssignViewModel>());
            bool isValid = true;
            if((selectedClasses == null || selectedClasses.Count == 0) || !ClassAssignViewModelContainsClass(selectedClasses.ToList(), selectedClass))
            {
                isValid = false;
            }
            return isValid;
        }

        private void ExecuteDetailedAssignStudents(object obj)
        {
            object[] ar = obj as object[];
            ClassAssignViewModel selectedClass = ar[0] as ClassAssignViewModel;
            System.Collections.IList items = (System.Collections.IList)ar[1];
            ObservableCollection<ClassAssignViewModel> selectedClasses = new ObservableCollection<ClassAssignViewModel>(items.Cast<ClassAssignViewModel>());
            if(assignedClassWithStudentsClasses == null)
            {
                assignedClassWithStudentsClasses = new List<AssignedClassWithStudentsClass>();
            }
            foreach (ClassAssignViewModel cm in selectedClasses)
            {
                assignedClassWithStudentsClasses.Add(new AssignedClassWithStudentsClass() { AlreadySelectedClass = cm.ClassToAssign, SelectedStudents = new List<StudentAccountModel>() });
            }
            
            teacherViewViewModel.ShowDetailedStudentsSelectionView.Execute(new object[] { teacherViewViewModel, assignedClassWithStudentsClasses, selectedClass.ClassToAssign, testModel });
        }

        private void ExecuteAssignStudentsToTest(object obj)
        {
            ObservableCollection<ClassModel> selectedClasses = obj as ObservableCollection<ClassModel>;
            if (assignedClassWithStudentsClasses != null)
            {
                foreach (AssignedClassWithStudentsClass acwsc in assignedClassWithStudentsClasses)
                {
                    foreach (StudentAccountModel sam in acwsc.SelectedStudents)
                    {
                        testModel.Students.Add(sam);
                    }
                }
            }
            foreach(ClassModel cm in selectedClasses)
            {
                if(!AssignedClassWithStudentsClassesContainsClass(assignedClassWithStudentsClasses, cm))
                {
                    foreach(StudentAccountModel sam in studentRepository.GetStudentsAccountsByClassId(cm.Id))
                    {
                        testModel.Students.Add(sam);
                    }
                }
            }
        }

        private bool AssignedClassWithStudentsClassesContainsClass(List<AssignedClassWithStudentsClass> ascwsclist, ClassModel classContainsOrNo)
        {
            foreach(AssignedClassWithStudentsClass ascwsc in ascwsclist)
            {
                if(ascwsc.AlreadySelectedClass == classContainsOrNo)
                {
                    return true;
                }
            }
            return false;
        }
        private bool ClassAssignViewModelContainsClass(List<ClassAssignViewModel> cavmlist, ClassAssignViewModel classContainsOrNo)
        {
            foreach (ClassAssignViewModel cavm in cavmlist)
            {
                if (cavm.ClassToAssign == classContainsOrNo.ClassToAssign)
                {
                    return true;
                }
            }
            return false;
        }

    }

    public class ClassAssignViewModel : ViewModelBase
    {
        public ClassModel ClassToAssign { get; set; }

        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; OnPropertyChanged(nameof(IsSelected)); }
        }


        public ClassAssignViewModel()
        {

        }
    }

}
