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
using Railways.ViewModel.Services;

namespace Railways.ViewModel
{
    public class AdminViewModel : ViewModelBase
    {
        private List<Employee> _employeeList;
        private List<Train> _trainList;

        private ObservableCollection<Train> _obsTrainList;

        private ObservableCollection<Employee> _obsEmpList;
        public int EmployeeSelectedIndex { get; set; }
        
        private int _trainSelectedIndex;
        /// <summary>
        /// Индекс выделенного в списке поезда
        /// </summary>
        public int TrainSelectedIndex {
            get
            {
                return _trainSelectedIndex;
            }
            set 
            {
                _trainSelectedIndex = value;
                RaisePropertyChanged("TrainSelectedIndex");
            } 
        }
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
        public RelayCommand<object> EmployeeSearchStringChangedCmd
        {
            get;
            private set;
        }
        public RelayCommand<object> TrainSearchStringChangedCmd
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
            _obsEmpList = new ObservableCollection<Employee>();
            _employeeList = new List<Employee>();

            _obsTrainList = new ObservableCollection<Train>();
            _trainList = new List<Train>();

            RefreshEmployeeList();
            RefreshTrainList();

            RegisterEmployeeCmd = new RelayCommand(() => AddEmployee());                   
            DeleteEmployeeCmd = new RelayCommand(() => DeleteEmployee());
            EmployeeSearchStringChangedCmd = new RelayCommand<object>(this.FilterEmployeeList);
            TrainSearchStringChangedCmd = new RelayCommand<object>(this.FilterTrainList);

            RegisterTrainCmd = new RelayCommand(() => AddTrain());
            DeleteTrainCmd = new RelayCommand(() => DeleteTrain());
            EditTrainCmd = new RelayCommand(() => EditTrain());

        }

        /// <summary>
        /// Обновление списка сотрудников
        /// </summary>
        public void RefreshEmployeeList()
        {
            _employeeList = ContextKeeper.Employees.All().ToList();
            _obsEmpList.Clear();
            _employeeList.ForEach(emp => _obsEmpList.Add(emp));
        }

        /// <summary>
        /// Добавление сотрудника
        /// </summary>
        private void AddEmployee()
        {
            var empInfo = new EmployeeInfoWindow(); 
            empInfo.Show();
            empInfo.Closing += new System.ComponentModel.CancelEventHandler((a, b) => RefreshEmployeeList());
        }

        /// <summary>
        /// Удаление сотрудника из списка
        /// </summary>
        private async void DeleteEmployee()
        {
            if (EmployeeSelectedIndex >= 0)
            {
                var empToDelete = EmployeeList[EmployeeSelectedIndex];

                var dialogResult = await DialogService.ShowDialog("AdminWindow",
                        "Вы уверены, что хотите удалить\n"
                        + empToDelete.FullName + "?",
                        DialogWindowType.OPTIONDIALOG);

                if (dialogResult)
                {
                    ContextKeeper.Employees.Remove(empToDelete);
                    RefreshEmployeeList();
                }

            }
        }
        /// <summary>
        /// Обновление списка поездов
        /// </summary>
        public void RefreshTrainList()
        {
            _trainList = ContextKeeper.Trains.All().ToList();
            _obsTrainList.Clear();
            _trainList.ForEach(train => _obsTrainList.Add(train));
        }
        /// <summary>
        /// Добавление поезда
        /// </summary>
        private void AddTrain()
        {
            var trainInfoWin = new TrainInfoWindow();
            trainInfoWin.Show();
            trainInfoWin.Closing += new System.ComponentModel.CancelEventHandler((a,b) => RefreshTrainList());
        }
        /// <summary>
        /// Удаление поезда из списка
        /// </summary>
        private async void DeleteTrain()
        {
            if (TrainSelectedIndex >= 0)
            {
                var trainToDelete = TrainList[TrainSelectedIndex];
                TrainSelectedIndex = -1;

                var dialogResult = await DialogService.ShowDialog("AdminWindow",
                        "Вы уверены, что хотите удалить\nпоезд №"
                        + trainToDelete.TrainNum + "?",
                        DialogWindowType.OPTIONDIALOG);

                if (dialogResult)
                {
                    TrainBuilder.DeleteTrainWithWagons(trainToDelete.Id);
                    RefreshTrainList();
                }
            }
        }
        /// <summary>
        /// Открытие окна с данными конкретного поезда для их редактирования
        /// </summary>
        private void EditTrain() 
        {
            if (TrainSelectedIndex >= 0)
            {
                var selectedTrainId = TrainList[TrainSelectedIndex].Id;
                var trainInfoWin = new TrainInfoWindow();
                trainInfoWin.Show();
                trainInfoWin.Closing += new System.ComponentModel.CancelEventHandler((a, b) => RefreshTrainList());
                Messenger.Default.Send(new TrainInfoMessage(selectedTrainId));
            }         
        }

        /// <summary>
        /// Фильтрация списка сотрудников в соответствии с заданными условиями
        /// </summary>
        /// <param name="searchTextBox"></param>
        public void FilterEmployeeList(object searchTextBox)
        {
            var textBox = searchTextBox as TextBox;
            var searchString = textBox.Text;

            if (searchString != null && searchString != "")
            {
                _employeeList = ContextKeeper.Employees
                    .Where(emp => emp.FullName.Contains(searchString) 
                        || emp.Id.ToString().Contains(searchString))
                .ToList();
                _obsEmpList.Clear();
                _employeeList.ForEach(emp => _obsEmpList.Add(emp));
            }
            else
            {
                RefreshEmployeeList();
            }
        }
        /// <summary>
        /// Фильтрация списка поездов в соответствии с заданными условиями
        /// </summary>
        /// <param name="searchTextBox"></param>
        public void FilterTrainList(object searchTextBox)
        {
            var textBox = searchTextBox as TextBox;
            var searchString = textBox.Text;

            if (searchString != null && searchString != "")
            {
                _trainList = ContextKeeper.Trains
                    .Where(train => train.TrainNum
                        .Contains(searchString) 
                        || train.Id.ToString().Contains(searchString))
                            .ToList();
                _obsTrainList.Clear();
                _trainList.ForEach(train => _obsTrainList.Add(train));
            }
            else
            {
                RefreshTrainList();
            }
        }

    }
}
