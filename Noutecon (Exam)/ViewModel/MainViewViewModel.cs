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
            Thread validateStudentThread = new Thread(new ThreadStart(ThreadWorkValidateUser));
            validateStudentThread.IsBackground = true;
            validateStudentThread.Start();  
        }

        private void LoadCurrentUserData()
        {
            var student = studentRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if(student != null)
            {
                CurrentStudentAccount.Id = student.Id;
                CurrentStudentAccount.Username = student.Username;
                CurrentStudentAccount.FirstName = student.FirstName;
                CurrentStudentAccount.LastName = student.LastName;             
                CurrentStudentAccount.ClassId = student.ClassId;
                CurrentStudentAccount.ProfilePicturePath = student.ProfilePicturePath;
            }
            else
            {
                MessageBox.Show("Invalid student, not logged in!");
                Application.Current.Shutdown();
            }
        }

        private void ThreadWorkValidateUser()
        {
            while (true)
            {
                var student = studentRepository.GetByUsername(CurrentStudentAccount.Username);
                if (student == null)
                {
                    Application.Current.Shutdown();
                }
                Thread.Sleep(1000);
            }
        }

    }
}
