using Noutecon__Exam_.Model;
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


        private TeacherViewViewModel teacherViewViewModel;
        public ICommand ShowTestCreationView { get; }
        public TeacherTestsViewModel(TeacherViewViewModel tvvm)
        {
            teacherViewViewModel = tvvm;
            ShowTestCreationView = new ViewModelCommand(ExecuteShowTestCreationView);
        }

        private void ExecuteShowTestCreationView(object obj)
        {
            teacherViewViewModel.ShowTestsSettingsView.Execute(obj);
        }
    }
}
