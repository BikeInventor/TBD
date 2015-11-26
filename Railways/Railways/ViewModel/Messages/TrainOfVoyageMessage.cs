using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railways.ViewModel.Messages
{
    public class TrainOfVoyageMessage
    {
        public int TrainId { get; set; }
        public TrainOfVoyageMessage(int trainId)
        {
            this.TrainId = trainId;
        }
    }
}
