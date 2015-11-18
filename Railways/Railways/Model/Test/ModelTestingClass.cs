using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railways.Model.Test
{
    public static class ModelTestingClass
    {
        public static void AddEmployee()
        {
            Model.Context.ContextKeeper.Initialize();
            Railways.Model.Logic.AdminFunctions.RegisterEmployee("Никита Евгенич", "1", "1");
        }
    }
}
