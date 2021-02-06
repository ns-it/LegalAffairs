using LegalAffairs.Commands;
using LegalAffairs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace LegalAffairs.ViewModels
{
    public class LogInViewModel
    {
        private readonly LegalAffairsDbContext db;
        public string UserNameField { get; set; }
        public string PasswordField { get; set; }
        public RelayCommand LogInCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public bool UserExists { get; set; }
        public User CurrentUser { get; private set; }
        public string StatusBarContent { get; private set; }
        public LogInViewModel()
        {
            db = new LegalAffairsDbContext();
            LogInCommand = new RelayCommand(ConfirmCredentialsAction, null);
            CancelCommand = new RelayCommand(CancelAction, null);


        }
 

        public void ConfirmCredentialsAction(object o)
        {
            PasswordBox passwordBox = o as PasswordBox;
            string password = passwordBox.Password;

            if (db.Users.Any(u => u.Username.Equals(UserNameField) && u.Password.Equals(password)))
            { MessageBox.Show("Success"); UserExists = true; CurrentUser = db.Users.Where(u => u.Username.Equals(UserNameField) && u.Password.Equals(password)).FirstOrDefault(); }
            else
            { MessageBox.Show("Failed"); UserExists = false; }
        }

        public void CancelAction(object o)
        {
            Application.Current.Shutdown();
        }
    }
}
