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
using Railways.ViewModel;
using Railways.ViewModel.Utils;
using System.Windows.Media;

namespace Railways.ViewModel
{
    public class SeatSetViewModel : ViewModelBase
    {
        private const String freeSeatOpacity = "0.3";
        private const String occupiedSeatOpacity = "1";
        private const Double selectedSeatOpacity = 0.5;
        private  Brush selectedSeatColor  = Brushes.LawnGreen;

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
                ResetSelectedButton();
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

        private String _seatInfo = "";
        public String SeatInfo
        {
            get
            {
                return _seatInfo;
            }
            set
            {
                _seatInfo = value;
                RaisePropertyChanged("SeatInfo");
            }
        }
      
        private bool _nextEnabled;
        private bool _prevEnabled;

        private Button _selectedSeatButton;
        private int _selectedSeatNumber;
        private Brush _selectedButtonColor;

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
        
        private ObservableRangeCollection<String> _berthSeatsVisibility;
        public ObservableRangeCollection<String> BerthSeatsVisibility
        {
            get { return _berthSeatsVisibility; }
            set { _berthSeatsVisibility = value; }
        }
        private ObservableRangeCollection<String> _coupeSeatsVisibility;
        public ObservableRangeCollection<String> CoupeSeatsVisibility
        {
            get { return _coupeSeatsVisibility; }
            set { _coupeSeatsVisibility = value; }
        }
        private ObservableRangeCollection<String> _luxSeatsVisibility;
        public ObservableRangeCollection<String> LuxSeatsVisibility
        {
            get { return _luxSeatsVisibility; }
            set { _luxSeatsVisibility = value; }
        }

        public RelayCommand<object> SelectSeatCmd { get; set; }
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

            _berthSeatsVisibility = new ObservableRangeCollection<String>();
            _coupeSeatsVisibility = new ObservableRangeCollection<String>();
            _luxSeatsVisibility = new ObservableRangeCollection<String>();

