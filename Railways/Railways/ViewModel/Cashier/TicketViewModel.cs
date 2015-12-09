using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Railways.Model.Context;
using Railways.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Railways.Model.Logic;
using System.Collections.ObjectModel;
using Railways.ViewModel.Messages;
using GalaSoft.MvvmLight.Messaging;
using Railways.Model.ModelBuilder;
using Railways.View;
using Railways.ViewModel.Utils;
using System.Windows.Data;

namespace Railways.ViewModel
{
    public class TicketViewModel : ViewModelBase
    {
        public RelayCommand<TicketWindow> PrintTicketCmd { get; set; }

        public TicketViewModel() 
        {
            PrintTicketCmd = new RelayCommand<TicketWindow>(PrintTicket);
        }

        private void PrintTicket(TicketWindow window)
        {
            window.Close();
        }

    }
}
