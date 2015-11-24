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

namespace Railways.ViewModel
{
    public class AdminViewModel : ViewModelBase
    {
        private List<Employee> _employeeList;
        private List<Train> _trainList;

        private ObservableCollection<Train> _obsTrainList;

        private ObservableCollection<Employee> _obsEmpList;
        public int EmployeeSelectedIndex { get; set; }
        public int TrainSelectedIndex { get; set; }
        public RelayCommand RegisterEmployeeCmd
        {
            get;
            private set;
        }

        public RelayCommand DeleteEmployeeCmd
        {
            get;
            private set;
        }
        public RelayCommand RegisterTrainCmd
        {
            get;
            private set;
        }
        public RelayCommand DeleteTrainCmd
        {
            get;
            private set;
        }
        public RelayCommand EditTrainCmd
        {
            get;
            private set;
        }

        public ObservableCollection<Employee> EmployeeList
        {
            get
            {
                return _obsEmpList;
            }
            set
            {
                _obsEmpList = value;
            }
        }
        public ObservableCollection<Train> TrainList
        {
            get { return _obsTrainList; }
            set { _obsTrainList = value; }
        }

        public AdminViewModel()
        {
            ContextKeeper.Initialize();

            _obsEmpList = new ObservableCollection<Employee>();
            _employeeList = new List<Employee>();

            _obsTrainList = new ObservableCollection<Train>();
            _trainList = new List<Train>();

            RefreshEmployeeList();
            RefreshTrainList();

            RegisterEmployeeCmd = new RelayCommand(() => AddEmployee());                   
            DeleteEmployeeCmd = new RelayCommand(() => DeleteEmployee());

            RegisterTrainCmd = new RelayCommand(() => AddTrain());
            DeleteTrainCmd = new RelayCommand(() => DeleteTrain());
            EditTrainCmd = new RelayCommand(() => EditTrain());

            Messenger.Default.Register<RefreshEmployeeListMessage>(this, (msg) =>
            {
                RefreshEmployeeList();
            });

            Messenger.Default.Register<RefreshTrainListMessage>(this, (msg) =>
            {
                RefreshTrainList();
            });
        }

        public void RefreshEmployeeList()
        {
            _employeeList = ContextKeeper.Employees.All().ToList();
            _obsEmpList.Clear();
            _employeeList.ForEach(emp => _obsEmpList.Add(emp));
        }

        private void AddEmployee()
        {
            var empInfo = new EmployeeInfoWindow(); 
            empInfo.Show();
        }

        private void DeleteEmployee()
        {
            if (EmployeeSelectedIndex >= 0)
            {
                var empToDelete = EmployeeList[EmployeeSelectedIndex];
                ContextKeeper.Employees.Remove(empToDelete);
                RefreshEmployeeList();
            }
        }

        public void RefreshTrainList()
        {
            _trainList = ContextKeeper.Trains.All().ToList();
            _obsTrainList.Clear();
            _trainList.ForEach(train => _obsTrainList.Add(train));
        }

        private void AddTrain()
        {
            //var trainInfo = new EmployeeInfoWindow();
            //trainInfo.Show();
            for (int i = 0; i < 20; i++)
            {
                var newTrain = new Train();
                newTrain.TrainNum = "1" + i.ToString() + "C";
                ContextKeeper.Trains.Add(newTrain);
            }
            RefreshTrainList();
        }

        private void DeleteTrain()
        {
            if (TrainSelectedIndex >= 0)
            {
                var trainToDelete = TrainList[TrainSelectedIndex];
                ContextKeeper.Trains.Remove(trainToDelete);
                RefreshTrainList();
            }
        }
        private void EditTrain() 
        {
            var selectedTrainId = TrainList[TrainSelectedIndex].Id;
            var trainInfoWin = new TrainInfoWindow();
            trainInfoWin.Show();
            Messenger.Default.Send(new SendTrainInfoMessage(selectedTrainId));
        }
    }
}
