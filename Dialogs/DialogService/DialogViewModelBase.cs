using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace LegalAffairs.Dialogs.DialogService
{
    public abstract class DialogViewModelBase
    {
        public string Message { get; protected set; }
        public SolidColorBrush ForegroundColor { get; set; }
        public SolidColorBrush BackgroundColor { get; set; }

        public DialogType DialogType { get; private set; }

        public DialogViewModelBase()
        {
            DialogType = DialogType.DEFAULT;
        }

        public DialogViewModelBase(DialogType type)
        {
            DialogType = type;
        }

        public DialogViewModelBase(string message)
        {
            this.Message = message;
            DialogType = DialogType.DEFAULT;
        }

        public DialogViewModelBase(string message, DialogType type) : this(message)
        {
            DialogType = type;
        }

        public DialogResult UserDialogResult { get; private set; }

        public void CloseDialogWithResult(Window dialog, DialogResult result)
        {
            this.UserDialogResult = result;
            if (dialog != null)
                if (result.Equals(DialogResult.Yes))
                    dialog.DialogResult = true;
                else if (result.Equals(DialogResult.No))
                    dialog.DialogResult = false;


        }

    }
}
