using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railways.Model.Context
{
    /// <summary>
    /// Контекст станции, принадлежащей пути следования поезда
    /// </summary>
    public class RouteContext : ContextBase<Route, RailwayDataModelContainer>
    {
    }
}
