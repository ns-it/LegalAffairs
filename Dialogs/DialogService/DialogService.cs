using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace LegalAffairs.Dialogs.DialogService
{
    public static class DialogService
    {
        public static DialogResult OpenDialog(DialogViewModelBase vm, Window owner)
        {
            DialogWindow dialogWindow = new DialogWindow();

            if (owner != null)
            {
                dialogWindow.Owner = owner;
                dialogWindow.FlowDirection = owner.FlowDirection;
            }

            SetUIColor(ref vm);

            dialogWindow.DataContext = vm;
            dialogWindow.ShowDialog();


            DialogResult result = (dialogWindow.DataContext as DialogViewModelBase).UserDialogResult;
            return result;
        }

        private static void SetUIColor(ref DialogViewModelBase vm)
        {
            if (vm.DialogType.Equals(DialogType.DANGER))
            {
                vm.BackgroundColor = (SolidColorBrush)Application.Current.Resources["Background.Danger"];
                vm.ForegroundColor = (SolidColorBrush)Application.Current.Resources["Foreground.Light"];
            }
            else if (vm.DialogType.Equals(DialogType.SUCCESS))
            {
                vm.BackgroundColor = (SolidColorBrush)Application.Current.Resources["Background.Success"];
                vm.ForegroundColor = (SolidColorBrush)Application.Current.Resources["Foreground.Light"];
            }
            else if (vm.DialogType.Equals(DialogType.WARNING))
            {
                vm.BackgroundColor = (SolidColorBrush)Application.Current.Resources["Background.Warning"];
                vm.ForegroundColor = (SolidColorBrush)Application.Current.Resources["Foreground.Light"];
            }
        }
    }
}
