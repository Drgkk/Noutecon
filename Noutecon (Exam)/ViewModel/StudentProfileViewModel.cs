using Noutecon__Exam_.Model;
using Noutecon__Exam_.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Noutecon__Exam_.ViewModel
{
    public class StudentProfileViewModel : ViewModelBase
    {
        private StudentAccountModel currentStudent;

        public StudentAccountModel CurrentStudent
        {
            get { return currentStudent; }
            set { currentStudent = value; OnPropertyChanged(nameof(CurrentStudent)); }
        }

        private bool usernameTextBoxVisibility;

        public bool UsernameTextBoxVisibility
        {
            get { return usernameTextBoxVisibility; }
            set { usernameTextBoxVisibility = value; OnPropertyChanged(nameof(UsernameTextBoxVisibility)); UsernameTextBoxLostFocus(); }
        }

        private bool usernameTextVisibility;

        public bool UsernameTextVisibility
        {
            get { return usernameTextVisibility; }
            set { usernameTextVisibility = value; OnPropertyChanged(nameof(UsernameTextVisibility)); }
        }

        private string usernameErrorMessage;

        public string UsernameErrorMessage
        {
            get { return usernameErrorMessage; }
            set { usernameErrorMessage = value; OnPropertyChanged(nameof(UsernameErrorMessage)); }
        }

        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; OnPropertyChanged(nameof(Username)); }
        }

        //
        private bool firstnameTextBoxVisibility;

        public bool FirstnameTextBoxVisibility
        {
            get { return firstnameTextBoxVisibility; }
            set { firstnameTextBoxVisibility = value; OnPropertyChanged(nameof(FirstnameTextBoxVisibility)); FirstNameTextBoxLostFocus(); }
        }

        private bool firstNameTextVisibility;

        public bool FirstNameTextVisibility
        {
            get { return firstNameTextVisibility; }
            set { firstNameTextVisibility = value; OnPropertyChanged(nameof(FirstNameTextVisibility)); }
        }

        private string firstNameErrorMessage;

        public string FirstNameErrorMessage
        {
            get { return firstNameErrorMessage; }
            set { firstNameErrorMessage = value; OnPropertyChanged(nameof(FirstNameErrorMessage)); }
        }

        private string firstName;

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; OnPropertyChanged(nameof(FirstName)); }
        }

        //

        private bool lastnameTextBoxVisibility;

        public bool LastnameTextBoxVisibility
        {
            get { return lastnameTextBoxVisibility; }
            set { lastnameTextBoxVisibility = value; OnPropertyChanged(nameof(LastnameTextBoxVisibility)); LastNameTextBoxLostFocus(); }
        }

        private bool lastNameTextVisibility;

        public bool LastNameTextVisibility
        {
            get { return lastNameTextVisibility; }
            set { lastNameTextVisibility = value; OnPropertyChanged(nameof(LastNameTextVisibility)); }
        }

        private string lastNameErrorMessage;

        public string LastNameErrorMessage
        {
            get { return lastNameErrorMessage; }
            set { lastNameErrorMessage = value; OnPropertyChanged(nameof(LastNameErrorMessage)); }
        }

        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; OnPropertyChanged(nameof(LastName)); }
        }

        //
        private SecureString previousPassword;

        public SecureString PreviousPassword
        {
            get { return previousPassword; }
            set { previousPassword = value; OnPropertyChanged(nameof(PreviousPassword)); }
        }

        private SecureString newPassword;

        public SecureString NewPassword
        {
            get { return newPassword; }
            set { newPassword = value; OnPropertyChanged(nameof(NewPassword)); }
        }

        private string passwordErrorMessage;

        public string PasswordErrorMessage
        {
            get { return passwordErrorMessage; }
            set { passwordErrorMessage = value; OnPropertyChanged(nameof(PasswordErrorMessage)); }
        }

        private MainViewViewModel mainViewViewModel;
        private ITeacherRepository teacherRepository;
        private IStudentRepository studentRepository;

        public ICommand ChangePFP { get; }
        public ICommand ChangeUsername { get; }
        public ICommand ChangeUsernameVisibility { get; }

        public ICommand ChangeFirstName { get; }
        public ICommand ChangeFirstNameVisibility { get; }

        public ICommand ChangeLastName { get; }
        public ICommand ChangeLastNameVisibility { get; }

        public ICommand ChangePassword { get; }

        public StudentProfileViewModel(MainViewViewModel mvvm)
        {
            mainViewViewModel = mvvm;
            CurrentStudent = mainViewViewModel.CurrentStudentAccount;
            teacherRepository = new TeacherRepository();
            studentRepository = new StudentRepository();
            ChangePFP = new ViewModelCommand(ExecuteChangePFP);
            ChangeUsername = new ViewModelCommand(ExecuteChangeUsername);
            ChangeUsernameVisibility = new ViewModelCommand(ExecuteChangeUsernameVisibility);
            ChangeFirstName = new ViewModelCommand(ExecuteChangeFirstName);
            ChangeFirstNameVisibility = new ViewModelCommand(ExecuteChangeFirstNameVisibility);
            ChangeLastName = new ViewModelCommand(ExecuteChangeLastName);
            ChangeLastNameVisibility = new ViewModelCommand(ExecuteChangeLastNameVisibility);
            UsernameTextBoxVisibility = false;
            UsernameTextVisibility = true;
            FirstnameTextBoxVisibility = false;
            FirstNameTextVisibility = true;
            LastnameTextBoxVisibility = false;
            LastNameTextVisibility = true;
            ChangePassword = new ViewModelCommand(ExecuteChangePassword, CanExecuteChangePassword);
        }

        private bool CanExecuteChangePassword(object obj)
        {
            bool isValid = true;
            if (PreviousPassword == null || PreviousPassword.Length <= 3
                || NewPassword == null || NewPassword.Length <= 3)
            {
                isValid = false;
            }
            return isValid;
        }

        private void ExecuteChangePassword(object obj)
        {
            PasswordErrorMessage = "";
            if (new NetworkCredential("", PreviousPassword).Password != studentRepository.GetNetworkCredential(mainViewViewModel.CurrentStudentAccount.Id).Password)
            {
                PasswordErrorMessage = "Previous password does not match with your current password!";
                return;
            }
            studentRepository.EditPassword(mainViewViewModel.CurrentStudentAccount.Id, new System.Net.NetworkCredential("", newPassword));
            MessageBox.Show("Password has been succesfully changed!", "Password change success", MessageBoxButtons.OK);
        }

        private void LastNameTextBoxLostFocus()
        {
            if (lastnameTextBoxVisibility == false)
            {
                lastnameTextBoxVisibility = false;
                LastNameTextVisibility = true;
            }
        }
        private void ExecuteChangeLastNameVisibility(object obj)
        {
            LastNameErrorMessage = "";
            LastName = currentStudent.LastName;
            LastNameTextVisibility = false;
            LastnameTextBoxVisibility = true;
        }

        private void ExecuteChangeLastName(object obj)
        {
            LastNameTextVisibility = true;
            LastnameTextBoxVisibility = false;
            if (string.IsNullOrEmpty(LastName))
            {
                LastNameErrorMessage = "Cannot be null";
                return;
            }
            studentRepository.EditStudentLastName(currentStudent.Id, LastName);
            StudentAccountModel model = studentRepository.GetAccountById(mainViewViewModel.CurrentStudentAccount.Id);
            mainViewViewModel.CurrentStudentAccount = model;
            CurrentStudent = model;
        }

        private void FirstNameTextBoxLostFocus()
        {
            if (firstnameTextBoxVisibility == false)
            {
                firstnameTextBoxVisibility = false;
                FirstNameTextVisibility = true;
            }
        }
        private void ExecuteChangeFirstNameVisibility(object obj)
        {
            FirstNameErrorMessage = "";
            FirstName = currentStudent.FirstName;
            FirstNameTextVisibility = false;
            FirstnameTextBoxVisibility = true;
        }

        private void ExecuteChangeFirstName(object obj)
        {
            FirstNameTextVisibility = true;
            FirstnameTextBoxVisibility = false;
            if (string.IsNullOrEmpty(FirstName))
            {
                FirstNameErrorMessage = "Cannot be null";
                return;
            }
            studentRepository.EditStudentFirstName(currentStudent.Id, FirstName);
            StudentAccountModel model = studentRepository.GetAccountById(mainViewViewModel.CurrentStudentAccount.Id);
            mainViewViewModel.CurrentStudentAccount = model;
            CurrentStudent = model;
        }

        private void ExecuteChangeUsernameVisibility(object obj)
        {
            UsernameErrorMessage = "";
            Username = currentStudent.Username;
            UsernameTextVisibility = false;
            UsernameTextBoxVisibility = true;
        }

        private void UsernameTextBoxLostFocus()
        {
            if (usernameTextBoxVisibility == false)
            {
                usernameTextBoxVisibility = false;
                UsernameTextVisibility = true;
            }
        }

        private void ExecuteChangeUsername(object obj)
        {
            UsernameTextVisibility = true;
            UsernameTextBoxVisibility = false;
            if (teacherRepository.GetByUsername(Username) != null || studentRepository.GetByUsername(Username) != null)
            {
                UsernameErrorMessage = "Account with that username already exists!";
                return;
            }
            if (string.IsNullOrEmpty(Username))
            {
                UsernameErrorMessage = "Cannot be null";
                return;
            }
            if (MessageBox.Show($"Are you sure you want to change username: {CurrentStudent.Username} to {Username}?", "Change Username", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            string filePathOld = $"{System.AppDomain.CurrentDomain.BaseDirectory}ProfilePictures\\{CurrentStudent.Username}.jpg";
            string filePathNew = $"{System.AppDomain.CurrentDomain.BaseDirectory}ProfilePictures\\{Username}.jpg";
            if (File.Exists(filePathOld))
            {
                using (File.Create(filePathNew)) { }
                System.IO.File.Copy(filePathOld, filePathNew, true);
            }

            studentRepository.EditStudentUsername(currentStudent.Id, Username);
            studentRepository.EditPfpById(currentStudent.Id, filePathNew);
            StudentAccountModel model = studentRepository.GetAccountById(mainViewViewModel.CurrentStudentAccount.Id);
            mainViewViewModel.CurrentStudentAccount = model;
            CurrentStudent = model;
            //File.Delete(filePathOld);
        }

        private void ExecuteChangePFP(object obj)
        {
            System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = $"{System.AppDomain.CurrentDomain.BaseDirectory}ProfilePictures\\{CurrentStudent.Username}.jpg";
                using (File.Create(filePath)) { }
                System.IO.File.Copy(fileDialog.FileName, filePath, true);
                studentRepository.EditPfpById(mainViewViewModel.CurrentStudentAccount.Id, filePath);
                StudentAccountModel model = studentRepository.GetAccountById(mainViewViewModel.CurrentStudentAccount.Id);
                mainViewViewModel.CurrentStudentAccount = model;
                CurrentStudent = model;
            }

        }
    }
}
