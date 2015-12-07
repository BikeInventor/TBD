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
using MaterialDesignThemes.Wpf;
using Railways.View;
using Railways.ViewModel.Services;

namespace Railways.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private bool _loginButtonAvailability;
        private String _loadingVisibility;
        private bool _isConnected;
        public bool LogInButtonAvailability
        {
            get
            {
                return _loginButtonAvailability;
            }
            set
            {
                _loginButtonAvailability = value;
                RaisePropertyChanged("LogInButtonAvailability");
            }
        }
        public String LoadingVisibility
        {
            get
            {
                return _loadingVisibility;
            }
            set
            {
                _loadingVisibility = value;
                RaisePropertyChanged("LoadingVisibility");
            }
        }
        public String Id { get; set; }
        public RelayCommand<object> LoginCmd
        {
            get;
            private set;
        }

        public LoginViewModel()
        {
            LoginCmd = new RelayCommand<object>(this.TryLogin);
            LoadingVisibility = "0";
            LogInButtonAvailability = true;
        }
        /// <summary>
        /// Осуществление входа сотрудника в систему
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        public async void TryLogin(object pBox)
        {
            try
            {                
                LoadingVisibility = "100";
                LogInButtonAvailability = false;
                var passwordBox = pBox as PasswordBox;
                var password = passwordBox.Password;

                if (!_isConnected)
                {
                    _isConnected = await ConnectToDB();
                    if (!_isConnected)
                    {
                        await DialogService.ShowDialog("LoginWindow", 
                            "Не удалось подключиться к серверу базы данных", 
                            DialogWindowType.INFODIALOG);
                        return;
                    }
                }

                if (CorrectAuthInfo(int.Parse(Id), password) && !String.IsNullOrEmpty(this.Id))
                {
                    LogIn(int.Parse(Id));
                }
                else
                {
                    await DialogService.ShowDialog("LoginWindow", 
                        "Пользователь не найден", 
                        DialogWindowType.INFODIALOG);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка подключения");
            }
            finally
            {
                LogInButtonAvailability = true;
                LoadingVisibility = "0";
            }
        }

        /// <summary>
        /// Проверка корректности id и пароля пользователя
        /// </summary>
        /// <returns>true, если существует пользователь с заданной парой id - пароль</returns>
        private static Boolean CorrectAuthInfo(int id, String password)
        {
            var currentEmp = ContextKeeper.Employees.FindBy(emp => emp.Id == id).FirstOrDefault();
            if (currentEmp != null && (ModelUtils.CorrectHash(currentEmp.Password, password)))
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

        public async Task <bool> ConnectToDB()
        {
            try
            {
                await ContextKeeper.Initialize();
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