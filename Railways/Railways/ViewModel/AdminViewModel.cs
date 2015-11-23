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

namespace Railways.ViewModel
{
    public class AdminViewModel : ViewModelBase
    {
        private List<Employee> _employeeList;

        private ObservableCollection<Employee> _obsEmpList;
        public int SelectedIndex { get; set; }
        public RelayCommand RegisterEmployee
        {
            get;
            private set;
        }

        public RelayCommand DeleteEmployee
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

        public AdminViewModel()
        {
            ContextKeeper.Initialize();

            _obsEmpList = new ObservableCollection<Employee>();
            _employeeList = new List<Employee>();

            RefreshList();

            RegisterEmployee = new RelayCommand(() => AddEmployee());                   
            DeleteEmployee = new RelayCommand(() => DeleteEmp());

            Messenger.Default.Register<RefreshEmployeeListMessage>(this, (msg) =>
            {
                RefreshList();
            });
        }

        public void RefreshList()
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

        private void DeleteEmp()
        {
            if (SelectedIndex >= 0)
            {
                var EmpToDelete = EmployeeList[SelectedIndex];
                ContextKeeper.Employees.Remove(EmpToDelete);
                RefreshList();
            }
        }
    }
}
