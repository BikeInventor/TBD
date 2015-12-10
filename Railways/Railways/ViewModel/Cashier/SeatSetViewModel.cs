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
using Railways.ViewModel.Services;

namespace Railways.ViewModel
{
    public class SeatSetViewModel : ViewModelBase
    {
        #region Constants
        private const String freeSeatOpacity = "0.3";
        private const String occupiedSeatOpacity = "1";
        private const Double selectedSeatOpacity = 0.5;
        private  Brush selectedSeatColor  = Brushes.LawnGreen;
        #endregion

        #region Properties
        private TripInfo _tripInfo;
        private double _currentSeatPrice;

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

        private String _currentTripInfo;
        public String CurrentTripInfo
        {
            get 
            {
                return _currentTripInfo;
            }
            set
            {
                _currentTripInfo = value;
                RaisePropertyChanged("CurrentTripInfo");
            }
        }
      
        private bool _nextEnabled;
        private bool _prevEnabled;

        private bool _berthAvailability;
        private bool _coupeAvailability;
        private bool _luxAvailability;

        public bool BerthAvailability
        {
            get
            {
                return _berthAvailability;
            }
            set
            {
                _berthAvailability = value;
                RaisePropertyChanged("BerthAvailability");
            }
        }
        public bool CoupeAvailability
        {
            get
            {
                return _coupeAvailability;
            }
            set
            {
                _coupeAvailability = value;
                RaisePropertyChanged("CoupeAvailability");
            }
        }
        public bool LuxAvailability
        {
            get
            {
                return _luxAvailability;
            }
            set
            {
                _luxAvailability = value;
                RaisePropertyChanged("LuxAvailability");
            }
        }

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
        #endregion

        #region Commands
        public RelayCommand<object> SelectSeatCmd { get; set; }
        public RelayCommand NextWagonCmd { get; set; }
        public RelayCommand PrevWagonCmd { get; set; }
        public RelayCommand ClientInfoInputCmd { get; set; }
        #endregion

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
            ClientInfoInputCmd = new RelayCommand(() => ClientInfoInput());

            Messenger.Default.Register<TripInfoMessage>(this, (msg) =>
            {
                this._tripInfo = msg.CurrentTripInfo;
                SetTrainInfo();
                SetCurrentTripInfo();
            });
        }

        /// <summary>
        /// Формирование списков вагонов поезда по их типам
        /// </summary>
        /// <param name="trainId"></param>
        private async void SetTrainInfo()
        {
            await Task.Run(() => 
            {
                this.trainId = this._tripInfo.TrainId; ;
                this.depDate = this._tripInfo.DepartureTime;
                this.arrDate = this._tripInfo.ArrivalTime;

                Berth.Clear();
                Coupe.Clear();
                Lux.Clear();

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
                SetCurrentTab();
                NextEnabled = true;
                SetCurrentWagonInfo();
            });
           
        }

