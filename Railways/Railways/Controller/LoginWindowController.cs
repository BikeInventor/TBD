using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Railways.Model;
using Railways.Model.Context;

namespace Railways.Logic
{
    public static class LoginWindowController
    {
        /// <summary>
        /// Осуществление входа сотрудника в систему
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        public static void Login(String id, String password)
        {
           if (CorrectAuthInfo(int.Parse(id), password))
           {
               System.Console.WriteLine("ОК!");
           }
           else 
           {
               System.Console.WriteLine("NOT OK!");
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
    }
}
