using Railways.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railways.Model.ModelBuilder
{
    public static class VoyageBuilder
    {
        public static Voyage GetVoyageOfTrain(int trainId)
        {
            var voyageOfTrain = ContextKeeper.Voyages
                .Where(voyage => voyage.TrainId == trainId)
                .FirstOrDefault();

            return voyageOfTrain as Voyage;
        }

        /// <summary>
        /// Связывание рейса с поездом
        /// </summary>
        /// <param name="voyageId"></param>
        /// <param name="trainId"></param>
        public static void ConnectVoyageToTrain(int voyageId, int trainId)
        {
            var voyage = ContextKeeper.Voyages.First(v => v.Id == voyageId);
            voyage.TrainId = trainId;
        }
    }
}
