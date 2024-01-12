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

        

        private TeacherViewViewModel teacherViewViewModel;
        private ITestRepository testRepository;
        public ICommand ShowTestCreationView { get; }
        public TeacherTestsViewModel(TeacherViewViewModel tvvm)
        {
            teacherViewViewModel = tvvm;
            ShowTestCreationView = new ViewModelCommand(ExecuteShowTestCreationView);
            testRepository = new TestRepository();
            Tests = testRepository.GetTestsByTeacherId(teacherViewViewModel.CurrentTeacher.Id);
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
