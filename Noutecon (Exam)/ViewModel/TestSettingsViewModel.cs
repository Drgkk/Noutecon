using Noutecon__Exam_.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Noutecon__Exam_.ViewModel
{
    public class TestSettingsViewModel : ViewModelBase
    {

        private string testName;

        public string TestName
        {
            get { return testName; }
            set { testName = value; OnPropertyChanged(nameof(TestName)); }
        }

        private string testNameErrorMessage;

        public string TestNameErrorMessage
        {
            get { return testNameErrorMessage; }
            set { testNameErrorMessage = value; OnPropertyChanged(nameof(TestNameErrorMessage)); }
        }

        private string testNumOfTries;

        public string TestNumOfTries
        {
            get { return testNumOfTries; }
            set { testNumOfTries = value; OnPropertyChanged(nameof(TestNumOfTries)); }
        }



        private TeacherViewViewModel teacherViewViewModel;
        private TestModel? testModelToEdit;

        public ICommand CancelTestCreation { get; }
        public ICommand CreateNewTest { get; }

        public TestSettingsViewModel(TeacherViewViewModel tvvm, TestModel? testModelToEdit) 
        {
            teacherViewViewModel = tvvm;
            this.testModelToEdit = testModelToEdit;
            TestNumOfTries = "1";
            CancelTestCreation = new ViewModelCommand(ExecuteCancelTestCreation);
            CreateNewTest = new ViewModelCommand(ExecuteCreateNewTest, CanExecuteCreateNewTest);
            if(testModelToEdit != null)
            {
                TestName = testModelToEdit.Name;
                TestNumOfTries = testModelToEdit.NumberOfTries.ToString();
            }
        }

        private bool CanExecuteCreateNewTest(object obj)
        {
            bool isValid = true;
            if(string.IsNullOrEmpty(TestName) || TestName.Length < 3 || string.IsNullOrEmpty(TestNumOfTries) || TestNumOfTries == "0")
            {
                isValid = false;
            }
            return isValid;
        }

        private void ExecuteCreateNewTest(object obj)
        {
            TestModel testModel = new TestModel() { Name = TestName, NumberOfTries = int.Parse(TestNumOfTries) };
            if (testModelToEdit == null)
            {
                teacherViewViewModel.ShowTestsCreationView.Execute(new object[] { testModel, null});
                teacherViewViewModel.Caption = TestName;
            }
            else
            {
                testModelToEdit.Name = TestName;
                testModelToEdit.NumberOfTries = int.Parse(TestNumOfTries);
                teacherViewViewModel.ShowTestsCreationView.Execute(new object[] { testModel, testModelToEdit });
                teacherViewViewModel.Caption = TestName;
            }
        }

        private void ExecuteCancelTestCreation(object obj)
        {
            teacherViewViewModel.ShowTestsView.Execute(obj);
        }
    }
}
