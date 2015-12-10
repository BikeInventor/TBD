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
    public class ScheduleWindowViewModel : ViewModelBase
    {
        #region Properties
        private AsyncObservableCollection<TripInfo> _obsTripInfo;
        private String _stationFrom;
        private String _stationTo;
        private String _loadingVisibility;
        private DateTime _desiredDepartureDate;
        private bool _openTripButtonAvailability;
        private bool _searchButtonAvailability;
        public bool OpenTripButtonAvailability
        {
            get
            {
                return _openTripButtonAvailability;
            }
            set
            {
                _openTripButtonAvailability = value;
                RaisePropertyChanged("OpenTripButtonAvailability");
            }
        }
        public bool SearchButtonAvailability
        {
            get { return _searchButtonAvailability; }
            set
            {
                _searchButtonAvailability = value;
                RaisePropertyChanged("SearchButtonAvailability");
            }
        }
        private String _noTicketsFoundMsgVisibility;
        public String NoTicketsFoundMsgVisibility
        {
            get
            {
                return _noTicketsFoundMsgVisibility;
            }
            set
            {
                _noTicketsFoundMsgVisibility = value;
                RaisePropertyChanged("NoTicketsFoundMsgVisibility");
            }
        }
        public IEnumerable<String> Stations 
        {
            get; private set; 
        }
        public String LoadingVisibility
        {
            get { return _loadingVisibility; }
            set
            {
                this._loadingVisibility = value;
                RaisePropertyChanged("LoadingVisibility");
            }
        }
        public String SelectedStation 
        { 
            get; set; 
        }
        public String StationFrom
        {
            get { return _stationFrom; }
            set { _stationFrom = value; }
        }
        public String StationTo
        {
            get { return _stationTo; }
            set { _stationTo = value; }
        }
        public DateTime DesiredDepartureDate
        {
            get { return _desiredDepartureDate; }
            set { _desiredDepartureDate = value; }
        }
        public int SelectedTrip
        { 
            get; set; 
        }
        public List<TripInfo> SuitableVoyages 
        {
            get; set; 
        }
        public AsyncObservableCollection<TripInfo> ObsTripInfo
        {
            get { return _obsTripInfo; }
            set { _obsTripInfo = value; }
        }
        #endregion

        #region Commands
        public RelayCommand FindTrainsCmd 
        { 
            get;
            private set; 
        }
        public RelayCommand OpenTripCmd
        {
            get;
            private set; 
        }
        #endregion

        public ScheduleWindowViewModel() 
        {
            LoadingVisibility = "0";
            NoTicketsFoundMsgVisibility = "0";
            OpenTripButtonAvailability = true;
            SearchButtonAvailability = true;

            Stations = ContextKeeper.Stations
                .All()
                .Select(s => s.StationName)
                .AsEnumerable<String>();

            _obsTripInfo = new AsyncObservableCollection<TripInfo>();
            FindTrainsCmd = new RelayCommand(this.FindTrains);
            OpenTripCmd = new RelayCommand(this.OpenTrip);
            SuitableVoyages = new List<TripInfo>();
            this.DesiredDepartureDate = DateTime.Now.Date.AddDays(1);
        }

        /// <summary>
        /// Поиск поездов по заданным станциям отправления и прибытия и времени отправления
        /// </summary>
        public async void FindTrains()
        {
            LoadingVisibility = "100";
            NoTicketsFoundMsgVisibility = "0";
            SearchButtonAvailability = false;
            try
            {
                this.SuitableVoyages = await VoyageSearchEngine.FindVoyagesAsync(StationFrom, StationTo, DesiredDepartureDate);
            }
            catch
            {
                NoTicketsFoundMsgVisibility = "100";
                Console.WriteLine("По данному запросу маршрутов не найдено!");
            }
            ObsTripInfo.Clear();
            if (SuitableVoyages == null || SuitableVoyages.Count == 0)
            {
                LoadingVisibility = "0";
                SearchButtonAvailability = true;
                NoTicketsFoundMsgVisibility = "100";
                return;
            }
                SuitableVoyages.ForEach(v => 
                {     
                    ObsTripInfo.Add(v);
                });
            LoadingVisibility = "0";
            SearchButtonAvailability = true;
        }

        /// <summary>
        /// Открытие выбранной поездки
        /// </summary>
        private void OpenTrip()
        {
            if (this.SelectedTrip >= 0)
            {
                ContextKeeper.ResetConnection();

                HideOpenTripButton();

                var wagonsInfoWin = new SeatSetWindow();
                wagonsInfoWin.Show();

                Messenger.Default.Send(new TripInfoMessage(ObsTripInfo[SelectedTrip]));
            }
        }

        /// <summary>
        /// Установка кнопки выбора поезда в неактивное состояние
        /// для предотвращения повторных нажатий
        /// </summary>
        private async void HideOpenTripButton()
        {
            await Task.Run(() =>
            {
                OpenTripButtonAvailability = false;
                System.Threading.Thread.Sleep(3000);
                OpenTripButtonAvailability = true;
            });
        }
    }
}
