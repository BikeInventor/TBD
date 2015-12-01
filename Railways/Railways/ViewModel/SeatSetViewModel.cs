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

        private int _currentTabIndex;
        public int CurrentTabIndex
        {
            get { return _currentTabIndex; }
            set 
            { 
                _currentTabIndex = value; 
                RaisePropertyChanged("CurrentTabIndex"); 
                SetCurrentWagonType();
                SetCurrentWagonInfo();
            }
        }

        private WagonType _currentWagonType;

        private String _wagonInfo = "";
        public String WagonInfo
        {
            get { return this._wagonInfo; }
            set 
            {
                _wagonInfo = value;
                RaisePropertyChanged("WagonInfo");
            }
        }
        

        private bool _nextEnabled;
        private bool _prevEnabled;
        public bool NextEnabled
        {
            get { return _nextEnabled; }
            set 
            {
                _nextEnabled = value;
                RaisePropertyChanged("NextEnabled");
            }
        }
        public bool PrevEnabled
        {
            get { return _prevEnabled; }
            set
            {
                _prevEnabled = value;
                RaisePropertyChanged("PrevEnabled");
            }
        }

        private int trainId;
        private DateTime arrDate;
        private DateTime depDate;

        public WagonSeatsSet CurrentBerth
        {
            get;
            private set;
        }
        public WagonSeatsSet CurrentCoupe
        {
            get;
            private set;
        }
        public WagonSeatsSet CurrentLux
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

        public RelayCommand NextWagonCmd { get; set; }
        public RelayCommand PrevWagonCmd { get; set; }                              

        public SeatSetViewModel()
        {
            _berth = new List<WagonSeatsSet>();
            _coupe = new List<WagonSeatsSet>();
            _lux = new List<WagonSeatsSet>();

            CurrentBerth = new WagonSeatsSet();
            CurrentCoupe = new WagonSeatsSet();
            CurrentLux = new WagonSeatsSet();

            _berthSeatsVisibility = new ObservableCollection<String>();
            _coupeSeatsVisibility = new ObservableCollection<String>();
            _luxSeatsVisibility = new ObservableCollection<String>();

            NextWagonCmd = new RelayCommand(() => NextWagon());
            PrevWagonCmd = new RelayCommand(() => PrevWagon());

            Messenger.Default.Register<TripInfoMessage>(this, (msg) =>
            {
                SetTrainInfo(msg.TrainId, msg.DepDate, msg.ArrDate);
            });
        }

        /// <summary>
        /// Формирование списков вагонов поезда по их типам
        /// </summary>
        /// <param name="trainId"></param>
        private void SetTrainInfo(int trainId, DateTime depDate, DateTime arrDate)
        {
            this.trainId = trainId;
            this.depDate = depDate;
            this.arrDate = arrDate;

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
                            this.Coupe.Add(new WagonSeatsSet(wagon.Id, depDate, arrDate));
                            break;
                        }
                    case WagonType.LUX:
                        {
                            this.Lux.Add(new WagonSeatsSet(wagon.Id, depDate, arrDate));
                            break;
                        }
                }

            });
            SetCurrentWagons();

          //  SetWagonSeatsButtonsVisibility(CurrentBerth, _berthSeatsVisibility);
            //SetWagonSeatsButtonsVisibility(CurrentCoupe, CoupeSeatsVisibility);
            //SetWagonSeatsButtonsVisibility(CurrentLux, LuxSeatsVisibility);

            NextEnabled = true;  
            CurrentTabIndex = 0;
            SetCurrentWagonInfo();
        }

        private void SetWagonSeatsButtonsVisibility(WagonSeatsSet seats, ObservableCollection<String> visibility)
        {
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
                this.CurrentCoupe = Coupe[0];
            }

            if (this.Lux.Count != 0)
            {
                this.CurrentLux = Lux[0];
            }
        }

        private void NextWagon()
        {

            switch (_currentTabIndex)
            {
                case 0:
                    {
                        var currentIndex = Berth.IndexOf(CurrentBerth);
                        if (SetNext(Berth.Count, currentIndex))
                        {
                            CurrentBerth = Berth[++currentIndex];
                            SetWagonSeatsButtonsVisibility(CurrentBerth, _berthSeatsVisibility);
                            RaisePropertyChanged("BerthSeatsVisibility");
                            SetCurrentWagonInfo();
                        }
                        break;
                    }
                case 1:
                    {
                        var currentIndex = Coupe.IndexOf(CurrentCoupe);
                        if (SetNext(Coupe.Count, currentIndex))
                        {
                            CurrentCoupe = Coupe[++currentIndex];
                            SetWagonSeatsButtonsVisibility(CurrentCoupe, _coupeSeatsVisibility);
                            SetCurrentWagonInfo();
                        }
                        break;
                    }
                case 2:
                    {
                        var currentIndex = Lux.IndexOf(CurrentLux);
                        if (SetNext(Lux.Count, currentIndex))
                        {
                            CurrentLux = Lux[++currentIndex];
                            SetWagonSeatsButtonsVisibility(CurrentLux, _luxSeatsVisibility);
                            SetCurrentWagonInfo();
                        }
                        break;
                    }
                }
        }
           
        private void PrevWagon()
        {

            switch (_currentTabIndex)
            {
                case 0:
                    {
                        var currentIndex = Berth.IndexOf(CurrentBerth);
                        if (SetPrev(Berth.Count, currentIndex))
                        {
                            CurrentBerth = Berth[--currentIndex];
                            //   SetWagonSeatsButtonsVisibility(currentWagon, BerthSeatsVisibility);
                            SetCurrentWagonInfo();
                        }                     
                        break;
                    }
                case 1:
                    {
                        var currentIndex = Coupe.IndexOf(CurrentCoupe);
                        if (SetPrev(Coupe.Count, currentIndex))
                        {
                            CurrentCoupe = Coupe[--currentIndex];
                            //   SetWagonSeatsButtonsVisibility(currentWagon, CoupeSeatsVisibility);
                            SetCurrentWagonInfo();
                        }
                        break;
                    }
                case 2:
                    {
                        var currentIndex = Lux.IndexOf(CurrentLux);
                        if (SetPrev(Lux.Count, currentIndex))
                        {
                            CurrentLux = Lux[--currentIndex];
                            SetWagonSeatsButtonsVisibility(CurrentLux, LuxSeatsVisibility);
                            SetCurrentWagonInfo();
                        }
                        break;
                    }
            }
        }

        private bool SetNext(int count, int currentIndex)
        {
            if (count == 1)
            {
                PrevEnabled = false;
                NextEnabled = false;
            }
            if (count - 1 > currentIndex)
            {
                PrevEnabled = true;
                if (currentIndex == count - 2)
                {
                    NextEnabled = false;
                }

                return true;
            }
            return false;
        }

        private bool SetPrev(int count, int currentIndex)
        {
            if (count == 1)
            {
                PrevEnabled = false;
                NextEnabled = false;
            }
            if (currentIndex != 0)
            {
                if (currentIndex == count - 1)
                    NextEnabled = true;
                PrevEnabled = true;
                if (currentIndex == 1)
                {
                    PrevEnabled = false;
                }
                return true;
            }
            else { PrevEnabled = false; }
            return false;
        }

        private void SetCurrentWagonType()
        {
            
            switch (CurrentTabIndex)
            {
                case 0: 
                    {
                        _currentWagonType = WagonType.BERTH;
                        if (Berth.Count != 0)
                        {
                            CurrentBerth = Berth[0];
                            SetStartNextPrevButtons(Berth.Count);
                        }
                        break;
                    }
                case 1: 
                    {
                        _currentWagonType = WagonType.COUPE;
                        if (Coupe.Count != 0)
                        {
                            CurrentCoupe = Coupe[0];
                            SetStartNextPrevButtons(Coupe.Count);
                        }
                        break;
                    }
                case 2: 
                    {
                        if (Lux.Count != 0)
                        {
                            CurrentLux = Lux[0];
                            SetStartNextPrevButtons(Lux.Count);
                        }
                        _currentWagonType = WagonType.LUX;
                        break;
                    }
            }

        }

        private void SetCurrentWagonInfo()
        {
            this.WagonInfo = "";
            this.WagonInfo += "Поезд № ";
            this.WagonInfo += ContextKeeper.Trains
                .Where(t => t.Id == trainId)
                .Select(t => t.TrainNum)
                .First() + "\n";
            this.WagonInfo += "Вагон № ";
            switch (_currentWagonType)
            {
                case WagonType.BERTH:
                    {
                        this.WagonInfo += ContextKeeper.Wagons
                            .Where(w => w.Id == CurrentBerth.WagonId)
                            .Select(w => w.WagonNum)
                            .First();
                        break;
                    }
                case WagonType.COUPE:
                    {
                        this.WagonInfo += ContextKeeper.Wagons
                            .Where(w => w.Id == CurrentCoupe.WagonId)
                            .Select(w => w.WagonNum)
                            .First();
                        break;
                    }
                case WagonType.LUX:
                    {
                        this.WagonInfo += ContextKeeper.Wagons
                            .Where(w => w.Id == CurrentLux.WagonId)
                            .Select(w => w.WagonNum)
                            .First();
                        break;
                    }
            }

        }

        private void SetStartNextPrevButtons(int wagonsCount)
        {
            PrevEnabled = false;
            NextEnabled = (wagonsCount > 1) ? true : false;
        }


    }
}
