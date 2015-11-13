using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Railways.Model.Context;

namespace Railways.Model
{
    public static class TestDataLoader
    {
        public static void AddTestTrain()
        {
            /*
            var seat = new Seat();
            seat.SeatType = (int)WagonType.BERTH;
            seat.Sold = false;

            var seat1 = new Seat();
            seat1.SeatType = (int)WagonType.BERTH;
            seat1.Sold = false;
            var seat2 = new Seat();
            seat2.SeatType = (int)WagonType.COUPE;
            seat2.Sold = true;
            var seat3 = new Seat();
            seat3.SeatType = (int)WagonType.BERTH;
            seat3.Sold = true;
            var seat4 = new Seat();
            seat4.SeatType = (int)WagonType.LUX;
            seat4.Sold = false;
            var seat5 = new Seat();
            seat5.SeatType = (int)WagonType.COUPE;
            seat5.Sold = false;
            

            var deartureStation = new Station();
            deartureStation.StationName = "Станция2";
            ContextKeeper.Stations.Add(deartureStation);

            var newTrain = new Train();
            newTrain.ArrivalStationID = 1;
            newTrain.DepartureStationID = 2;
            newTrain.TrainNum = 109;                             
            ContextKeeper.Trains.Add(newTrain);

            var seat = new Seat();
            seat.SeatType = 0;
            ContextKeeper.Seats.Add(seat);


            var wagonSeat = new WagonSeat();
            wagonSeat.Seat = ContextKeeper.Seats.FindBy(s => s.Id == 1).FirstOrDefault();
            ContextKeeper.WagonSeats.Add(wagonSeat);

            var wagon = new Wagon();
            wagon.SeatsAmount = 1;
            wagon.WagonType = 0;
            ContextKeeper.Wagons.Add(wagon);

            var trainComposition = new TrainComposition();
            trainComposition.TrainId = 1;
            trainComposition.WagonId = 1;
            trainComposition.WagonNum = 1;
            ContextKeeper.TrainCompositions.Add(trainComposition);
            */
        }
    }
}
