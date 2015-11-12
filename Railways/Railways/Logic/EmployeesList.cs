using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Railways.Logic
{
    public class EmployeesList : IRepository<Employee>
    {
        private RailwayDataEntities db = new RailwayDataEntities();

        public IEnumerable<Employee> List
        {
            get
            {
                return db.Employee;
            }
        }
        /// <summary>
        /// Добавление нового сотрудника
        /// </summary>
        /// <param name="employee"></param>
        public void Add(Employee employee)
        {
            db.Employee.Add(employee);
            db.SaveChanges();
        }
        /// <summary>
        /// Удаление сотрудника
        /// </summary>
        /// <param name="employee"></param>
        public void Delete(Employee employee)
        {
            db.Employee.Remove(employee);
            db.SaveChanges();
        }
        /// <summary>
        /// Изменение данных о сотруднике
        /// </summary>
        /// <param name="employee"></param>
        public void Update(Employee employee)
        {
            db.Employee.Attach(employee);
            db.Entry(employee).State = EntityState.Modified;
            db.SaveChanges();
        }
        /// <summary>
        /// Поиск сотрудника по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Employee FindById(int id)
        {
            var result = (from emp in db.Employee where emp.Id == id select emp).FirstOrDefault();
            return result;
        }
    }
}
