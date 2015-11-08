using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railways.Model.Context
{
    /// <summary>
    /// Контекст состава поезда с репозиторием "TrainCompositionSet"
    /// </summary>
    public class TrainCompositionContext : ContextBase<TrainComposition, RailwayDataEntities>
    {
    }
}
