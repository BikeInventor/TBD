using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Railways.Model.Context;

namespace Railways.View
{
    class VoyageView
    {
        public int VoyageId { get; private set; }
        public string TrainNum { get; private set; }
        public string DepartureStation { get; private set; }
        public string ArrivalStation { get; private set; }
        public DateTime? DepartureDate { get; private set; }
        public DateTime? ArrivalDate { get; private set; }
        public int? Periodicity { get; private set; }
        public bool IsLuxSeatsAvailable { get; private set; }
        public bool IsCoupeSeatsAvailable { get; private set; }     
        public bool IsBerthSeatsAvailable { get; private set; }
        public float LuxPrice{get;private set;}
        public float CoupePrice { get; private set; }
        public float BerthPrice { get; private set; }

        public VoyageView(int _voyageId) 
        {
            VoyageId = _voyageId;
            this.InitializeProperties();
        }

        private void InitializeProperties()
        {
            var voyage = ContextKeeper.Voyages.FindBy(_voyage=> _voyage.Id==VoyageId).First();
            var train = ContextKeeper.Trains.FindBy(_train => _train.Id == voyage.TrainId).First();

            this.TrainNum = ContextKeeper.Trains.First(_train => _train.Id == voyage.TrainId).TrainNum;
            this.DepartureStation = ContextKeeper.Stations.First(station => station.Id == train.DepartureStationID).StationName;
            this.ArrivalStation = ContextKeeper.Stations.First(station => station.Id == train.ArrivalStationID).StationName;
            this.Periodicity = train.Periodicity;
            this.DepartureDate = train.DepartureTime;
            this.ArrivalDate = train.ArrivalTime;
            SetSeatsAvailibility(train.Id);
        }

        private void SetPrice(float price)
        {
        }

        public static VoyageView FormView(int id)
        {
            return new VoyageView(id);
        }


        private void SetSeatsAvailibility(int trainId)
        {
            var wagons = ContextKeeper.TrainCompositions
                .Where(trainComp => trainComp.TrainId == trainId)
                .Select(trainComp => trainComp.WagonId).ToList();
            this.IsLuxSeatsAvailable = FreeSeatsOfExactTypeAvailable(SeatType.LUX, wagons);
            this.IsCoupeSeatsAvailable = FreeSeatsOfExactTypeAvailable(SeatType.COUPE, wagons);
            this.IsBerthSeatsAvailable = FreeSeatsOfExactTypeAvailable(SeatType.BERTH, wagons);
        }


        private bool FreeSeatsOfExactTypeAvailable(SeatType seatType, List<int?> wagons)
        {
            var seatsCount = 0;
            wagons.ForEach(wagId =>
            {
                var seatIds = ContextKeeper.WagonSeats.Where(wagon => wagon.Id == wagId).Select(seat => seat.Id).ToList();
                var currentWagonSeatsCount = 0;
                seatIds.ForEach(seatId =>
                {
                    currentWagonSeatsCount +=
                        ContextKeeper.Seats
                            .Where(seat => seat.Id == seatId)
                            .Where(seat => seat.SeatType == (int)seatType)
                            .Where(seat => seat.Sold == false)
                            .Count();
                });
                seatsCount += currentWagonSeatsCount;
            });

            bool isAvailible = (seatsCount>0) ? true : false;
            return isAvailible;
        }
    }
}
