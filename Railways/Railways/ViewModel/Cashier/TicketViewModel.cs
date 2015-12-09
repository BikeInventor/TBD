﻿using System;
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
        private TicketInfoMessage ticketMessage;

        private String _tripInfoText;
        private String _directionText;
        private String _seatText;
        private String _clientInfoText;
        private String _costText;
        private String _arrivalText;
        private String _ticketNumText;

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
            PrintTicketCmd = new RelayCommand<TicketWindow>(PrintTicket);
            Messenger.Default.Register<TicketInfoMessage>(this, (msg) =>
            {
                ticketMessage = msg;
                SetTicketInfo();
            });
        }

        private void PrintTicket(TicketWindow window)
        {
            window.Close();
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
            SeatText = "МЕСТА 0" + SeatNum() + " ФПК ДАЛЬНОВОСТОЧНЫЙ";
            ClientInfoText = "ПН " + client.PassportNum.ToUpper() +
                "/" + client.FullName.ToUpper();
            CostText = "W-" + ticketMessage.TicketPrice +
                " РУБ В Т.Ч.СТР.2.3; ТАРИФ РФ 496.5 В Т.Ч.НДС 71.62 РУБ" +
                "\n" + "СЕРВИС 83.8 В Т.Ч.НДС 12.78 РУБ С БЕЛЬЕМ УО ЭЛ.ДОК.347238532/4";
            ArrivalText = "ПРИБЫТИЕ " + String.Format("{0:dd.MM}", ticketMessage.TripInfo.ArrivalTime) +
                " В " + ticketMessage.TripInfo.ArrivalTime.ToShortTimeString() + 
                "\n" + "ВРЕМЯ ОТПР И ПРИБ МОСКОВСКОЕ";
        }
    }
}