using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Railways.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Railways.Model.Logic;
using Railways.Model.Context;
using Microsoft.Practices.ServiceLocation;
using Railways.ViewModel.Messages;
using GalaSoft.MvvmLight.Messaging;

namespace Railways.ViewModel
{
    public class DialogViewModel : ViewModelBase
    {
        public String Title { get; set; }
        public String Message { get; set; }
        public String FirstButtonText { get; set; }
        public String SecondButtonText { get; set; }
        public String FirstButtonVisibility { get; set; }
        public String SecondButtonVisibility { get; set; }
        public bool DialogResult { get; private set; }

        public RelayCommand<DialogWindow> FirstButtonPressCmd {get; private set;}
        public RelayCommand<DialogWindow> SecondButtonPressCmd { get; private set; }

        public DialogViewModel()
        {
            FirstButtonPressCmd = new RelayCommand<DialogWindow>(this.FirstButtonPress);
            SecondButtonPressCmd = new RelayCommand<DialogWindow>(this.FirstButtonPress);
        }

        private void FirstButtonPress(DialogWindow thisWindow)
        {
            this.DialogResult = true;
            thisWindow.Close();
        }

        private void SecondButtonPress(DialogWindow thisWindow)
        {
            this.DialogResult = false;
            thisWindow.Close();
        }


    }
}
