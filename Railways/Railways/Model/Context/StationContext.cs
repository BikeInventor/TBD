using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railways.Model.Context
{
    /// <summary>
    /// Контекст станции с репозиторием "StationSet"
    /// </summary>
    public class StationContext : ContextBase<Station, RailwayDataEntities>
    {
    }
}
