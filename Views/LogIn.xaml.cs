using LegalAffairs.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LegalAffairs.Views
{
    /// <summary>
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            LogInViewModel vm = DataContext as LogInViewModel;
            vm.ConfirmCredentialsAction(passwordBox);

            if (vm.UserExists)
            {
                new MainUI()
                {
                    DataContext = new MainUIViewModel(vm.CurrentUser)
                }.Show();

                this.Close();
            }
        }
    }
}
