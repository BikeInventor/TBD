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
using System.Windows.Data;

namespace Railways.ViewModel
{
    public class ScheduleWindowViewModel : ViewModelBase
    {
        private ObservableCollection<TripInfo> _obsTripInfo;
        public ObservableCollection<TripInfo> ObsTripInfo
        {
            get { return _obsTripInfo; }
            set { _obsTripInfo = value; }
        }

        private String _stationFrom;
        private String _stationTo;
        private DateTime _desiredDepartureDate;

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

        public ScheduleWindowViewModel() 
        {
            ContextKeeper.Initialize();
            _obsTripInfo = new ObservableCollection<TripInfo>();
            FindTrainsCmd = new RelayCommand(this.FindTrains);
            this.DesiredDepartureDate = DateTime.Now.AddDays(1);
        }

        public RelayCommand FindTrainsCmd { get; private set; }

        private void FindTrains()
        {
            var suitableVoyages = VoyageSearchEngine.FindVoyages(StationFrom, StationTo, DesiredDepartureDate);
            ObsTripInfo.Clear();
            suitableVoyages.ForEach(v => ObsTripInfo.Add(v));

            if (ObsTripInfo.Count != 0)
            {
                var isFree = TrainBuilder.CheckSeatAvailibility(33, ObsTripInfo[0].ArrivalTime, ObsTripInfo[0].DepartureTime);
                Console.WriteLine(isFree);
            }
        }
    }
}
