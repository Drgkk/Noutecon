using Noutecon__Exam_.Model;
using Noutecon__Exam_.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Noutecon__Exam_.ViewModel
{
    class TeacherViewViewModel : ViewModelBase
    {
        private TeacherAccountModel currentTeacher;
        private ITeacherRepository teacherRepository;

        public TeacherAccountModel CurrentTeacher { get => currentTeacher; set { currentTeacher = value; OnPropertyChanged(nameof(CurrentTeacher)); } }

        public TeacherViewViewModel()
        {
            CurrentTeacher = new TeacherAccountModel();
            teacherRepository = new TeacherRepository();
            LoadCurrentTeacherData();
        }

        

        private void LoadCurrentTeacherData()
        {
            var teacher = teacherRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if(teacher != null)
            {
                CurrentTeacher = new TeacherAccountModel()
                {
                    Username = teacher.Username,
                    FirstName = teacher.FirstName,
                    LastName = teacher.LastName,
                };
            }
            else
            {
                MessageBox.Show("Invalid teacher, not logged in!");
                Application.Current.Shutdown();
            }
        }


    }
}