        /// <summary>
        /// Установка начальной вкладки, в зависимости от доступности мест конкретного типа
        /// </summary>
        private void SetCurrentTab()
        {
            BerthAvailability = true;
            CoupeAvailability = true;
            LuxAvailability = true;

            if (Lux.Count > 0) _currentTabIndex = 2;
            else LuxAvailability = false;

            if (Coupe.Count > 0) _currentTabIndex = 1;
            else CoupeAvailability = false;

            if (Berth.Count > 0) _currentTabIndex = 0;
            else BerthAvailability = false;

            switch (CurrentTabIndex)
            {
                case 0:
                    {
                        SetWagonSeatsButtonsVisibility(CurrentBerth, _berthSeatsVisibility);
                        BerthSeatsVisibility.Add(null);
                        SetStartNextPrevButtons(Berth.Count);
                        break;
                    }
                case 1:
                    {
                        SetWagonSeatsButtonsVisibility(CurrentCoupe, _coupeSeatsVisibility);
                        CoupeSeatsVisibility.Add(null);
                        SetStartNextPrevButtons(Coupe.Count);
                        break;
                    }
                case 2:
                    {
                        SetWagonSeatsButtonsVisibility(CurrentLux, _luxSeatsVisibility);
                        LuxSeatsVisibility.Add(null);
                        SetStartNextPrevButtons(Lux.Count);
                        break;
                    }
            }

            RaisePropertyChanged("CurrentTabIndex");
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
                            SetWagonSeatsButtonsVisibility(CurrentCoupe, _coupeSeatsVisibility);
                            CoupeSeatsVisibility.Add(null);
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
                            LuxSeatsVisibility.Add(null);
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
                        if (Berth.Count != 0)
                        {
                            this.WagonInfo += ContextKeeper.Wagons
                                .Where(w => w.Id == CurrentBerth.WagonId)
                                .Select(w => w.WagonNum)
                                .First();
                        }
                        break;
                    }
                case WagonType.COUPE:
                    {
                        if (Coupe.Count != 0)
                        {
                            this.WagonInfo += ContextKeeper.Wagons
                                .Where(w => w.Id == CurrentCoupe.WagonId)
                                .Select(w => w.WagonNum)
                                .First();
                        }
                        break;

                    }
                case WagonType.LUX:
                    {
                        if (Lux.Count != 0)
                        {
                            this.WagonInfo += ContextKeeper.Wagons
                                .Where(w => w.Id == CurrentLux.WagonId)
                                .Select(w => w.WagonNum)
                                .First();
                        }
                        break;
                    }
            }

        }

        /// <summary>
        /// Установка кнопок перехода между вагонами вкладки в начальное состояние
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
                        if (CurrentBerth.Seats[selectedSeatIndex] == true)
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

        /// <summary>
        /// Отображение выбранного места в вагоне
        /// </summary>
        /// <param name="selectedSeatButton"></param>
        /// <param name="selectedSeatIndex"></param>
        private void ShowSeatSelection(Button selectedSeatButton, int selectedSeatIndex)
        {
            if (this._selectedSeatButton != null)
            {
                this._selectedSeatButton.Background = this._selectedButtonColor;
            }

            this._selectedButtonColor = selectedSeatButton.Background;
            this._selectedSeatButton = selectedSeatButton;
            this._selectedSeatButton.Background = selectedSeatColor;
            this._selectedSeatNumber = selectedSeatIndex;
            this.SeatInfo = "Место № " + (this._selectedSeatNumber + 1);

            switch (_currentTabIndex)
            {
                case 0:
                    {
                        _currentSeatPrice = this._tripInfo.BerthPrice;
                        if (_selectedSeatNumber % 2 != 0)
                        {
                            _currentSeatPrice = BusinessLogic.SetUpperSeatPrice(_currentSeatPrice);
                        }
                        else
                        {
                            _currentSeatPrice = BusinessLogic.SetLowerSeatPrice(_currentSeatPrice);
                        }
                        break;
                    }
                case 1:
                    {
                        _currentSeatPrice = this._tripInfo.CoupePrice;
                        if (_selectedSeatNumber % 2 != 0)
                        {
                            _currentSeatPrice = BusinessLogic.SetUpperSeatPrice(_currentSeatPrice);
                        }
                        else
                        {
                            _currentSeatPrice = BusinessLogic.SetLowerSeatPrice(_currentSeatPrice);
                        }
                        break;
                    }
                case 2:
                    {
                        _currentSeatPrice = this._tripInfo.LuxPrice;
                        break;
                    }
            }

            this.SeatInfo += "                   Цена билета: "
                + String.Format("{0:0.00}", this._currentSeatPrice)
                + "руб.";
        }

        /// <summary>
        /// Сброс выбранного места при переходе между вкладками/вагонами
        /// </summary>
        private void ResetSelectedButton()
        {
            try
            {
                if (_selectedSeatButton != null)
                {
                    this._selectedSeatButton.Background = this._selectedButtonColor;
                    this._selectedSeatNumber = -1;
                    this.SeatInfo = "";
                }
            }
            catch
            {
                Console.WriteLine("Err while resetting selected button");
            }

        }

        /// <summary>
        /// Задание содержания надписи о поездке, на которую покупается билет
        /// </summary>
        /// <param name="stationFrom"></param>
        /// <param name="stationTo"></param>
        /// <param name="depDate"></param>
        /// <param name="arrDate"></param>
        private void SetCurrentTripInfo()
        {
            CurrentTripInfo = "Отправка: "
                + this._tripInfo.DepartureStation + ", "
                + this._tripInfo.DepartureTime.ToLongDateString() + " в "
                + this._tripInfo.DepartureTime.ToShortTimeString() + "\n" 
                + "Прибытие: "
                + this._tripInfo.ArrivalStation + ", " 
                + this._tripInfo.ArrivalTime.ToLongDateString() + " в "
                + this._tripInfo.ArrivalTime.ToShortTimeString();
        }

        /// <summary>
        /// Открытие окна ввода данных о покупателе
        /// </summary>
        private void ClientInfoInput()
        {
            if (_selectedSeatNumber >= 0)
            {
                 var wagons = TrainBuilder.GetWagonsOfTrain(trainId);

                 int currentWag = 0;

                 switch (_currentTabIndex)
                 {
                     case 0:
                         {
                             currentWag = wagons
                                 .Where(w => w.WagonType == (int)WagonType.BERTH)
                                 .Where(w => w.Id == CurrentBerth.WagonId)
                                 .Select(w => w.Id)
                                 .First();
                             break;
                         }
                     case 1:
                         {
                             currentWag = wagons
                                 .Where(w => w.WagonType == (int)WagonType.COUPE)
                                 .Where(w => w.Id == CurrentCoupe.WagonId)
                                 .Select(w => w.Id)
                                 .First();
                             break;
                         }
                     case 2:
                         {
                             currentWag = wagons
                                 .Where(w => w.WagonType == (int)WagonType.LUX)
                                 .Where(w => w.Id == CurrentLux.WagonId)
                                 .Select(w => w.Id)
                                 .First();
                             break;
                         }
                 }
                

                 var seatsOfWagon = TrainBuilder.GetSeatsOfWagon(currentWag).ToList();
                 var selectedSeatId = seatsOfWagon[_selectedSeatNumber].Id;

                var clientInfo = new ClientInfoWindow();
                clientInfo.Show();
                clientInfo.Closing += new System.ComponentModel.CancelEventHandler((a, b) =>
                {
                    UpdateWagonSeats();
                });


                Messenger.Default.Send(new TicketInfoMessage(
                    this._tripInfo,
                    selectedSeatId,
                    AuthorizationService.CurrentEmployeeId,
                    0,
                    _currentSeatPrice
                ));
            }
        }

        /// <summary>
        /// Обновление информации о свободных местах в данном вагоне после покупки билета
        /// </summary>
        private void UpdateWagonSeats()
        {
            var wagonsOfTrain = TrainBuilder.GetWagonsOfTrain(trainId).ToList();

            switch (_currentTabIndex)
            {
                case 0:
                    {
                        var currentBerthIndex = Berth.IndexOf(CurrentBerth);
                        Berth.Clear();
                        wagonsOfTrain.ForEach(wagon =>
                        {
                            var newWag = new WagonSeatsSet(wagon.Id, depDate, arrDate);
                            var wagonType = (WagonType)ContextKeeper.Wagons.Where(w => w.Id == wagon.Id).Select(w => w.WagonType.Value).First();
                            if (wagonType == WagonType.BERTH)
                            {
                                this.Berth.Add(newWag);
                            }
                        });
                        CurrentBerth = Berth[currentBerthIndex];
                        SetWagonSeatsButtonsVisibility(CurrentBerth, _berthSeatsVisibility);
                        BerthSeatsVisibility.Add(null);
                        break;
                    }
                case 1:
                    {
                        var currentCoupeIndex = Coupe.IndexOf(CurrentCoupe);
                        Coupe.Clear();
                        wagonsOfTrain.ForEach(wagon =>
                        {
                            var newWag = new WagonSeatsSet(wagon.Id, depDate, arrDate);
                            var wagonType = (WagonType)ContextKeeper.Wagons.Where(w => w.Id == wagon.Id).Select(w => w.WagonType.Value).First();
                            if (wagonType == WagonType.COUPE)
                            {
                                this.Coupe.Add(newWag);
                            }
                        });
                        CurrentCoupe = Coupe[currentCoupeIndex];
                        SetWagonSeatsButtonsVisibility(CurrentCoupe, _coupeSeatsVisibility);
                        CoupeSeatsVisibility.Add(null);
                        break;
                    }
                case 2:
                    {
                        var currentLuxIndex = Lux.IndexOf(CurrentLux);
                        Lux.Clear();
                        wagonsOfTrain.ForEach(wagon =>
                        {
                            var newWag = new WagonSeatsSet(wagon.Id, depDate, arrDate);
                            var wagonType = (WagonType)ContextKeeper.Wagons.Where(w => w.Id == wagon.Id).Select(w => w.WagonType.Value).First();
                            if (wagonType == WagonType.LUX)
                            {
                                this.Lux.Add(newWag);
                            }
                        });
                        CurrentLux = Lux[currentLuxIndex];
                        SetWagonSeatsButtonsVisibility(CurrentLux, _luxSeatsVisibility);
                        LuxSeatsVisibility.Add(null);
                        break;
                    }
            }
            ResetSelectedButton();
        }
    }
}
