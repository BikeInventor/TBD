using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Railways.Model.Context;

/*

namespace Railways.Model.ModelBuilder
{
    /// <summary>
    /// Количество мест в вагоне, в зависимости от его типа
    /// </summary>
    public enum WagonSeatsAmount
    {
        BERTH = 54,
        COUPE = 36,
        LUX = 18,
    }
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
            wagon.WagonType = (int)wagonType;
            wagon.SeatsAmount = GetSeatsAmount(wagonType);
            ContextKeeper.Wagons.Add(wagon);

            var trainComposition = new TrainComposition();
            trainComposition.TrainId = trainId;
            trainComposition.WagonNum = GetWagonNumber(trainId);
            trainComposition.WagonId = wagon.Id;
            ContextKeeper.TrainCompositions.Add(trainComposition);

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
                seat.Sold = false;
                ContextKeeper.Seats.Add(seat);

                var wagonSeat = new WagonSeat();
                wagonSeat.WagonId = wagonId;
                wagonSeat.SeatNum = GetSeatNumber(wagonId);
                wagonSeat.SeatId = seat.Id;
                ContextKeeper.WagonSeats.Add(wagonSeat);
            }

        }


        /// <summary>
        /// Получение очередного номера вагона заданного поезда
        /// </summary>
        /// <param name="trainId"></param>
        /// <returns></returns>
        private static int GetWagonNumber(int trainId)
        {
            var lastWagonNumber =
                ContextKeeper.TrainCompositions
                .Where(composition => composition.TrainId == trainId)
                .Select(composition => composition.WagonNum)
                .Max();

            return (lastWagonNumber == null) ? 1 : (int)lastWagonNumber++;
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
        private static int GetSeatNumber(int wagonId)
        {
            var lastSeatNumber =
                ContextKeeper.WagonSeats
                .Where(seat => seat.WagonId == wagonId)
                .Select(seat => seat.SeatNum)
                .Max();

            return (lastSeatNumber == null) ? 1 : (int)lastSeatNumber++;
        }

        /// <summary>
        /// Получение количества вагонов заданного поезда
        /// </summary>
        /// <param name="trainId"></param>
        /// <returns></returns>
        public static int GetWagonsCount(int trainId)
        {
            return ContextKeeper.TrainCompositions
                .Where(composition => composition.TrainId == trainId)
                .Count();
        }
    }
}

*/