﻿using System;
using System.Linq;
using Railways.Model.Context;
using System.Windows;
using Railways.Model;

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
                ContextKeeper.Initialize();

                if (CorrectAuthInfo(int.Parse(id), password))
                {
                    _isLoggedIn = true;
                }
                else
                {
                    ShowError("");
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
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

        private static void ShowError(String message)
        {
            MessageBox.Show("Ошибка авторизации: направильный id или пароль\n" + message, 
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}