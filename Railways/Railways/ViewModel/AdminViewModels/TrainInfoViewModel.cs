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
    public class TrainInfoViewModel : ViewModelBase
    {
        private String _trainNum;
        private String _selectedWagonType;
        private Train _trainToEdit;
        private ObservableCollection<Wagon> _obsWagonList;

        public String SelectedWagonType
        {
            get
            {
                return _selectedWagonType;
            }
            set
            {
                _selectedWagonType = value;
                RaisePropertyChanged("SelectedWagonType");
            }
        }
        public String TrainNum
        {
            get 
            {
                return _trainNum;
            }
            set
            {
                _trainNum = value;
                RaisePropertyChanged("TrainNum");
            }
        }
        public ObservableCollection<Wagon> WagonList
        {
            get { return _obsWagonList; }
            set { _obsWagonList = value; }
        }
        public RelayCommand AddWagonCmd { get; private set; }
        public RelayCommand DeleteWagonCmd { get; private set; }
        public RelayCommand<TrainInfoWindow> SaveTrainInfoCmd { get; private set; }
        public RelayCommand<TrainInfoWindow> EditVoyageCmd { get; private set; }

        public TrainInfoViewModel()
        {
            SaveTrainInfoCmd = new RelayCommand<TrainInfoWindow>(this.SaveTrainInfo);

            SelectedWagonType = "0";

            ContextKeeper.Initialize();
            _obsWagonList = new ObservableCollection<Wagon>();

            AddWagonCmd = new RelayCommand(() => AddWagon());
            DeleteWagonCmd = new RelayCommand(() => DeleteWagon());
            EditVoyageCmd = new RelayCommand<TrainInfoWindow>(this.EditVoyage);

            Messenger.Default.Register<TrainInfoMessage>(this, (msg) =>
            {
                SetWagonInfo(msg.TrainId);
            });

        }

        /// <summary>
        /// Установка данных текущего поезда
        /// </summary>
        /// <param name="trainId"></param>
        public void SetWagonInfo(int trainId)
        {
            this._trainToEdit = ContextKeeper.Trains.First(train => train.Id == trainId);
            this.TrainNum = _trainToEdit.TrainNum;
            RefreshWagonsList();
        }

        /// <summary>
        /// Обновление списка вагонов поезда
        /// </summary>
        public void RefreshWagonsList()
        {
            this.WagonList.Clear();
            var wagons = TrainBuilder.GetWagonsOfTrain(_trainToEdit.Id);
            wagons.ToList().ForEach(wagon => this.WagonList.Add(wagon));
        }
        
        /// <summary>
        /// Добавление вагона к поезду
        /// </summary>
        /// <returns></returns>
        public async Task AddWagon() 
        {
            if (_trainToEdit == null)
            {
                _trainToEdit = new Train();
                _trainToEdit.TrainNum = TrainNum;
                ContextKeeper.Trains.Add(_trainToEdit);
            }
            System.Console.WriteLine(SelectedWagonType);
            WagonType wType = (WagonType)int.Parse(SelectedWagonType);
            try
            {
                await TrainBuilder.AddWagonToTrain(_trainToEdit.Id, wType);
                RefreshWagonsList();
            }
            catch (Exception)
            {
                Console.WriteLine("Жди, пока добавится предыдущий");
            }

        }
        
        /// <summary>
        /// Удаление вагона из поезда
        /// </summary>
        private void DeleteWagon() 
        {
            if (WagonList.Count != 0 && _trainToEdit != null)
            {
                TrainBuilder.DeleteLastWagonFromTrain(_trainToEdit.Id);
                RefreshWagonsList();
            }
        }

        /// <summary>
        /// Сохранение изменений информации о поезде
        /// </summary>
        /// <param name="window"></param>
        private void SaveTrainInfo(TrainInfoWindow window) 
        {
            if (_trainToEdit == null)
            {
                _trainToEdit = new Train();
                _trainToEdit.TrainNum = TrainNum;
                ContextKeeper.Trains.Add(_trainToEdit);
            }
            _trainToEdit.TrainNum = TrainNum;
            ContextKeeper.Trains.Update(_trainToEdit);
            window.Close();
        }

        /// <summary>
        /// Отображение рейса текущего поезда для дальнейшего редактирования
        /// </summary>
        /// <param name="window"></param>
        private void EditVoyage(TrainInfoWindow window)
        {
            if (_trainToEdit == null) return;
            var voyage = VoyageBuilder.GetVoyageOfTrain(_trainToEdit.Id);
            if (voyage == null)
            {
                var newVoyage = new Voyage();
                newVoyage.DepartureDateTime = DateTime.Now;
                newVoyage.TrainId = _trainToEdit.Id;
                ContextKeeper.Voyages.Add(newVoyage);         
            }
            var voyageEditWin = new VoyageEditWindow();
            voyageEditWin.Show();
            Messenger.Default.Send(new TrainOfVoyageMessage(_trainToEdit.Id));
            window.Close();
        }

    }

}

