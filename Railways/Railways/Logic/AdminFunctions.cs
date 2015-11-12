using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railways.Logic
{
    public static class AdminFunctions
    {
        /// <summary>
        /// Добавление нового сотрудника в систему
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="passportNum"></param>
        /// <param name="adminRights"></param>
        public static void RegisterEmployee(String fullName, String password, String adminRights)
        {
            var newEmp = new Employee();
            newEmp.Fullname = fullName;
            newEmp.Password = Utils.EncryptString(password);
            newEmp.AdminRights = int.Parse(adminRights);

            SystemObjects.EmpList.Add(newEmp);
        }
    }
}
