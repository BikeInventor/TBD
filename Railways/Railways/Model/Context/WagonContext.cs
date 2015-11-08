using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railways.Model.Context
{
    /// <summary>
    /// Контекст вагона с репозиторием "WagonSet"
    /// </summary>
    public class WagonContext : ContextBase<Wagon, RailwayDataEntities>
    {
    }
}
