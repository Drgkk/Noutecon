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
    public class StudentDetailedTestsViewModel : ViewModelBase
    {
        private ObservableCollection<StudentTestViewToShowInList> tests;

        public ObservableCollection<StudentTestViewToShowInList> Tests
        {
            get { return tests; }
            set { tests = value; OnPropertyChanged(nameof(Tests)); }
        }

        private TeacherViewViewModel teacherViewViewModel;
        private StudentAccountModel currentStudent;
        private ClassModel classModel;
        private ITestRepository testRepository;

        public ICommand GoBack { get; }

        public StudentDetailedTestsViewModel(TeacherViewViewModel tvvm, StudentAccountModel currentStudent, ClassModel classModel)
        {
            this.teacherViewViewModel = tvvm;
            this.currentStudent = currentStudent;
            this.classModel = classModel;
            testRepository = new TestRepository();
            Tests = new ObservableCollection<StudentTestViewToShowInList>();
            GoBack = new ViewModelCommand(ExecuteGoBack);
            foreach (var test in testRepository.GetTestsByStudentId(currentStudent.Id))
            {
                Tests.Add(new StudentTestViewToShowInList(currentStudent) { Test = test });
            }
        }

        private void ExecuteGoBack(object obj)
        {
            teacherViewViewModel.ShowDetailedClassView.Execute(classModel);
        }
    }
}
