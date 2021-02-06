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

namespace LegalAffairs.Dialogs.DialogService
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        public DialogWindow()
        {
            InitializeComponent();
        }

        //private void CloseButton_Click(object sender, RoutedEventArgs e)
        //{
        //    Application.Current.Shutdown();
        //    //this.Close();
        //}

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                if (e.ClickCount == 2)
                {
                    if (this.WindowState == WindowState.Maximized)
                    {
                        this.WindowState = WindowState.Normal;
                       

                        //MaximizeButton.Content = "1";
                    }
                    else
                    {
                        this.WindowState = WindowState.Maximized;
                        this.Width = this.ActualWidth;
                        this.Height = this.ActualHeight;
                        //MaximizeButton.Content = "2";
                    }
                }
                else
                {

                    DragMove();

                    //Application.Current.MainWindow.DragMove();
                }
        }
    }
}
