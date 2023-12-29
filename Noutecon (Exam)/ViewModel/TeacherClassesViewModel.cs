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
    public class TeacherClassesViewModel : ViewModelBase
    {

        private TeacherViewViewModel teacherViewViewModel;
        private ClassModel _selectedClass;

        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set
            { 
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                if(SearchText != null )
                    ExecuteSearch(null);
                else
                    Classes = AllClasses;
            }
        }


        public ClassModel SelectedClass
        {
            get { return _selectedClass; }
            set { _selectedClass = value; OnPropertyChanged(nameof(SelectedClass)); if (SelectedClass != null) { ExecuteShowClassDetailsView(null); } }
        }

        private ObservableCollection<ClassModel> _classes;

        public ObservableCollection<ClassModel> Classes
        {
            get { return _classes; }
            set { _classes = value; OnPropertyChanged(nameof(Classes)); }
        }

        private ObservableCollection<ClassModel> AllClasses;
        private IClassRepository classRepository;
        public ICommand ShowClassRegisterView { get; }
        public ICommand Refresh { get; }

        public TeacherClassesViewModel(TeacherViewViewModel tvvm) 
        {
            teacherViewViewModel = tvvm;
            ShowClassRegisterView = new ViewModelCommand(ExecuteShowClassRegisterView);
            classRepository = new ClassRepository();
            AllClasses = classRepository.GetClassesByTeacherId(teacherViewViewModel.CurrentTeacher.Id);
            Classes = AllClasses;
            Refresh = new ViewModelCommand(ExecuteRefresh);
        }

        private void ExecuteRefresh(object obj)
        {
            SearchText = "";
            AllClasses = classRepository.GetClassesByTeacherId(teacherViewViewModel.CurrentTeacher.Id);
            Classes = AllClasses;
        }

        private void ExecuteSearch(object obj)
        {
            SelectedClass = null;
            Classes = new ObservableCollection<ClassModel>(AllClasses.Where(o => o.Name.ToLower().Contains(SearchText.ToLower())));
        }

        private void ExecuteShowClassDetailsView(object obj)
        {
            teacherViewViewModel.ShowDetailedClassView.Execute(SelectedClass);
        }

        private void ExecuteShowClassRegisterView(object obj)
        {
            teacherViewViewModel.ShowClassesRegisterView.Execute(obj);
        }
    }
}
