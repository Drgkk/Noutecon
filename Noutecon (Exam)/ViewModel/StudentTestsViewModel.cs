using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
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
    public class StudentTestsViewModel : ViewModelBase
    {
        private ObservableCollection<StudentTestViewToShowInList> tests;

        public ObservableCollection<StudentTestViewToShowInList> Tests
        {
            get { return tests; }
            set { tests = value; OnPropertyChanged(nameof(Tests)); }
        }

        private StudentTestViewToShowInList selectedTest;

        public StudentTestViewToShowInList SelectedTest
        {
            get { return selectedTest; }
            set { selectedTest = value; OnPropertyChanged(nameof(SelectedTest)); OnSelectedTestChanged(); }
        }

        private string testName;

        public string TestName
        {
            get { return testName; }
            set { testName = value; OnPropertyChanged(nameof(TestName)); OnTestNameChanged(); }
        }



        private string testCategory;

        public string TestCategory
        {
            get { return testCategory; }
            set { testCategory = value; OnPropertyChanged(nameof(TestCategory)); OnCategoryChanged(); }
        }

        private MainViewViewModel mainViewViewModel;
        private ITestRepository testRepository;
        public ICommand ShowTestCreationView { get; }
        public ICommand ClearSearch { get; }
        public StudentTestsViewModel(MainViewViewModel mvvm)
        {
            mainViewViewModel = mvvm;
            testRepository = new TestRepository();
            Tests = new ObservableCollection<StudentTestViewToShowInList>();
            ClearSearch = new ViewModelCommand(ExecuteClearSearch);
            foreach (var test in testRepository.GetTestsByStudentId(mainViewViewModel.CurrentStudentAccount.Id))
            {
                Tests.Add(new StudentTestViewToShowInList(mainViewViewModel.CurrentStudentAccount) { Test = test });
            }
        }

        private void ExecuteClearSearch(object obj)
        {
            Tests.Clear();
            foreach (var test in testRepository.GetTestsByStudentId(mainViewViewModel.CurrentStudentAccount.Id))
            {
                Tests.Add(new StudentTestViewToShowInList(mainViewViewModel.CurrentStudentAccount) { Test = test });
            }
            TestName = "";
            TestCategory = "";
        }

        private void OnTestNameChanged()
        {
            ObservableCollection<StudentTestViewToShowInList> testCollectionHelper = new ObservableCollection<StudentTestViewToShowInList>();
            foreach (StudentTestViewToShowInList test in Tests)
            {
                if (test.Test.Name.ToLower().Contains(TestName.ToLower()))
                {
                    testCollectionHelper.Add(test);
                }
            }
            Tests = testCollectionHelper;
        }

        private void OnCategoryChanged()
        {
            ObservableCollection<StudentTestViewToShowInList> testCollectionHelper = new ObservableCollection<StudentTestViewToShowInList>();
            foreach (var test in Tests)
            {
                if (test.Test.Category.ToLower().Contains(TestCategory.ToLower()))
                {
                    testCollectionHelper.Add(test);
                }
            }
            Tests = testCollectionHelper;
        }

        private void OnSelectedTestChanged()
        {
            if(SelectedTest == null)
            {
                return;
            }    
            if(!CanExecuteCompleteTest(SelectedTest))
            {
                SelectedTest = null;
                return;
            }
            mainViewViewModel.ShowTestCompletionView.Execute(SelectedTest.Test);
            mainViewViewModel.Label = $"{SelectedTest.Test.Name}";
        }

        private bool CanExecuteCompleteTest(StudentTestViewToShowInList selectedTest)
        {
            if(selectedTest.StudentNumberOfTries < selectedTest.Test.NumberOfTries)
            {
                return true;
            }
            return false;
        }
    }


    public class StudentTestViewToShowInList
    {
        private TestModel test;

        public TestModel Test
        {
            get { return test; }
            set { test = value; OnTestChanged();  }
        }

        public int StudentNumberOfTries { get; set; }
        public double Result { get; set; }

        private SeriesCollection seriesCollection;

        public SeriesCollection SeriesCollection
        {
            get { return seriesCollection; }
            set { seriesCollection = value; }
        }

        private TeacherAccountModel testTeacher;

        public TeacherAccountModel TestTeacher
        {
            get { return testTeacher; }
            set { testTeacher = value; }
        }

        private Visibility isGradeVisible;

        public Visibility IsGradeVisible
        {
            get { return isGradeVisible; }
            set { isGradeVisible = value; }
        }



        private ITestRepository testRepository;
        private StudentAccountModel student;
        

        public StudentTestViewToShowInList(StudentAccountModel student)
        {
            this.student = student;
            testRepository = new TestRepository();
            
        }

        private void OnTestChanged()
        {
            (StudentNumberOfTries, Result) = testRepository.GetStudentTriesAndResult(student.Id, Test.Id);
            if(StudentNumberOfTries == 0 )
            {
                IsGradeVisible = Visibility.Collapsed;
            }
            else
            {
                IsGradeVisible = Visibility.Visible;
            }
            SeriesCollection = new SeriesCollection()
            {
                new PieSeries
                {
                    Title = "Right Answers",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(Math.Round(Result, 2)) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Wrong Answers",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(Math.Round(100 - Result, 2)) },
                    DataLabels = true
                }
            };
            TestTeacher = testRepository.GetTestTeacher(Test.Id);
        }

    }

}
