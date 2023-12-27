using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Noutecon__Exam_.ViewModel
{
    public class TeacherClassesViewModel : ViewModelBase
    {

        private TeacherViewViewModel teacherViewViewModel;
        public ICommand ShowClassRegisterView { get; }

        public TeacherClassesViewModel(TeacherViewViewModel tvvm) 
        {
            teacherViewViewModel = tvvm;
            ShowClassRegisterView = new ViewModelCommand(ExecuteShowClassRegisterView);
        }

        private void ExecuteShowClassRegisterView(object obj)
        {
            teacherViewViewModel.ShowClassesRegisterView.Execute(obj);
        }
    }
}
