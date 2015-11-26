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

        private List<Wagon> _wagonList;

        private ObservableCollection<Wagon> _obsWagonList;

        private bool isEditMode = false;
        private Train _trainToEdit;

        public ObservableCollection<Wagon> WagonList
        {
            get { return _obsWagonList; }
            set { _obsWagonList = value; }
        }
        public RelayCommand AddWagonCmd { get; private set; }
        public RelayCommand DeleteWagonCmd { get; private set; }
        public RelayCommand<TrainInfoWindow> SaveTrainInfoCmd { get; private set; }
        public RelayCommand EditVoyageCmd { get; private set; }

        public TrainInfoViewModel()
        {
            SaveTrainInfoCmd = new RelayCommand<TrainInfoWindow>(this.SaveTrainInfo);

            SelectedWagonType = "0";

            ContextKeeper.Initialize();
            _wagonList = new List<Wagon>();
            _obsWagonList = new ObservableCollection<Wagon>();

            AddWagonCmd = new RelayCommand(() => AddWagon());
            DeleteWagonCmd = new RelayCommand(() => DeleteWagon());
            EditVoyageCmd = new RelayCommand(() => EditVoyage());

            Messenger.Default.Register<SendTrainInfoMessage>(this, (msg) =>
            {
                SetWagonInfo(msg.TrainId);
            });

        }

        public void SetWagonInfo(int trainId)
        {
            this._trainToEdit = ContextKeeper.Trains.First(train => train.Id == trainId);
            isEditMode = true;
            this.TrainNum = _trainToEdit.TrainNum;
            RefreshWagonsList();
        }
        public void RefreshWagonsList()
        {
            this.WagonList.Clear();
            var wagons = TrainBuilder.GetWagonsOfTrain(_trainToEdit.Id);
            wagons.ToList().ForEach(wagon => this.WagonList.Add(wagon));
        }
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
        private void DeleteWagon() 
        {
            if (WagonList.Count != 0 && _trainToEdit != null)
            {
                TrainBuilder.DeleteLastWagonFromTrain(_trainToEdit.Id);
                RefreshWagonsList();
            }
        }
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
        private void EditVoyage()
        {
            if (_trainToEdit == null) return;
            var voyage = VoyageBuilder.GetVoyageOfTrain(_trainToEdit.Id);
            if (voyage == null)
            {
                var newVoyage = new Voyage();
                ContextKeeper.Voyages.Add(newVoyage);
                VoyageBuilder.ConnectVoyageToTrain(newVoyage.Id, _trainToEdit.Id);             
            }
            var voyageEditWin = new VoyageEditWindow();
            voyageEditWin.Show();
            Messenger.Default.Send(new SendTrainInfoMessage(_trainToEdit.Id));
        }

    }

}

