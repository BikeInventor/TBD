using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Railways.Model.Logic;
using System.Collections.ObjectModel;
using Railways.ViewModel.Messages;
using GalaSoft.MvvmLight.Messaging;
using Railways.Model.Context;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using System.Windows.Controls;
using System.Windows.Threading;


namespace Railways.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        public String Id { get; set; }
        public RelayCommand<object> LoginCmd
        {
            get;
            private set;
        }

        private bool _isConnected;

        public LoginViewModel()
        {
            LoginCmd = new RelayCommand<object>(this.TryLogin);
        }

        /// <summary>
        /// Осуществление входа сотрудника в систему
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        public void TryLogin(object pBox)
        {
            try
            {
                var passwordBox = pBox as PasswordBox;
                var password = passwordBox.Password;

                if (!_isConnected)
                {
                    ConnectToDB();
                    if (!_isConnected) return;
                }

                if (CorrectAuthInfo(int.Parse(Id), password) && !String.IsNullOrEmpty(this.Id))
                {
                    LogIn(int.Parse(Id));
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            catch (Exception)
            {
                // var controller = DialogService.ShowMessage("Неправильно указан id/пароль",
                //    "Ошибка аутентификации", MessageDialogStyle.Affirmative);
            }
        }

        /// <summary>
        /// Проверка корректности id и пароля пользователя
        /// </summary>
        /// <returns>true, если существует пользователь с заданной парой id - пароль</returns>
        private static Boolean CorrectAuthInfo(int id, String password)
        {
            var currentEmp = ContextKeeper.Employees.FindBy(emp => emp.Id == id).FirstOrDefault();
            if (currentEmp != null && (Utils.CorrectHash(currentEmp.Password, password)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void LogIn(int id)
        {
            var loggedInEmployee = ContextKeeper.Employees.Where(emp => emp.Id == id).First();
            if (loggedInEmployee.UserRights == 0)
            {
                var scheduleWin = new ScheduleWindow();
                scheduleWin.Show();
            }
            else
            {
                var adminWin = new AdminWindow();
                adminWin.Show();
            }
            Application.Current.MainWindow.Close();
        }

        public bool ConnectToDB()
        {
            try
            {
                ContextKeeper.Initialize();
                _isConnected = true;
                return true;
            }
            catch (Exception ex)
            {

                //var msg = DialogService.ShowMessage("Не удалось поключиться к базе данных\n" + ex.Message,
                //   "Ошибка подключения", MessageDialogStyle.Affirmative);


                return false;
            }
        }

        private Task ShowMsgAsync()
        {
            return Task.Run(() =>
            {
                var metroWin = Application.Current.MainWindow as MetroWindow;
                metroWin.ShowMessageAsync("This is the title", "Some message");
            });
        }

    }
}