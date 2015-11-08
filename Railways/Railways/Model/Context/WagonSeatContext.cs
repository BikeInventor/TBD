using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railways.Model.Context
{
    /// <summary>
    /// Контекст мест вагона с репозиторием "WagonSeatSet"
    /// </summary>
    public class WagonSeatContext : ContextBase<WagonSeat, RailwayDataEntities>
    {
    }
}
