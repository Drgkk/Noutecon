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
    public class StudentTestsViewModel : ViewModelBase
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



        private MainViewViewModel mainViewViewModel;
        private ITestRepository testRepository;
        public ICommand ShowTestCreationView { get; }
        public StudentTestsViewModel(MainViewViewModel mvvm)
        {
            mainViewViewModel = mvvm;
            testRepository = new TestRepository();
            Tests = testRepository.GetTestsByStudentId(mainViewViewModel.CurrentStudentAccount.Id);
        }

        private void OnSelectedTestChanged()
        {
            mainViewViewModel.ShowTestCompletionView.Execute(SelectedTest);
            mainViewViewModel.Label = $"{SelectedTest.Name}";
        }

    }
}
