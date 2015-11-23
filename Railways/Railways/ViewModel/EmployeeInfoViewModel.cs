using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Railways.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Railways.Model.Logic;
using Railways.Model.Context;
using Microsoft.Practices.ServiceLocation;
using Railways.ViewModel.Messages;
using GalaSoft.MvvmLight.Messaging;

namespace Railways.ViewModel
{
    public class EmployeeInfoViewModel : ViewModelBase
    {
        public String FullName { get; set; }
        public String Password { get; set; }
        public String UserRights { get; set; }

        public RelayCommand<EmployeeInfoWindow> RegisterEmployee
        {
            get;
            private set;
        }

        public EmployeeInfoViewModel()
        {
            RegisterEmployee = new RelayCommand<EmployeeInfoWindow>(this.AddEmployee);
        }

        private void AddEmployee(EmployeeInfoWindow window)
        {
            AdminFunctions.RegisterEmployee(FullName, Password, UserRights.ToString());
            Messenger.Default.Send(new RefreshEmployeeListMessage());
            window.Close();
        }
    }
}
