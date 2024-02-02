using Noutecon__Exam_.Model;
using Noutecon__Exam_.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Noutecon__Exam_.ViewModel
{
    public class TeacherTestsViewModel : ViewModelBase
    {

        private ObservableCollection<TestModel> tests;

        public ObservableCollection<TestModel> Tests
        {
            get { return tests; }
            set { tests = value; OnPropertyChanged(nameof(Tests)); }
        }

        private TestModel selectedTest;

        public TestModel SelectedTest
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

        

        private TeacherViewViewModel teacherViewViewModel;
        private ITestRepository testRepository;
        public ICommand ShowTestCreationView { get; }
        public ICommand ClearSearch { get; }
        public ICommand DeleteTest { get; }
        public TeacherTestsViewModel(TeacherViewViewModel tvvm)
        {
            teacherViewViewModel = tvvm;
            ShowTestCreationView = new ViewModelCommand(ExecuteShowTestCreationView);
            ClearSearch = new ViewModelCommand(ExecuteClearSearch);
            DeleteTest = new ViewModelCommand(ExecuteDeleteTest);
            testRepository = new TestRepository();
            Tests = testRepository.GetTestsByTeacherId(teacherViewViewModel.CurrentTeacher.Id);
        }

        private void ExecuteDeleteTest(object obj)
        {
            TestModel tm = (TestModel)obj;
            testRepository.RemoveTestById(tm.Id);
            Tests = testRepository.GetTestsByTeacherId(teacherViewViewModel.CurrentTeacher.Id);
        }

        private void ExecuteClearSearch(object obj)
        {
            Tests = testRepository.GetTestsByTeacherId(teacherViewViewModel.CurrentTeacher.Id);
            TestName = "";
            TestCategory = "";
        }

        private void OnTestNameChanged()
        {
            ObservableCollection<TestModel> testCollectionHelper = new ObservableCollection<TestModel>();
            foreach (var test in Tests)
            {
                if(test.Name.ToLower().Contains(TestName.ToLower()))
                {
                    testCollectionHelper.Add(test);
                }
            }
            Tests = testCollectionHelper;
        }

        private void OnCategoryChanged()
        {
            ObservableCollection<TestModel> testCollectionHelper = new ObservableCollection<TestModel>();
            foreach (var test in Tests)
            {
                if (test.Category.ToLower().Contains(TestCategory.ToLower()))
                {
                    testCollectionHelper.Add(test);
                }
            }
            Tests = testCollectionHelper;
        }

        private void OnSelectedTestChanged()
        {
            teacherViewViewModel.ShowTestsSettingsView.Execute(SelectedTest);
        }

        private void ExecuteShowTestCreationView(object obj)
        {
            teacherViewViewModel.ShowTestsSettingsView.Execute(null);
        }
    }
}
