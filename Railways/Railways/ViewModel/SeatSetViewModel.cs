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
    public class SeatSetViewModel : ViewModelBase
    {
        private List<WagonSeatsSet> _berth;
        private List<WagonSeatsSet> _coupe;
        private List<WagonSeatsSet> _lux;

        private bool _check;
        public bool Check
        {
            get { return _check; }
            set { _check = value; RaisePropertyChanged("Check"); }
        }

        private int trainId;
        private DateTime arrDate;
        private DateTime depDate;

        public WagonSeatsSet CurrentBerth
        {
            get;
            private set;
        }

        public List<WagonSeatsSet> Berth
        {
            get { return _berth; }
            set { _berth = value; RaisePropertyChanged("Berth"); }
        }
        public List<WagonSeatsSet> Coupe
        {
            get { return _coupe; }
            set { _coupe = value; RaisePropertyChanged("Coupe"); }
        }
        public List<WagonSeatsSet> Lux
        {
            get { return _lux; }
            set { _lux = value; RaisePropertyChanged("Lux"); }
        }
        
        private ObservableCollection<String> _berthSeatsVisibility;
        public ObservableCollection<String> BerthSeatsVisibility
        {
            get { return _berthSeatsVisibility; }
            set { _berthSeatsVisibility = value; }
        }
        private ObservableCollection<String> _coupeSeatsVisibility;
        public ObservableCollection<String> CoupeSeatsVisibility
        {
            get { return _coupeSeatsVisibility; }
            set { _coupeSeatsVisibility = value; }
        }
        private ObservableCollection<String> _luxSeatsVisibility;
        public ObservableCollection<String> LuxSeatsVisibility
        {
            get { return _luxSeatsVisibility; }
            set { _luxSeatsVisibility = value; }
        }

        public RelayCommand NextBerthCmd;
        public RelayCommand PrevBerthCmd;

        public RelayCommand NextCoupeCmd;
        public RelayCommand PrevCoupeCmd;

        public RelayCommand NextLuxCmd;
        public RelayCommand PrevLuxCmd;
                                

        public SeatSetViewModel()
        {
            _berth = new List<WagonSeatsSet>();
            _coupe = new List<WagonSeatsSet>();
            _lux = new List<WagonSeatsSet>();

            _berthSeatsVisibility = new ObservableCollection<String>();
            _coupeSeatsVisibility = new ObservableCollection<String>();
            _luxSeatsVisibility = new ObservableCollection<String>();

            NextBerthCmd = new RelayCommand(() => NextBerth());
            PrevBerthCmd = new RelayCommand(() => PrevBerth());

            Messenger.Default.Register<TripInfoMessage>(this, (msg) =>
            {
                SetTrainInfo(msg.TrainId, msg.DepDate, msg.ArrDate);
            });
        }

        /// <summary>
        /// Формирование списков вагонов поезда по их типам
        /// </summary>
        /// <param name="trainId"></param>
        public void SetTrainInfo(int trainId, DateTime depDate, DateTime arrDate)
        {
            Check = false;
            this.trainId = trainId;
            this.depDate = depDate;
            this.arrDate = arrDate;

           // this.TrainInfo += "Поезд № ";
           // this.TrainInfo += ContextKeeper.Trains.Where(t => t.Id == trainId).Select(t => t.TrainNum).First() + "\n";

            var wagonsOfTrain = TrainBuilder.GetWagonsOfTrain(trainId).ToList();
            wagonsOfTrain.ForEach(wagon =>
            {
                var wagonType = (WagonType)ContextKeeper.Wagons.Where(w => w.Id == wagon.Id).Select(w => w.WagonType.Value).First();
                switch (wagonType)
                {
                    case WagonType.BERTH:
                        {
                            this.Berth.Add(new WagonSeatsSet(wagon.Id, depDate, arrDate));
                            break;
                        }
                    case WagonType.COUPE:
                        {
                            this.Berth.Add(new WagonSeatsSet(wagon.Id, depDate, arrDate));
                            break;
                        }
                    case WagonType.LUX:
                        {
                            this.Berth.Add(new WagonSeatsSet(wagon.Id, depDate, arrDate));
                            break;
                        }
                }
            });

            SetCurrentWagons();
            SetWagonSeatsButtonsVisibility(CurrentBerth, BerthSeatsVisibility);
        }

        private void SetWagonSeatsButtonsVisibility(WagonSeatsSet seats, ObservableCollection<String> visibility)
        {
           //3 this.TrainInfo += "Вагон № ";
            visibility.Clear();
            seats.Seats.ForEach(seatIsFree => 
            {
                if (seatIsFree)
                    visibility.Add("0.3");
                else
                    visibility.Add("1");
            });
        }

        private void SetCurrentWagons()
        {
            if (this.Berth.Count != 0)
            {
                this.CurrentBerth = Berth[0];
            }

            if (this.Coupe.Count != 0)
            {
                this.CurrentBerth = Coupe[0];
            }

            if (this.Lux.Count != 0)
            {
                this.CurrentBerth = Lux[0];
            }
        }

        private void NextBerth()
        {
            var currentIndex = Berth.IndexOf(CurrentBerth);

            if (Berth.Count > currentIndex)
            {
                CurrentBerth = Berth[++currentIndex];
                SetWagonSeatsButtonsVisibility(CurrentBerth, BerthSeatsVisibility);
            }
        }

        private void PrevBerth()
        {
            var currentIndex = Berth.IndexOf(CurrentBerth);

            if (currentIndex != 0)
            {
                CurrentBerth = Berth[--currentIndex];
                SetWagonSeatsButtonsVisibility(CurrentBerth, BerthSeatsVisibility);
            }
        }
    }
}
