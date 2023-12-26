using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Noutecon__Exam_.Model;
using Noutecon__Exam_.Repositories;

namespace Noutecon__Exam_.ViewModel
{
    class MainViewViewModel : ViewModelBase
    {
        private StudentAccountModel currentStudentAccount;
        private IStudentRepository studentRepository;

        public StudentAccountModel CurrentStudentAccount { get => currentStudentAccount; set { currentStudentAccount = value; OnPropertyChanged(nameof(CurrentStudentAccount)); } }

        public MainViewViewModel()
        {
            studentRepository = new StudentRepository();
            CurrentStudentAccount = new StudentAccountModel();
            LoadCurrentUserData();
        }

        private void LoadCurrentUserData()
        {
            var student = studentRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if(student != null)
            {
                CurrentStudentAccount.Username = student.Username;
                CurrentStudentAccount.FirstName = student.FirstName;
                CurrentStudentAccount.LastName = student.LastName;             
            }
            else
            {
                MessageBox.Show("Invalid student, not logged in!");
                Application.Current.Shutdown();
            }
        }
    }
}
