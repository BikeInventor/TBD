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

            var arrivalStation = new Station();
            arrivalStation.StationName = "Станция1";
            Contexts.Stations.Add(arrivalStation);

            var deartureStation = new Station();
            deartureStation.StationName = "Станция2";
            Contexts.Stations.Add(deartureStation);


            var newTrain = new Train();
            newTrain.ArrivalStationID = 1;
            newTrain.DepartureStationID = 2;
            newTrain.DepartureTime = DateTime.Now.TimeOfDay;
            newTrain.TrainNum = 109;                             // заменить на VARCHAR
            Contexts.Trains.Add(newTrain);

            var seat = new Seat();
            seat.SeatType = 0;
            Contexts.Seats.Add(seat);


            var wagonSeat = new WagonSeat();
            wagonSeat.Seat = Contexts.Seats.FindBy(s => s.Id == 1).FirstOrDefault();
            Contexts.WagonSeats.Add(wagonSeat);

            var wagon = new Wagon();
            wagon.SeatsAmount = 1;
            wagon.WagonType = 0;
            Contexts.Wagons.Add(wagon);

            var trainComposition = new TrainComposition();
            trainComposition.TrainId = 1;
            trainComposition.WagonId = 1;
            trainComposition.WagonNum = 1;
            Contexts.TrainCompositions.Add(trainComposition);


        }
    }
}
