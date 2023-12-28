using Noutecon__Exam_.Model;
using Noutecon__Exam_.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Noutecon__Exam_.ViewModel
{
    public class ClassRegisterViewModel : ViewModelBase
    {
       // private string _uniqueId;
        private string _name;
        private int _grade;
        private string _errorMessage;

        //public string UniqueId { get => _uniqueId; set { _uniqueId = value; OnPropertyChanged(nameof(UniqueId)); } }
        public string Name { get => _name; set { _name = value; OnPropertyChanged(nameof(Name)); } }
        public int Grade { get => _grade; set { _grade = value; OnPropertyChanged(nameof(Grade)); } }
        public string ErrorMessage { get => _errorMessage; set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); } }

        private TeacherViewViewModel teacherViewViewModel;
        private IClassRepository classRepository;
        public ICommand RegisterClass { get; }
        public ICommand CancelRegistration { get; }
        

        public ClassRegisterViewModel(TeacherViewViewModel tvvm)
        {
            teacherViewViewModel = tvvm;
            classRepository = new ClassRepository();
            RegisterClass = new ViewModelCommand(ExecuteRegisterClass, CanExecuteRegisterClass);
            CancelRegistration = new ViewModelCommand(ExecuteCancelRegistration);
        }

        private bool CanExecuteRegisterClass(object obj)
        {
            bool isValid = true;
            if(string.IsNullOrEmpty(Name) || Name.Length <= 3 || Grade == null)
            {
                isValid = false;
            }
            return isValid;
        }

        private void ExecuteCancelRegistration(object obj)
        {
            teacherViewViewModel.ShowClassesView.Execute(obj);
        }

        private void ExecuteRegisterClass(object obj)
        {
            ClassCodeGenerator classCodeGenerator = new ClassCodeGenerator();

            ClassModel classModel = new ClassModel()
            {
                UniqueId = classCodeGenerator.GenerateCode(),
                Name = this.Name,
                Grade = this.Grade,
                CuratorId = teacherViewViewModel.CurrentTeacher.Id
            };
            classRepository.Add(classModel);
            ExecuteCancelRegistration(null);
        }
    }
}
