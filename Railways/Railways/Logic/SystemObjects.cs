using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railways.Logic
{
    public static class SystemObjects
    {
        private static EmployeesList employeesList = new EmployeesList();
        /// <summary>
        /// Список сотрудников системы
        /// </summary>
        public static EmployeesList EmpList {
            get { return employeesList;  }
        }
    }
}
