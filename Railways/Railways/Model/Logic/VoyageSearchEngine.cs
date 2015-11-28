using Railways.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railways.Model.Logic
{
    public static class VoyageSearchEngine
    {
        public static void FindVoyages(String depStationName, String arStationName, DateTime depDate)
        {
            var voyagesForThisDate = ContextKeeper.Voyages.Where(v => v.DepartureDateTime == depDate);
        }
    }
}
