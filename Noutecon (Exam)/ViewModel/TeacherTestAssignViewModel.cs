using Noutecon__Exam_.Model;
using Noutecon__Exam_.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
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
        private TestModel? testModelToEdit;

        private IClassRepository classRepository;
        private IStudentRepository studentRepository;
        private ITestRepository testRepository;

        public ICommand AssignStudentsToTest { get; }
        public ICommand DetailedAssignStudents { get; }

        public TeacherTestAssignViewModel(TeacherViewViewModel tvvm, List<AssignedClassWithStudentsClass> assignedClassWithStudentsClasses, TestModel testModel, TestModel? testModelToEdit/* List<StudentAccountModel>? selectedStudents, List<ClassModel>? alreadySelectedClasses*/)
        {
            teacherViewViewModel = tvvm;
            /*this.selectedStudents = selectedStudents;
            this.alreadySelectedClasses = alreadySelectedClasses;*/
            this.assignedClassWithStudentsClasses = assignedClassWithStudentsClasses;
            this.testModel = testModel;
            this.testModelToEdit = testModelToEdit;
            Classes = new ObservableCollection<ClassAssignViewModel>();
            classRepository = new ClassRepository();
            studentRepository = new StudentRepository();
            testRepository = new TestRepository();
            AssignStudentsToTest = new ViewModelCommand(ExecuteAssignStudentsToTest, CanExecuteAssignStudentsToTest);
            DetailedAssignStudents = new ViewModelCommand(ExecuteDetailedAssignStudents, CanExecuteDetailedAssignStudents);
            ObservableCollection<ClassModel> tempClasses = classRepository.GetClassesByTeacherId(teacherViewViewModel.CurrentTeacher.Id);
            if(testModelToEdit == null)
            {
                foreach (ClassModel tempClass in tempClasses)
                {
                    bool isCheckedh = false;
                    if (assignedClassWithStudentsClasses != null && AssignedClassWithStudentsClassesContainsClass(assignedClassWithStudentsClasses, tempClass))
                    {
                        isCheckedh = true;
                    }
                    Classes.Add(new ClassAssignViewModel { ClassToAssign = tempClass, IsSelected = isCheckedh });
                }
            }
            else
            {
                if(assignedClassWithStudentsClasses == null)
                {
                    foreach (ClassModel tempClass in tempClasses)
                    {
                        List<StudentAccountModel> studentsInTempClass = studentRepository.GetStudentsAccountsByClassId(tempClass.Id).ToList();
                        foreach (StudentAccountModel st in testModelToEdit.Students)
                        {
                            if (studentsInTempClass.Any(o => o.Username == st.Username))
                            {
                                if (assignedClassWithStudentsClasses != null && AssignedClassWithStudentsClassesContainsClass(assignedClassWithStudentsClasses, tempClass))
                                {
                                    assignedClassWithStudentsClasses.Where(o => o.AlreadySelectedClass.UniqueId == tempClass.UniqueId).First().SelectedStudents.Add(st);
                                }
                                else
                                {
                                    if (assignedClassWithStudentsClasses == null)
                                    {
                                        assignedClassWithStudentsClasses = new List<AssignedClassWithStudentsClass>();
                                    }
                                    assignedClassWithStudentsClasses.Add(new AssignedClassWithStudentsClass() { AlreadySelectedClass = tempClass, SelectedStudents = new List<StudentAccountModel>() });
                                    assignedClassWithStudentsClasses.Where(o => o.AlreadySelectedClass.UniqueId == tempClass.UniqueId).First().SelectedStudents.Add(st);
                                }
                            }
                        }
                    }
                }
                
                foreach (ClassModel tempClass in tempClasses)
                {
                    bool isCheckedh = false;
                    if (assignedClassWithStudentsClasses != null && AssignedClassWithStudentsClassesContainsClass(assignedClassWithStudentsClasses, tempClass))
                    {
                        isCheckedh = true;
                    }
                    Classes.Add(new ClassAssignViewModel { ClassToAssign = tempClass, IsSelected = isCheckedh });
                }
            }
        }

        private bool CanExecuteAssignStudentsToTest(object obj)
        {
            bool isValid = true;
            System.Collections.IList items = (System.Collections.IList)obj;
            ObservableCollection<ClassAssignViewModel> selectedClasses = new ObservableCollection<ClassAssignViewModel>(items.Cast<ClassAssignViewModel>());
            if (assignedClassWithStudentsClasses == null && selectedClasses.Count == 0)
            {
                isValid = false;
            }
            return isValid;
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
            
            teacherViewViewModel.ShowDetailedStudentsSelectionView.Execute(new object[] { teacherViewViewModel, assignedClassWithStudentsClasses, selectedClass.ClassToAssign, testModel, testModelToEdit });
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
                if(assignedClassWithStudentsClasses == null || (assignedClassWithStudentsClasses != null && !AssignedClassWithStudentsClassesContainsClass(assignedClassWithStudentsClasses, cm.ClassToAssign)))
                {
                    foreach(StudentAccountModel sam in studentRepository.GetStudentsAccountsByClassId(cm.ClassToAssign.Id))
                    {
                        testModel.Students.Add(sam);
                    }
                }
            }
            testModel.Students = testModel.Students.Distinct(new StudentDistinctComparer()).ToList();

            if(testModelToEdit == null)
            {
                int testId = testRepository.Add(testModel);
                string testDirPath = $"{System.AppDomain.CurrentDomain.BaseDirectory}\\Tests\\{testModel.Name}_{testId}";
                if (!Directory.Exists(testDirPath))
                {
                    System.IO.Directory.CreateDirectory(testDirPath);
                }
                int i = 0;
                foreach(QuestionModel qm in testModel.Questions)
                {
                    string questionDirPath = $"{testDirPath}\\Question{i}";
                    if(!Directory.Exists(questionDirPath))
                    {
                        System.IO.Directory.CreateDirectory(questionDirPath);
                    }

                    if (testModel.Questions[i].ImagePath != "/Images/NoImageIcon.png")
                    {
                        string imageFilePath = $"{questionDirPath}\\Image.jpg";
                        if (!File.Exists(imageFilePath))
                        {
                            using (File.Create(imageFilePath)) { }
                        }
                        else
                        {
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                        }
                        if(testModel.Questions[i].ImagePath != imageFilePath)
                        {
                            System.IO.File.Copy(testModel.Questions[i].ImagePath, imageFilePath, true);
                        }
                        
                        testModel.Questions[i].ImagePath = imageFilePath;
                    }
                    

                    if (testModel.Questions[i].AudioPath != null)
                    {
                        string audioFilePath = $"{questionDirPath}\\Audio.mp3";
                        if (!File.Exists(audioFilePath))
                        {
                            using (File.Create(audioFilePath)) { }
                        }
                        else
                        {
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                        }
                        if(testModel.Questions[i].AudioPath != audioFilePath)
                        {
                            System.IO.File.Copy(testModel.Questions[i].AudioPath, audioFilePath, true);
                        }
                        
                        testModel.Questions[i].AudioPath = audioFilePath;
                    }

                    

                    testRepository.EditImageAndAudioById(testModel.Questions[i].ImagePath, testModel.Questions[i].AudioPath, testId, i);

                    i++;
                }
                
                
            }
            else
            {
                testModelToEdit.Students = testModel.Students;
                
                if(testModel.Name != testRepository.GetNameById(testModelToEdit.Id))
                {
                    string testDirPath = $"{System.AppDomain.CurrentDomain.BaseDirectory}\\Tests\\{testModel.Name}_{testModelToEdit.Id}";
                    if (!Directory.Exists(testDirPath))
                    {
                        System.IO.Directory.CreateDirectory(testDirPath);
                    }

                    int i = 0;
                    foreach (QuestionModel qm in testModel.Questions)
                    {
                        string questionDirPath = $"{testDirPath}\\Question{i}";
                        if (!Directory.Exists(questionDirPath))
                        {
                            System.IO.Directory.CreateDirectory(questionDirPath);
                        }
                        if (testModel.Questions[i].ImagePath != "/Images/NoImageIcon.png")
                        {
                            string imageFilePath = $"{questionDirPath}\\Image.jpg";
                            if (!File.Exists(imageFilePath))
                            {
                                using (File.Create(imageFilePath)) { }
                            }
                            else
                            {
                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                            }
                            if (testModel.Questions[i].ImagePath != imageFilePath)
                            {
                                System.IO.File.Copy(testModel.Questions[i].ImagePath, imageFilePath, true);
                            }

                            testModelToEdit.Questions[i].ImagePath = imageFilePath;
                        }


                        if (testModel.Questions[i].AudioPath != null)
                        {
                            string audioFilePath = $"{questionDirPath}\\Audio.mp3";
                            if (!File.Exists(audioFilePath))
                            {
                                using (File.Create(audioFilePath)) { }
                            }
                            else
                            {
                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                            }
                            if (testModel.Questions[i].AudioPath != audioFilePath)
                            {
                                System.IO.File.Copy(testModel.Questions[i].AudioPath, audioFilePath, true);
                            }

                            testModelToEdit.Questions[i].AudioPath = audioFilePath;
                        }



                        i++;
                    }
                    if (Directory.Exists($"{System.AppDomain.CurrentDomain.BaseDirectory}\\Tests\\{testRepository.GetNameById(testModelToEdit.Id)}_{testModelToEdit.Id}"))
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        Directory.Delete($"{System.AppDomain.CurrentDomain.BaseDirectory}\\Tests\\{testRepository.GetNameById(testModelToEdit.Id)}_{testModelToEdit.Id}", true);
                    }
                }
                else
                {
                    string testDirPath = $"{System.AppDomain.CurrentDomain.BaseDirectory}\\Tests\\{testModel.Name}Helper_{testModelToEdit.Id}";
                    if (!Directory.Exists(testDirPath))
                    {
                        System.IO.Directory.CreateDirectory(testDirPath);
                    }

                    int i = 0;
                    foreach (QuestionModel qm in testModel.Questions)
                    {
                        string questionDirPath = $"{testDirPath}\\Question{i}";
                        if (!Directory.Exists(questionDirPath))
                        {
                            System.IO.Directory.CreateDirectory(questionDirPath);
                        }
                        if (testModel.Questions[i].ImagePath != "/Images/NoImageIcon.png")
                        {
                            string imageFilePath = $"{questionDirPath}\\Image.jpg";
                            if (!File.Exists(imageFilePath))
                            {
                                using (File.Create(imageFilePath)) { }
                            }
                            else
                            {
                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                            }
                            if (testModel.Questions[i].ImagePath != imageFilePath)
                            {
                                System.IO.File.Copy(testModel.Questions[i].ImagePath, imageFilePath, true);
                            }

                            testModelToEdit.Questions[i].ImagePath = imageFilePath;
                        }


                        if (testModel.Questions[i].AudioPath != null)
                        {
                            string audioFilePath = $"{questionDirPath}\\Audio.mp3";
                            if (!File.Exists(audioFilePath))
                            {
                                using (File.Create(audioFilePath)) { }
                            }
                            else
                            {
                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                            }
                            if (testModel.Questions[i].AudioPath != audioFilePath)
                            {
                                System.IO.File.Copy(testModel.Questions[i].AudioPath, audioFilePath, true);
                            }

                            testModelToEdit.Questions[i].AudioPath = audioFilePath;
                        }
                        i++;
                    }

                        if (Directory.Exists($"{System.AppDomain.CurrentDomain.BaseDirectory}\\Tests\\{testModel.Name}_{testModelToEdit.Id}"))
                        {
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            Directory.Delete($"{System.AppDomain.CurrentDomain.BaseDirectory}\\Tests\\{testModel.Name}_{testModelToEdit.Id}", true);
                        }

                        testDirPath = $"{System.AppDomain.CurrentDomain.BaseDirectory}\\Tests\\{testModel.Name}_{testModelToEdit.Id}";
                        if (!Directory.Exists(testDirPath))
                        {
                            System.IO.Directory.CreateDirectory(testDirPath);
                        }

                        i = 0;
                        foreach (QuestionModel qm in testModel.Questions)
                        {
                            string questionDirPath = $"{testDirPath}\\Question{i}";
                            if (!Directory.Exists(questionDirPath))
                            {
                                System.IO.Directory.CreateDirectory(questionDirPath);
                            }
                            if (testModel.Questions[i].ImagePath != "/Images/NoImageIcon.png")
                            {
                                string imageFilePath = $"{questionDirPath}\\Image.jpg";
                                if (!File.Exists(imageFilePath))
                                {
                                    using (File.Create(imageFilePath)) { }
                                }
                                else
                                {
                                    GC.Collect();
                                    GC.WaitForPendingFinalizers();
                                }
                                if (testModel.Questions[i].ImagePath != imageFilePath)
                                {
                                    System.IO.File.Copy(testModel.Questions[i].ImagePath, imageFilePath, true);
                                }

                                testModelToEdit.Questions[i].ImagePath = imageFilePath;
                            }


                            if (testModel.Questions[i].AudioPath != null)
                            {
                                string audioFilePath = $"{questionDirPath}\\Audio.mp3";
                                if (!File.Exists(audioFilePath))
                                {
                                    using (File.Create(audioFilePath)) { }
                                }
                                else
                                {
                                    GC.Collect();
                                    GC.WaitForPendingFinalizers();
                                }
                                if (testModel.Questions[i].AudioPath != audioFilePath)
                                {
                                    System.IO.File.Copy(testModel.Questions[i].AudioPath, audioFilePath, true);
                                }

                                testModelToEdit.Questions[i].AudioPath = audioFilePath;
                            }

                            i++;
                        }
                        if (Directory.Exists($"{System.AppDomain.CurrentDomain.BaseDirectory}\\Tests\\{testModel.Name}Helper_{testModelToEdit.Id}"))
                        {
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            Directory.Delete($"{System.AppDomain.CurrentDomain.BaseDirectory}\\Tests\\{testModel.Name}Helper_{testModelToEdit.Id}", true);
                        }
                    
                }
                
                testRepository.Edit(testModelToEdit.Id, testModelToEdit);
            }
            teacherViewViewModel.ShowTestsView.Execute(obj);

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

    public class StudentDistinctComparer : IEqualityComparer<StudentAccountModel>
    {
        public bool Equals(StudentAccountModel? x, StudentAccountModel? y)
        {
            return x.Username == y.Username;
        }

        public int GetHashCode([DisallowNull] StudentAccountModel obj)
        {
            return obj.Username.GetHashCode();
        }
    }

}
