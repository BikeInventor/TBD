using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Railways.Model;
using Railways.Model.Context;
using Gat.Controls.Framework;

namespace Railways.Logic
{
    public static class LoginWindowController
    {
        private static bool _isLoggedIn = false;
        /// <summary>
        /// Признак удачного входа в систему
        /// </summary>
        public static bool IsLoggedIn
        {
            get { return _isLoggedIn;}
        }

        /// <summary>
        /// Осуществление входа сотрудника в систему
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        public static void Login(String id, String password)
        {
            try
            {
                if (CorrectAuthInfo(int.Parse(id), password))
                {
                    _isLoggedIn = true;
                }
                else
                {
                    ShowError();
                    System.Console.WriteLine("NOT OK!");
                }
            }
            catch (Exception e)
            {
                ShowError();
            }
        }

        /// <summary>
        /// Проверка корректности id и пароля пользователя
        /// </summary>
        /// <returns>true, если существует пользователь с заданной парой id - пароль</returns>
        private static Boolean CorrectAuthInfo(int id, String password)
        {
            Context.Initialize();
            var currentEmp = Context.Employees.Repository.Find(id);

                if (currentEmp != null && (Utils.CorrectHash(currentEmp.Password, password)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }

        private static void ShowError()
        {
            Gat.Controls.MessageBoxView messageBox = new Gat.Controls.MessageBoxView();
            Gat.Controls.MessageBoxViewModel vm = (Gat.Controls.MessageBoxViewModel)messageBox.FindResource("ViewModel");
            vm.Show("Ошибка авторизации: направильный id или пароль", "Ошибка", Gat.Controls.MessageBoxButton.Ok, Gat.Controls.MessageBoxImage.Warning);
        }
    }
}
