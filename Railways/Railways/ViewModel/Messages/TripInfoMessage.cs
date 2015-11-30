using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railways.ViewModel.Messages
{
    public class TripInfoMessage
    {
        public int TrainId { get; set; }
        public DateTime DepDate { get; set; }
        public DateTime ArrDate { get; set; }

        public TripInfoMessage(int trainId, DateTime depDate, DateTime arrDate)
        {
            this.ArrDate = arrDate;
            this.DepDate = depDate;
            this.TrainId = trainId;
        }
    }
}
