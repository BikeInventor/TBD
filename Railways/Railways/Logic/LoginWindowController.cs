using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
           if (isCorrectAuthInfo(int.Parse(id), password))
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
        /// <returns></returns>
        private static Boolean isCorrectAuthInfo(int id, String password)
        {
            using (RailwayDataEntities db = new RailwayDataEntities())
            {
                var currentEmp = SystemObjects.EmpList.FindById(id);

                if (currentEmp == null)
                {
                    return false;
                }
                else if (Utils.CorrectHash(currentEmp.Password, password))
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
}
