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
        private String _stationName;
        private String _arrivalTime;
        private String _departureTime;
        private String _distance;
        private DateTime _departureOffset;
        private DateTime _arrivalOffset;

        private ObservableCollection<Route> _obsRoutesOfVoyage;
        public ObservableCollection<Route> ObsRoutesOfVoyage
        {
            get { return _obsRoutesOfVoyage; }
            set { _obsRoutesOfVoyage = value; }
        }
 
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
                RaisePropertyChanged("Distance");
            }
        }
        public DateTime DepartureOffset
        {
            get { return _departureOffset; }
            set { _departureOffset = value; RaisePropertyChanged("DepartureOffset"); }
        }
        public DateTime ArrivalOffset
        {
            get { return _arrivalOffset; }
            set { _arrivalOffset = value; RaisePropertyChanged("ArrivalOffset"); }
        }

        public RelayCommand AddRouteCmd { get; private set; }
        public RelayCommand DeleteRouteCmd { get; private set; }
        public RelayCommand<VoyageEditWindow> SaveVoyageInfoCmd { get; private set; }
        
        public VoyageEditViewModel()
        {
            ContextKeeper.Initialize();
            _obsRoutesOfVoyage = new ObservableCollection<Route>();
            
            AddRouteCmd = new RelayCommand(() => AddRouteToVoyage());
            DeleteRouteCmd = new RelayCommand(() => DeleteRouteFromVoyage());
            SaveVoyageInfoCmd = new RelayCommand<VoyageEditWindow>(this.SaveVoyageInfo);

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
            this.Periodicity = (this._voyage.Periodicity - 1).ToString();
            this.DepartureDate = (DateTime)this._voyage.DepartureDateTime;
            this.DepartureOffset = this.DepartureDate;
            this.ArrivalOffset = this.DepartureDate;
            RefreshRoutesOfVoyage();
        }

        private void RefreshRoutesOfVoyage()
        {
            this.ObsRoutesOfVoyage.Clear();
            var routes = VoyageBuilder.GetRoutesOfVoyage(this._voyage.Id);
            if (routes != null)
            {
                routes.ToList().ForEach(route => ObsRoutesOfVoyage.Add(route));
            }            
        }

        private void AddRouteToVoyage()
        {
            if (String.IsNullOrEmpty(this.StationName) || String.IsNullOrEmpty(this.DepartureTime)
                || String.IsNullOrEmpty(this.ArrivalTime) || String.IsNullOrEmpty(this.Distance)
                || String.IsNullOrEmpty(DepartureOffset.ToString()) || String.IsNullOrEmpty(ArrivalOffset.ToString())
                )
            {
                return;
            }
            var newRoute = new Route();
            newRoute.DepartureTimeOffset = DepartureOffset + TimeSpan.Parse(DepartureTime);
            newRoute.ArrivalTimeOffset = ArrivalOffset + TimeSpan.Parse(ArrivalTime); 
            newRoute.Distance = double.Parse(Distance);
            newRoute.StationId = AddStation();
            ContextKeeper.Routes.Add(newRoute);
            VoyageBuilder.AddRouteToVoyage(_voyage.Id, newRoute.Id);
            RefreshRoutesOfVoyage();
            this.StationName = this.DepartureTime = this.ArrivalTime = this.Distance = null;
        }

        private void DeleteRouteFromVoyage()
        {
            if (_obsRoutesOfVoyage.Count != 0)
            { 
                VoyageBuilder.DeleteLastRouteOfVoyage(_voyage.Id);
                RefreshRoutesOfVoyage();
            }
        }

        private void SaveVoyageInfo(VoyageEditWindow window)
        {
            this._voyage.Periodicity = (byte?)(byte.Parse(this.Periodicity) + 1);
            this._voyage.DepartureDateTime = this.DepartureDate;
            ContextKeeper.Voyages.Update(_voyage);
            window.Close();
        }

        /// <summary>
        /// Перевод строки с указаным временем в минуты.
        /// </summary>
        /// <param name="timeString"></param>
        /// <returns></returns>
        private int ParseTimeString(String timeString)
        {
            var time = TimeSpan.Parse(timeString);
            var offset = time.Minutes + time.Hours * 60;
            return offset;
        }

        /// <summary>
        /// Проверка существования станции с заданным названием.
        /// </summary>
        /// <returns>Id станции, если существует. Id новой станции, если нет.</returns>
        private int AddStation()
        {
            var station = ContextKeeper.Stations
                .Where(_station => _station.StationName == StationName)
                .Select(_station => _station).FirstOrDefault();
            if (station == null)
            {
                var newStation = new Station();
                newStation.StationName = StationName;
                ContextKeeper.Stations.Add(newStation);
                return newStation.Id;
            }
            return station.Id;
        }
    }
}
