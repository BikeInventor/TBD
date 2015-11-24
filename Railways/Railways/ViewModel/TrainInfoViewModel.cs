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


namespace Railways.ViewModel
{
    class TrainInfoViewModel
    {
        private String _trainNum;
        public String TrainName { get; set; }

        private List<Wagon> _wagonList;

        private ObservableCollection<Wagon> _obsWagonList;

        private bool isEditMode = false;
        private Train _trainToEdit;

        public ObservableCollection<Wagon> WagonList
        {
            get { return _obsWagonList; }
            set { _obsWagonList = value; }
        }
        public int WagonSelectedIndex { get; set; }
        public RelayCommand AddWagonCmd { get; private set; }
        public RelayCommand DeleteWagonCmd { get; private set; }
        public RelayCommand SaveTrainInfoCmd { get; private set; }

        public TrainInfoViewModel()
        {
            ContextKeeper.Initialize();
            _wagonList = new List<Wagon>();
            _obsWagonList = new ObservableCollection<Wagon>();

            RefreshWagonList();
            AddWagonCmd = new RelayCommand(() => AddWagon());
            DeleteWagonCmd = new RelayCommand(() => DeleteWagon());

            SaveTrainInfoCmd = new RelayCommand(() => SaveTrainInfo());

            Messenger.Default.Register<SendTrainInfoMessage>(this, (msg) =>
            {
                SetWagonInfo(msg.TrainId);
            });

        }

        public void SetWagonInfo(int trainId)
        {
            this._trainToEdit = ContextKeeper.Trains.First(train => train.Id == trainId);
            isEditMode = true;
            this._trainNum = _trainToEdit.TrainNum;
        }

        public void RefreshWagonsList()
        {
            var wagons = TrainBuilder.GetWagonsOfTrain(_trainToEdit.Id);
            wagons.ToList().ForEach(wagon => this._wagonList.Add(wagon));
        }

        public void AddWagon() { }
        public void DeleteWagon() { }
        public void SaveTrainInfo() { }
        public void RefreshWagonList() { }

    }
}
