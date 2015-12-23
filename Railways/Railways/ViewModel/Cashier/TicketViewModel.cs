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
using System.Windows.Controls;

namespace Railways.ViewModel
{
    public class TicketViewModel : ViewModelBase
    {
        public RelayCommand<object> PrintTicketCmd { get; set; }
        private TicketInfoMessage ticketMessage;

        private String _tripInfoText;
        private String _directionText;
        private String _seatText;
        private String _clientInfoText;
        private String _costText;
        private String _arrivalText;
        private String _ticketNumText;
        private String _controlText;

        public String ControlText
        {
            get { return _controlText; }
            set 
            {
                _controlText = value;
                RaisePropertyChanged("ControlText");
            }
        }
        public String TripInfoText
        {
            get { return _tripInfoText; }
            set
            {
                _tripInfoText = value;
                RaisePropertyChanged("TripInfoText");
            }
        }
        public String DirectionText
        {
            get { return _directionText; }
            set
            {
                _directionText = value;
                RaisePropertyChanged("DirectionText");
            }
        }
        public String SeatText
        {
            get { return _seatText; }
            set
            {
                _seatText = value;
                RaisePropertyChanged("SeatText");
            }
        }
        public String ClientInfoText
        {
            get { return _clientInfoText; }
            set
            {
                _clientInfoText = value;
                RaisePropertyChanged("ClientInfoText");
            }
        }
        public String CostText
        {
            get { return _costText; }
            set
            {
                _costText = value;
                RaisePropertyChanged("CostText");
            }
        }
        public String ArrivalText
        {
            get { return _arrivalText; }
            set
            {
                _arrivalText = value;
                RaisePropertyChanged("ArrivalText");
            }
        }
        public String TicketNumText
        {
            get { return _ticketNumText; }
            set
            {
                _ticketNumText = value;
                RaisePropertyChanged("TicketNumText");
            }
        }

        public TicketViewModel() 
        {
            PrintTicketCmd = new RelayCommand<object>(PrintTicket);
            Messenger.Default.Register<TicketInfoMessage>(this, (msg) =>
            {
                if (msg.IsForPrint)
                {
                    ticketMessage = msg;
                    SetTicketInfo();
                }
            });
        }

        private void PrintTicket(object grid)
        {
            Grid area = (Grid)grid;
            var printDialog = new PrintDialog();
            if (printDialog.ShowDialog() != true)
            {
                return;
            }
            printDialog.PrintVisual(area, "Печать билета");
        }

        private String WagonTypePrefix()
        {
            switch (TrainBuilder.GetWagonTypeOfSeat(ticketMessage.SeatId))
            {
                case WagonType.BERTH:
                    {
                        return "П";
                    }
                case WagonType.COUPE:
                    {
                        return "К";
                    }
                case WagonType.LUX:
                    {
                        return "Л";
                    }
                default:
                    {
                        return "П";
                    }
            }
        }

        private String SeatNum()
        {
            return ContextKeeper.Seats
                .Where(s => s.Id == ticketMessage.SeatId)
                .Select(s => s.SeatNum)
                .First()
                .Value
                .ToString();
        }

        private Client ClientInfo()
        {
            return ContextKeeper.Clients
                .Where(c => c.Id == ticketMessage.ClientId)
                .First();
        }

        private String TicketNumber()
        {
            var currentTicket = ContextKeeper.Tickets.Select(ticket => ticket.Id).Max();
            return "HK" + DateTime.Now.Year * 1000 + currentTicket;
        }

        private void SetTicketInfo()
        {
            var client = ClientInfo();

            TripInfoText = ticketMessage.TripInfo.TrainNumber.ToUpper() +
                "  " + String.Format("{0:dd.MM}",ticketMessage.TripInfo.DepartureTime) +
                " " + ticketMessage.TripInfo.DepartureTime.ToShortTimeString() +
                "   " + TrainBuilder.GetWagonNumOfSeat(ticketMessage.SeatId) + 
                " " + WagonTypePrefix() + " 000280.3" + " 000273.0" +
                " " + "01" + " "+ "полный W";
            DirectionText = ticketMessage.TripInfo.DepartureStation.ToUpper() +
                " - " + ticketMessage.TripInfo.ArrivalStation.ToUpper()
                + "(2000002-2010090)" + "КЛ.ОБСЛ.ЗП";
            SeatText = "МЕСТА 0" + SeatNum() + " ФПК/20 1797 21 2130";
            ClientInfoText = "ПН " + client.PassportNum.ToUpper() +
                "/" + client.FullName.ToUpper();
            CostText = "W-" + String.Format("{0:0.00}", ticketMessage.TicketPrice) +
                " РУБ В Т.Ч.СТР.2.3; ТАРИФ РФ 496.5 В Т.Ч.НДС 71.62 РУБ" +
                "\n" + "СЕРВИС 83.8 В Т.Ч.НДС 12.78 РУБ С БЕЛЬЕМ УО ЭЛ.ДОК.347238532/4";
            ArrivalText = "ПРИБЫТИЕ " + String.Format("{0:dd.MM}", ticketMessage.TripInfo.ArrivalTime) +
                " В " + ticketMessage.TripInfo.ArrivalTime.ToShortTimeString() + 
                "\n" + "ВРЕМЯ ОТПР И ПРИБ МОСКОВСКОЕ";
            TicketNumText = TicketNumber();

            ControlText = ticketMessage.TripInfo.TrainNumber.ToUpper() +
                " " + String.Format("{0:dd.MM}", ticketMessage.TripInfo.DepartureTime) +
                " " + ticketMessage.TripInfo.DepartureTime.ToShortTimeString() +
                " " + TrainBuilder.GetWagonNumOfSeat(ticketMessage.SeatId) +
                " " + WagonTypePrefix() + "\n\n" +
                ticketMessage.TripInfo.DepartureStation.ToUpper() + "\n" +
                ticketMessage.TripInfo.ArrivalStation.ToUpper() + "\n\n" +
                "ПН" + client.PassportNum.ToUpper() + "\n" +
                NameCutter.MakeShorter(client.FullName).ToUpper() + "\n\n" +
                "СУММА " + String.Format("{0:0.00}", ticketMessage.TicketPrice) + "\n\n" +
                "20.4 С БЕЛЬЕМ УО" + "\n" +
                "20 1797 21 2130" + "\n" +
                "347238532/4";
        }
    }
}
