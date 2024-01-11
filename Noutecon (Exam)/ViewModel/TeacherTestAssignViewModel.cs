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
        private ITestRepository testRepository;

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
            testRepository = new TestRepository();
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
                if(!AssignedClassWithStudentsClassesContainsClass(assignedClassWithStudentsClasses, cm.ClassToAssign))
                {
                    assignedClassWithStudentsClasses.Add(new AssignedClassWithStudentsClass() { AlreadySelectedClass = cm.ClassToAssign, SelectedStudents = new List<StudentAccountModel>() });
                }
                
            }
            
            teacherViewViewModel.ShowDetailedStudentsSelectionView.Execute(new object[] { teacherViewViewModel, assignedClassWithStudentsClasses, selectedClass.ClassToAssign, testModel });
        }

        private void ExecuteAssignStudentsToTest(object obj)
        {
            System.Collections.IList items = (System.Collections.IList)obj;
            ObservableCollection<ClassAssignViewModel> selectedClasses = new ObservableCollection<ClassAssignViewModel>(items.Cast<ClassAssignViewModel>());
            if (assignedClassWithStudentsClasses != null)
            {
                foreach (AssignedClassWithStudentsClass acwsc in assignedClassWithStudentsClasses)
                {
                    if(SelectedClassesContainAssignedClassWithStudentsClass(selectedClasses, acwsc))
                    {
                        
                        foreach (StudentAccountModel sam in acwsc.SelectedStudents)
                        {
                            testModel.Students.Add(sam);
                        }
                        if (acwsc.SelectedStudents.Count == 0)
                        {
                            foreach (StudentAccountModel sam in studentRepository.GetStudentsAccountsByClassId(acwsc.AlreadySelectedClass.Id))
                            {
                                testModel.Students.Add(sam);
                            }
                        }
                    }
                    else
                    {
                        foreach (StudentAccountModel sam in studentRepository.GetStudentsAccountsByClassId(acwsc.AlreadySelectedClass.Id))
                        {
                            testModel.Students.Add(sam);
                        }
                    }
                    
                }
            }
            foreach(ClassAssignViewModel cm in selectedClasses)
            {
                if(!AssignedClassWithStudentsClassesContainsClass(assignedClassWithStudentsClasses, cm.ClassToAssign))
                {
                    foreach(StudentAccountModel sam in studentRepository.GetStudentsAccountsByClassId(cm.ClassToAssign.Id))
                    {
                        testModel.Students.Add(sam);
                    }
                }
            }


            testRepository.Add(testModel);

        }

        private bool AssignedClassWithStudentsClassesContainsClass(List<AssignedClassWithStudentsClass> ascwsclist, ClassModel classContainsOrNo)
        {
            foreach(AssignedClassWithStudentsClass ascwsc in ascwsclist)
            {
                if(ascwsc.AlreadySelectedClass.UniqueId == classContainsOrNo.UniqueId)
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
                if (cavm.ClassToAssign.UniqueId == classContainsOrNo.ClassToAssign.UniqueId)
                {
                    return true;
                }
            }
            return false;
        }

        private bool SelectedClassesContainAssignedClassWithStudentsClass(ObservableCollection<ClassAssignViewModel> cavmlist, AssignedClassWithStudentsClass acwsc)
        {
            foreach (ClassAssignViewModel cavm in cavmlist)
            {
                if (cavm.ClassToAssign.UniqueId == acwsc.AlreadySelectedClass.UniqueId)
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
