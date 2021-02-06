using LegalAffairs.Commands;
using LegalAffairs.Dialogs.DialogService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace LegalAffairs.Dialogs.DialogYesNo
{

 public  class DialogYesNoViewModel : DialogViewModelBase
    {
        public RelayCommand YesCommand { get; set; }
        public RelayCommand NoCommand { get; set; }

        public DialogYesNoViewModel(string message) : base(message)
        {
            YesCommand = new RelayCommand(OnYesClicked);
            NoCommand = new RelayCommand(OnNoClicked);
        }

        private void OnYesClicked(object parameter)
        {
            this.CloseDialogWithResult(parameter as Window, DialogResult.Yes);
        }
        private void OnNoClicked(object parameter)
        {
            this.CloseDialogWithResult(parameter as Window, DialogResult.No);
        }
    }

}
