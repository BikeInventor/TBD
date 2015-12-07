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
using Railways.View;

namespace Railways.ViewModel
{
    public enum DialogWindowType
    {
        INFODIALOG,
        OPTIONDIALOG
    }

    public class DialogViewModel : ViewModelBase
    {
        private String _message;
        private String _firstButtonText;
        private String _secondButtonText;
        private String _firstButtonVisibility;
        private String _secondButtonVisibility;

        public String Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                RaisePropertyChanged("Message");
            }
        }
        public String FirstButtonText
        {
            get
            {
                return _firstButtonText;
            }
            set
            {
                _firstButtonText = value;
                RaisePropertyChanged("FirstButtonText");
            }
        }
        public String SecondButtonText
        {
            get
            {
                return _secondButtonText;
            }
            set
            {
                _secondButtonText = value;
                RaisePropertyChanged("SecondButtonText");
            }
        }
        public String FirstButtonVisibility
        {
            get
            {
                return _firstButtonVisibility;
            }
            set
            {
                _firstButtonVisibility = value;
                RaisePropertyChanged("FirstButtonVisibility");
            }
        }
        public String SecondButtonVisibility
        {
            get
            {
                return _secondButtonVisibility;
            }
            set
            {
                _secondButtonVisibility = value;
                RaisePropertyChanged("SecondButtonVisibility");
            }
        }


        /// <summary>
        /// Создание диалогового окна заданного типа с заданным собщением
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="dialogType">Тип диалогового окна</param>
        public DialogViewModel(String message, DialogWindowType dialogType)
        {
            this.Message = message;

            if (dialogType == DialogWindowType.INFODIALOG)
            {
                SetUpInfoDialog();
            }
            else
            {
                SetUpOptionDialog();
            }
        }

        private void SetUpInfoDialog()
        {
            this.FirstButtonVisibility = "0";
            this.SecondButtonVisibility = "100";
            this.SecondButtonText = "OK";
        }

        private void SetUpOptionDialog()
        {
            this.FirstButtonVisibility = "100";
            this.SecondButtonVisibility = "100";
            this.FirstButtonText = "ДА";
            this.SecondButtonText = "НЕТ";
        }
    }
}
