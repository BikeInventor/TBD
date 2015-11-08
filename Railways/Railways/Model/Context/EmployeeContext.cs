using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railways.Model.Context
{
    /// <summary>
    /// Контекст сотрудника с репозиторием "EmployeeSet"
    /// </summary>
    public class EmployeeContext : ContextBase<Employee, RailwayDataEntities>
    {
    }
}
