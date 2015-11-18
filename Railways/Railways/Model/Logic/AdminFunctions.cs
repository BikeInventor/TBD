using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Railways.Model.Context;
using Railways.Model;

namespace Railways.Model.Logic
{
    public static class AdminFunctions
    {
        public static void RegisterEmployee(String fullName, String password, String adminRights)
        {
            var newEmp = new Employee();
            newEmp.FullName = fullName;
            newEmp.Password = Utils.EncryptString(password);
            newEmp.UserRights = byte.Parse(adminRights);

            ContextKeeper.Employees.Add(newEmp);
        }
    }
}
