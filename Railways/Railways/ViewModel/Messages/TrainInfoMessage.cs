using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Railways.Model;

namespace Railways.ViewModel.Messages
{
    public class TrainInfoMessage
    {
        public int TrainId { get; set; }
        public TrainInfoMessage(int trainId)
        {
            this.TrainId = trainId;
        }
    }
}
