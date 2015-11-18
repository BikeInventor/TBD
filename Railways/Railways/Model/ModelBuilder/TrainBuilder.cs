using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Railways.Model.Context;

/// <summary>
/// Тип места в вагоне
/// </summary>
public enum WagonType
{
    BERTH = 0,
    COUPE = 1,
    LUX = 2,
}

/// <summary>
/// Количество мест в вагоне, в зависимости от его типа
/// </summary>
public enum WagonSeatsAmount
{
    BERTH = 54,
    COUPE = 36,
    LUX = 18,
}

namespace Railways.Model.ModelBuilder
{

    public static class TrainBuilder
    {
        /// <summary>
        /// Добавление вагона к поезду
        /// </summary>
        /// <param name="trainId"></param>
        /// <param name="wagonType"></param>
        public static void AddWagonToTrain(int trainId, WagonType wagonType)
        {
            var wagon = new Wagon();
            wagon.WagonType = (byte)wagonType;
            wagon.WagonNum = GetWagonNumber(trainId);
            ContextKeeper.Wagons.Add(wagon);

            var trainWagon = new TrainWagon();
            trainWagon.WagonId = wagon.Id;
            trainWagon.TrainId = trainId;
            ContextKeeper.TrainWagons.Add(trainWagon);

            AddSeatsToWagon(wagon.Id, wagonType);
        }

        /// <summary>
        /// Добавление мест определённого типа к заданному вагона
        /// </summary>
        /// <param name="wagonId"></param>
        /// <param name="wagonType"></param>
        private static void AddSeatsToWagon(int wagonId, WagonType wagonType)
        {
            for (int i = 0; i < GetSeatsAmount(wagonType); i++)
            {
                var seat = new Seat();
                seat.SeatNum = GetSeatNumber(wagonId);
                ContextKeeper.Seats.Add(seat);

                var wagonSeat = new WagonSeat();
                wagonSeat.WagonId = wagonId;
                wagonSeat.SeatId = seat.Id;
                ContextKeeper.WagonSeats.Add(wagonSeat);
            }

        }


        /// <summary>
        /// Получение очередного номера вагона заданного поезда
        /// </summary>
        /// <param name="trainId"></param>
        /// <returns></returns>
        private static byte GetWagonNumber(int trainId)
        {
            byte defaultNumber = 1;

            var allWagons = ContextKeeper.TrainWagons.Where(tw => tr)


            var wagonsOfTrainIds = ContextKeeper.TrainWagons
                    .Where(tw => tw.TrainId == trainId)
                    .Select(tw => tw.Id);

            var lastWagonNumber = ContextKeeper.Wagons
                .Where(wagon =>  wagonsOfTrainIds
                .Contains(wagon.Id))
                .Select(wagon => wagon.WagonNum)
                .Max();
    
            return (lastWagonNumber > 0) ? (byte)lastWagonNumber : defaultNumber;
        }

        /// <summary>
        /// Получение количества мест в вагоне по типу вагона
        /// </summary>
        /// <param name="wagonType"></param>
        /// <returns></returns>
        private static int GetSeatsAmount(WagonType wagonType)
        {
            switch (wagonType)
            {
                case WagonType.BERTH:
                    {
                        return (int)WagonSeatsAmount.BERTH;
                    }
                case WagonType.COUPE:
                    {
                        return (int)WagonSeatsAmount.COUPE;
                    }
                case WagonType.LUX:
                    {
                        return (int)WagonSeatsAmount.LUX;
                    }
            }
            return (int)WagonSeatsAmount.BERTH;
        }

        /// <summary>
        /// Получение очередного номера места заданного вагона
        /// </summary>
        /// <param name="trainId"></param>
        /// <returns></returns>
        private static byte GetSeatNumber(int wagonId)
        {
            byte defaultNumber = 1;

            var seatsOfWagonIds = ContextKeeper.WagonSeats
                .Where(ws => ws.WagonId == wagonId)
                .Select(id => id.Id);

            var lastSeatNumber = ContextKeeper.Seats
                .Where(seat => seatsOfWagonIds
                .Contains(seat.Id))
                .Select(seat => seat.SeatNum)
                .Max();

            return (lastSeatNumber > 0) ? (byte)lastSeatNumber : defaultNumber;
        }

        /// <summary>
        /// Получение количества вагонов заданного поезда
        /// </summary>
        /// <param name="trainId"></param>
        /// <returns></returns>
        public static int GetWagonsCount(int trainId)
        {
            return ContextKeeper.TrainWagons
                .Where(tw => tw.TrainId == trainId)
                .Count();
        }
    }
}

