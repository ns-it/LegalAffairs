using LegalAffairs.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LegalAffairs.ViewModels
{
    public class MainUIViewModel
    {
        //private readonly LegalAffairsDbContext db;
        public static User CurrentUser { get; private set; }
        public string CurrentUserName { get; private set; }
        public string StatusBarContent { get; private set; }

        public MainUIViewModel()
        {
            //db = new LegalAffairsDbContext();
        }
        public MainUIViewModel(User CurrentUser) : base()
        {
            MainUIViewModel.CurrentUser = CurrentUser;
            CurrentUserName = CurrentUser.FullName;
            StatusBarContent = "تم تسجيل الدخول باسم: " + CurrentUserName;
        }
    }
}