            SelectSeatCmd = new RelayCommand<object>(this.SelectSeat);
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
                var newWag = new WagonSeatsSet(wagon.Id, depDate, arrDate);
                if (newWag.Seats.Any(freeSeat => freeSeat == true))
                {
                    var wagonType = (WagonType)ContextKeeper.Wagons.Where(w => w.Id == wagon.Id).Select(w => w.WagonType.Value).First();
                    switch (wagonType)
                    {
                        case WagonType.BERTH:
                            {

                                this.Berth.Add(newWag);
                                break;
                            }
                        case WagonType.COUPE:
                            {
                                this.Coupe.Add(newWag);
                                break;
                            }
                        case WagonType.LUX:
                            {
                                this.Lux.Add(newWag);
                                break;
                            }
                    }

                }
               
            });
            SetCurrentWagons();

             SetWagonSeatsButtonsVisibility(CurrentBerth, _berthSeatsVisibility);
             BerthSeatsVisibility.Add(null);
            //SetWagonSeatsButtonsVisibility(CurrentCoupe, CoupeSeatsVisibility);
            //SetWagonSeatsButtonsVisibility(CurrentLux, LuxSeatsVisibility);

            NextEnabled = true;  
            CurrentTabIndex = 0;
            SetCurrentWagonInfo();
        }

        /// <summary>
        /// Установка видимости для кнопок, сответствующим местам в вагоне
        /// Видимая - если место занято,
        /// </summary>
        /// <param name="seats"></param>
        /// <param name="visibility"></param>
        private void SetWagonSeatsButtonsVisibility(WagonSeatsSet seats, ObservableRangeCollection<String> visibility)
        {
            visibility.Clear();

            visibility.AddRange(seats.Seats.Select(seatIsFree => 
            {
                return seatIsFree ? freeSeatOpacity : occupiedSeatOpacity;
            }));
        }

        /// <summary>
        /// Установка текущего вагона для каждой вкладки при их переключении
        /// </summary>
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

        /// <summary>
        /// Переход к следующему вагону на вкладке
        /// </summary>
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
                            BerthSeatsVisibility.Add(null);
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
                            SetWagonSeatsButtonsVisibility(CurrentCoupe, CoupeSeatsVisibility);
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
                            SetWagonSeatsButtonsVisibility(CurrentLux, LuxSeatsVisibility);
                            SetCurrentWagonInfo();
                        }
                        break;
                    }
                }
            ResetSelectedButton();
        }
           
        /// <summary>
        /// Переход к предыдущему вагону на вкладке
        /// </summary>
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
                            SetWagonSeatsButtonsVisibility(CurrentBerth, _berthSeatsVisibility);
                            BerthSeatsVisibility.Add(null);
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
                            SetWagonSeatsButtonsVisibility(CurrentCoupe, _coupeSeatsVisibility);
                            CoupeSeatsVisibility.Add(null);
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
                            SetWagonSeatsButtonsVisibility(CurrentLux, _luxSeatsVisibility);
                            LuxSeatsVisibility.Add(null);
                            SetCurrentWagonInfo();
                        }
                        break;
                    }
            }
            ResetSelectedButton();
        }

        /// <summary>
        /// Установка доступности кнопки перехода к следующему вагону на вкладке
        /// </summary>
        /// <param name="count"></param>
        /// <param name="currentIndex"></param>
        /// <returns>Возвращает true, если переход осуществлён</returns>
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
        
        /// <summary>
        /// Установка доступности кнопки перехода к предыдущему вагону на вкладке
        /// </summary>
        /// <param name="count"></param>
        /// <param name="currentIndex"></param>
        /// <returns>Возвращает true, если переход осуществлён</returns>
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

        /// <summary>
        /// Установка текущего типа вагонов поезда в зависимости от выбранной вкладки
        /// </summary>
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
                            SetWagonSeatsButtonsVisibility(CurrentBerth, _berthSeatsVisibility);
                            BerthSeatsVisibility.Add(null);
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
                            SetWagonSeatsButtonsVisibility(CurrentCoupe, _coupeSeatsVisibility);
                            CoupeSeatsVisibility.Add(null);
                        }
                        break;
                    }
                case 2: 
                    {
                        if (Lux.Count != 0)
                        {
                            CurrentLux = Lux[0];
                            SetStartNextPrevButtons(Lux.Count);
                            SetWagonSeatsButtonsVisibility(CurrentLux, _luxSeatsVisibility);
                            LuxSeatsVisibility.Add(null);
                        }
                        _currentWagonType = WagonType.LUX;
                        break;
                    }
            }

        }

        /// <summary>
        /// Отображение на форме информации о текущем вагоне
        /// </summary>
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

        /// <summary>
        /// Установка кнопок перехода между вагонами вкладки в начальное состояник
        /// </summary>
        /// <param name="wagonsCount"></param>
        private void SetStartNextPrevButtons(int wagonsCount)
        {
            PrevEnabled = false;
            NextEnabled = (wagonsCount > 1) ? true : false;
        }

        /// <summary>
        /// Выбор места для покупки билета
        /// </summary>
        /// <param name="button"></param>
        private void SelectSeat(object button)
        {
            var selectedSeatButton = (Button)button;
            var selectedSeatIndex = int.Parse(selectedSeatButton.Name
                .Substring(1, selectedSeatButton.Name.Length - 1)) - 1;

            switch (_currentTabIndex)
            {
                case 0:
                    {
                        if (CurrentBerth.Seats[selectedSeatIndex] == true 
                            && selectedSeatButton != this._selectedSeatButton)
                        {
                            ShowSeatSelection(selectedSeatButton, selectedSeatIndex);
                        }
                        break;
                    }
                case 1:
                    {
                        if (CurrentCoupe.Seats[selectedSeatIndex] == true
                            && selectedSeatButton != this._selectedSeatButton)
                        {
                            ShowSeatSelection(selectedSeatButton, selectedSeatIndex);
                        }
                        break;
                    }
                case 2:
                    {
                        if (CurrentLux.Seats[selectedSeatIndex] == true
                            && selectedSeatButton != this._selectedSeatButton)
                        {
                            ShowSeatSelection(selectedSeatButton, selectedSeatIndex);
                        }
                        break;
                    }
            }

        }

        private void ShowSeatSelection(Button selectedSeatButton, int selectedSeatIndex)
        {
            if (this._selectedSeatButton != null)
            {
                this._selectedSeatButton.Background = this._selectedButtonColor;
            }

            this._selectedButtonColor = selectedSeatButton.Background;
            this._selectedSeatButton = selectedSeatButton;
            this._selectedSeatButton.Background = selectedSeatColor;
            this._selectedSeatButton.Opacity = selectedSeatOpacity;
            this._selectedSeatNumber = selectedSeatIndex;
            this.SeatInfo = "Место № " + this._selectedSeatNumber + 1;
        }

        private void ResetSelectedButton()
        {
            if (_selectedSeatButton != null)
            {
                this._selectedSeatButton.Background = this._selectedButtonColor;
                this._selectedSeatNumber = -1;
                this.SeatInfo = "";
            }

        }
    }
}
