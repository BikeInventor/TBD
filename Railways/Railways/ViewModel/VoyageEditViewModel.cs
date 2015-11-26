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
using Railways.View;
using Railways.Model.ModelBuilder;
using System.Windows.Controls;

namespace Railways.ViewModel
{
    public class VoyageEditViewModel : ViewModelBase
    {
        private String _trainNum;
        private Voyage _voyage;
        private String _periodicity;
        private DateTime _departureDate;
        private DateTime _generateToDate;
        private String _stationName;
        private String _arrivalTime;
        private String _departureTime;
        private String _distance;

        private ObservableCollection<Route> _obsRoutesOfVoyage;
 
        public String TrainNum
        {
            get { return _trainNum; }
            set
            {
                _trainNum = value;
                RaisePropertyChanged("TrainNum");
            }
        }
        public String Periodicity
        {
            get { return _periodicity; }
            set 
            {
                _periodicity = value;
                RaisePropertyChanged("Periodicity");
            }
        }
        public DateTime DepartureDate
        {
            get { return _departureDate; }
            set
            {
                _departureDate = value;
                RaisePropertyChanged("DepartureDate");
            }
        }
        public DateTime GenerateToDate
        {
            get { return _generateToDate; }
            set
            {
                _generateToDate = value;
                RaisePropertyChanged("GenerateToDate");
            }
        }
        public String StationName
        {
            get { return _stationName; }
            set
            {
                _stationName = value;
                RaisePropertyChanged("StationName");
            }
        }
        public String ArrivalTime
        {
            get { return _arrivalTime; }
            set 
            { 
                _arrivalTime = value;
                RaisePropertyChanged("ArrivalTime");
            }
        }
        public String DepartureTime
        {
            get { return _departureTime; }
            set
            {
                _departureTime = value;
                RaisePropertyChanged("DepartureTime");
            }
        }
        public String Distance
        {
            get { return _distance; }
            set
            {
                _distance = value;
                RaisePropertyChanged("ArrivalTimeDistance");
            }
        }
        
        public VoyageEditViewModel()
        {
            ContextKeeper.Initialize();
            _obsRoutesOfVoyage = new ObservableCollection<Route>();

            Messenger.Default.Register<TrainOfVoyageMessage>(this, (msg) =>
            {
                SetVoyageInfo(msg.TrainId);
            });
        }

        private void SetVoyageInfo(int trainId) 
        {
            this._voyage = VoyageBuilder.GetVoyageOfTrain(trainId);
            this.TrainNum = "Номер поезда: " + ContextKeeper.Trains
                .Where(train => train.Id == trainId)
                .Select(train => train.TrainNum)
                .First();
            this.Periodicity = this._voyage.Periodicity.ToString();
            if (this._voyage.DepartureDateTime != null) 
                this.DepartureDate = (DateTime)this._voyage.DepartureDateTime;
        }



    }
}
