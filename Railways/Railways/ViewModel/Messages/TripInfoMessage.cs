using Railways.Model.ModelBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railways.ViewModel.Messages
{
    public class TripInfoMessage
    {
        public TripInfo CurrentTripInfo { get; set; }

        public TripInfoMessage(TripInfo currentTripInfo)
        {
            this.CurrentTripInfo = currentTripInfo;
        }
    }
}
