using Railways.Model.ModelBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railways.ViewModel.Messages
{
    public class TicketInfoMessage
    {
        public bool IsForPrint { get; set; }
        public TripInfo TripInfo { get; set; }
        public int SeatId { get; set; }
        public int EmpId { get; set; }
        public int ClientId { get; set; }
        public double TicketPrice { get; set; }

        public TicketInfoMessage(TripInfo tripInfo, int seatId, 
            int empId, int clientId, double ticketPrice)
        {
            this.TripInfo = tripInfo;
            this.SeatId = seatId;
            this.EmpId = empId;
            this.ClientId = clientId;
            this.TicketPrice = ticketPrice;
        }

        public TicketInfoMessage(TicketInfoMessage msg)
        {
            this.TripInfo = msg.TripInfo;
            this.SeatId = msg.SeatId;
            this.EmpId = msg.SeatId;
            this.ClientId = msg.ClientId;
            this.TicketPrice = msg.TicketPrice;
        }
    }
}
