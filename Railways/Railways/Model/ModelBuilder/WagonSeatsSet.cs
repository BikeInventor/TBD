using Railways.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railways.Model.ModelBuilder
{
    public class WagonSeatsSet
    {
        public List<bool> Seats { get; private set; }

        public int WagonId { get; private set; }

        public List<int> SeatsIds { get; private set; }

        public WagonSeatsSet(int wagonId, DateTime depDate, DateTime arrDate)
        {
            this.Seats = new List<bool>();
            this.WagonId = wagonId;
            int seatsAmount = 0;

            var wagonType = (WagonType)ContextKeeper.Wagons.Where(w => w.Id == wagonId).Select(w => w.WagonType).First();

            switch (wagonType) 
            {
                case WagonType.BERTH: {
                    seatsAmount = (int)WagonSeatsAmount.BERTH;
                    break;
                }
                case WagonType.COUPE: {
                    seatsAmount = (int)WagonSeatsAmount.COUPE;
                    break;
                }
                case WagonType.LUX: {
                    seatsAmount = (int)WagonSeatsAmount.LUX;
                    break;
                }
            }

            SeatsIds = TrainBuilder.GetSeatsOfWagon(wagonId).Select(s => s.Id).ToList();

            for (var i = 0; i < seatsAmount; i++)
            {
                Seats.Add(TrainBuilder.SeatAvailibility(SeatsIds[i], depDate, arrDate));
            }
        }
    }
}
