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
    public class ClientInfoViewModel : ViewModelBase
    {
        private String _fio;
        private String _passportNum;

        private TicketInfoMessage message;

        public String FIO
        {
            get
            {
                return _fio;
            }
            set
            {
                _fio = value;
                RaisePropertyChanged("FIO");
            }
        }   
        public String PassportNum
        {
            get
            {
                return _passportNum;
            }
            set
            {
                _passportNum = value;
                RaisePropertyChanged("PassportNum");
            }
        }

        public RelayCommand<ClientInfoWindow> SaveClientInfoCmd { get; private set; }

        public ClientInfoViewModel() 
        {
            SaveClientInfoCmd = new RelayCommand<ClientInfoWindow>(this.SaveClientInfo);

            Messenger.Default.Register<TicketInfoMessage>(this, (msg) =>
            {
                this.message = msg;
            });
        }

        private void SaveClientInfo(ClientInfoWindow window)
        {
            if (!String.IsNullOrEmpty(FIO) || !String.IsNullOrEmpty(PassportNum))
            {
                var client = new Client();
                client.FullName = this.FIO;
                client.PassportNum = this.PassportNum;
                ContextKeeper.Clients.Add(client);
                this.message.ClientId = client.Id;
                BuyTicket();
                var ticket = new TicketWindow();
                ticket.Show();
                window.Close();
            }
        }

        private void BuyTicket()
        {
            var newTicket = new Ticket();
            newTicket.ClientId = message.ClientId;
            newTicket.SeatId = message.SeatId;
            newTicket.EmployeeId = message.EmpId;
            newTicket.Price = message.TicketPrice;
            newTicket.ArrivalDate = message.TripInfo.ArrivalTime;
            newTicket.DepartureDate = message.TripInfo.DepartureTime;
            newTicket.ArrivalRouteId = message.TripInfo.ArrRoute.Id;
            newTicket.DepartureRouteId = message.TripInfo.DepRoute.Id;

            ContextKeeper.Tickets.Add(newTicket);
        }
    }
}
