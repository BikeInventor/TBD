using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railways.Model.Context
{
    /// <summary>
    /// Контекст пути следования поезда с репозиторием "VoyageSet"
    /// </summary>
    public class VoyageContext : ContextBase<Voyage, RailwayDataEntities>
    {
    }
}
