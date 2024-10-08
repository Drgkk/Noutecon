﻿using Microsoft.Win32;
using Noutecon__Exam_.Model;
using Noutecon__Exam_.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Noutecon__Exam_.ViewModel
{
    public class TeacherProfileViewModel : ViewModelBase
    {
        private TeacherAccountModel currentTeacher;

        public TeacherAccountModel CurrentTeacher
        {
            get { return currentTeacher; }
            set { currentTeacher = value; OnPropertyChanged(nameof(CurrentTeacher)); }
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


        private TeacherViewViewModel teacherViewViewModel;
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

        public TeacherProfileViewModel(TeacherViewViewModel tvvm)
        {
            teacherViewViewModel = tvvm;
            CurrentTeacher = teacherViewViewModel.CurrentTeacher;
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
            if(PreviousPassword == null || PreviousPassword.Length <= 3
                || NewPassword == null || NewPassword.Length <= 3)
            {
                isValid = false;
            }
            return isValid;
        }

        private void ExecuteChangePassword(object obj)
        {
            PasswordErrorMessage = "";
            if(new NetworkCredential("", PreviousPassword).Password != teacherRepository.GetNetworkCredential(teacherViewViewModel.CurrentTeacher.Id).Password)
            {
                PasswordErrorMessage = "Previous password does not match with your current password!";
                return;
            }
            teacherRepository.EditPassword(teacherViewViewModel.CurrentTeacher.Id, new System.Net.NetworkCredential("", newPassword));
            System.Windows.Forms.MessageBox.Show("Password has been succesfully changed!", "Password change success", MessageBoxButtons.OK);
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
            LastName = currentTeacher.LastName;
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
            teacherRepository.EditTeacherLastName(currentTeacher.Id, LastName);
            TeacherAccountModel model = teacherRepository.GetAccountById(teacherViewViewModel.CurrentTeacher.Id);
            teacherViewViewModel.CurrentTeacher = model;
            CurrentTeacher = model;
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
            FirstName = currentTeacher.FirstName;
            FirstNameTextVisibility = false;
            FirstnameTextBoxVisibility = true;
        }

        private void ExecuteChangeFirstName(object obj)
        {
            FirstNameTextVisibility = true;
            FirstnameTextBoxVisibility = false;
            if(string.IsNullOrEmpty(FirstName))
            {
                FirstNameErrorMessage = "Cannot be null";
                return;
            }
            teacherRepository.EditTeacherFirstName(currentTeacher.Id, FirstName);
            TeacherAccountModel model = teacherRepository.GetAccountById(teacherViewViewModel.CurrentTeacher.Id);
            teacherViewViewModel.CurrentTeacher = model;
            CurrentTeacher = model;
        }

        private void ExecuteChangeUsernameVisibility(object obj)
        {
            UsernameErrorMessage = "";
            Username = currentTeacher.Username;
            UsernameTextVisibility = false;
            UsernameTextBoxVisibility = true;
        }

        private void UsernameTextBoxLostFocus()
        {
            if(usernameTextBoxVisibility ==  false)
            {
                usernameTextBoxVisibility = false;
                UsernameTextVisibility = true;
            }
        }

        private void ExecuteChangeUsername(object obj)
        {
            UsernameTextVisibility = true;
            UsernameTextBoxVisibility = false;
            if(teacherRepository.GetByUsername(Username) != null || studentRepository.GetByUsername(Username) != null)
            {
                UsernameErrorMessage = "Account with that username already exists!";
                return;
            }
            if (string.IsNullOrEmpty(Username))
            {
                UsernameErrorMessage = "Cannot be null";
                return;
            }
            if(System.Windows.Forms.MessageBox.Show($"Are you sure you want to change username: {CurrentTeacher.Username} to {Username}?", "Change Username", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }


            string filePathOld = $"{System.AppDomain.CurrentDomain.BaseDirectory}ProfilePictures\\{CurrentTeacher.Username}.jpg";            
            string filePathNew = $"{System.AppDomain.CurrentDomain.BaseDirectory}ProfilePictures\\{Username}.jpg";
            if (!File.Exists(filePathOld))
            {
                teacherRepository.EditTeacherUsername(currentTeacher.Id, Username);
                TeacherAccountModel model2 = teacherRepository.GetAccountById(teacherViewViewModel.CurrentTeacher.Id);
                teacherViewViewModel.CurrentTeacher = model2;
                CurrentTeacher = model2;
                return;
            }
            teacherViewViewModel.CurrentTeacher.ProfilePicturePath = filePathNew;
            CurrentTeacher.ProfilePicturePath = filePathNew;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (File.Exists(filePathOld))
            {
                using (File.Create(filePathNew)) { }
                System.IO.File.Copy(filePathOld, filePathNew, true);
            }

            teacherRepository.EditTeacherUsername(currentTeacher.Id, Username);
            
            teacherRepository.EditPfpById(currentTeacher.Id, filePathNew);
            TeacherAccountModel model = teacherRepository.GetAccountById(teacherViewViewModel.CurrentTeacher.Id);
            teacherViewViewModel.CurrentTeacher = model;
            CurrentTeacher = model;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            File.Delete(filePathOld);
        }

        private void ExecuteChangePFP(object obj)
        {
            System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = $"{System.AppDomain.CurrentDomain.BaseDirectory}ProfilePictures\\{CurrentTeacher.Username}.jpg";
                if (!File.Exists(filePath))
                {
                    using (File.Create(filePath)) { }
                }
                else
                {
                    TeacherAccountModel teacherAccountModelTemp = new TeacherAccountModel() { Id = teacherViewViewModel.CurrentTeacher.Id, FirstName = teacherViewViewModel.CurrentTeacher.FirstName, LastName = teacherViewViewModel.CurrentTeacher.LastName, Username = teacherViewViewModel.CurrentTeacher.Username, School = teacherViewViewModel.CurrentTeacher.School, ProfilePicturePath = " " };
                    teacherViewViewModel.CurrentTeacher = teacherAccountModelTemp;
                    CurrentTeacher = teacherAccountModelTemp;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                System.IO.File.Copy(fileDialog.FileName, filePath, true);
                teacherRepository.EditPfpById(teacherViewViewModel.CurrentTeacher.Id, filePath);
                TeacherAccountModel model = teacherRepository.GetAccountById(teacherViewViewModel.CurrentTeacher.Id);
                teacherViewViewModel.CurrentTeacher = model;
                CurrentTeacher = model;
                




            }
        }

        



    }
}
