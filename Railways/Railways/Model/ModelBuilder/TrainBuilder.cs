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
        public static async Task AddWagonToTrain(int trainId, WagonType wagonType)
        {
            var wagon = new Wagon();
            wagon.WagonType = (byte)wagonType;
            wagon.WagonNum = GetWagonNumber(trainId);
            ContextKeeper.Wagons.Add(wagon);

            var trainWagon = new TrainWagon();
            trainWagon.WagonId = wagon.Id;
            trainWagon.TrainId = trainId;
            ContextKeeper.TrainWagons.Add(trainWagon);
            await Task.Run(() => AddSeatsToWagonAsync(wagon.Id, wagonType));

        }

        /// <summary>
        /// Удаление последнего вагона, принадлежащего заданному поезду
        /// </summary>
        /// <param name="trainId"></param>
        public static void DeleteLastWagonFromTrain(int trainId)
        {
            var wagons = ContextKeeper.TrainWagons
                    .Where(tw => tw.TrainId == trainId)
                    .Select(tw => tw.WagonId);
            var lastWagonId = wagons.Max();
            var lastWagon = ContextKeeper.TrainWagons.First(w => w.WagonId == lastWagonId);

            ContextKeeper.TrainWagons.Remove(lastWagon); 
        }

        /// <summary>
        /// Удаление мест, принадлежащих поезду
        /// </summary>
        /// <param name="wagonId"></param>
        private static void DeleteWagonSeats(int wagonId)
        {
            var seatsOfWagonIds = ContextKeeper.WagonSeats
                    .Where(ws => ws.WagonId == wagonId)
                    .Select(id => id.Id);

            var seatsOfWagon = ContextKeeper.Seats
                .Where(seat => seatsOfWagonIds.Contains(seat.Id))
                .ToList();

            seatsOfWagon.ForEach(seat => ContextKeeper.Seats.Remove(seat));
        }

        /// <summary>
        /// Удаление всех вагонов заданного поезда
        /// </summary>
        /// <param name="trainId"></param>
        public static void DeleteTrainWithWagons(int trainId)
        {
            var wagonsOfTrain = ContextKeeper.TrainWagons
                    .Where(tw => tw.TrainId == trainId);

            wagonsOfTrain.ToList().ForEach(wag => ContextKeeper.TrainWagons.Remove(wag));

            ContextKeeper.Trains.Remove(ContextKeeper.Trains.First(train => train.Id == trainId));
        }

        /// <summary>
        /// Добавление мест определённого типа к заданному вагона
        /// </summary>
        /// <param name="wagonId"></param>
        /// <param name="wagonType"></param>
        private static Task AddSeatsToWagonAsync(int wagonId, WagonType wagonType)
        {
            return Task.Run(() =>
            {
                for (int i = 0; i < GetSeatsAmount(wagonType); i++)
                {
                    var seat = new Seat();
                    seat.SeatNum = i + 1;
                    ContextKeeper.Seats.Add(seat);

                    var wagonSeat = new WagonSeat();
                    wagonSeat.WagonId = wagonId;
                    wagonSeat.SeatId = seat.Id;
                    ContextKeeper.WagonSeats.Add(wagonSeat);
                }
            });            
        }


        /// <summary>
        /// Получение очередного номера вагона заданного поезда
        /// </summary>
        /// <param name="trainId"></param>
        /// <returns></returns>
        private static byte GetWagonNumber(int trainId)
        {

            var wagonsOfTrainIds = ContextKeeper.TrainWagons
                    .Where(tw => tw.TrainId == trainId)
                    .Select(tw => tw.Id);

            var lastWagonNumber = 1 + ContextKeeper.Wagons
                .Where(wagon => wagonsOfTrainIds
                .Contains(wagon.Id))
                .Count();
                  
            return (byte)lastWagonNumber;
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

            return (lastSeatNumber > 0) ? (byte)lastSeatNumber++ : defaultNumber;
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

        /// <summary>
        /// Получение всех вагонов заданного поезда
        /// </summary>
        /// <param name="trainId"></param>
        /// <returns></returns>
        public static IQueryable<Wagon> GetWagonsOfTrain(int trainId)
        {
            var wagonsOfTrain = ContextKeeper.TrainWagons
                .Where(tw => tw.TrainId == trainId).Select(tw => tw.WagonId);
            var wagons = ContextKeeper.Wagons
                .Where(wagon => wagonsOfTrain.Contains(wagon.Id));
            return wagons;
        }

        /// <summary>
        /// Получение всех мест заданного вагона
        /// </summary>
        /// <param name="wagonId"></param>
        /// <returns></returns>
        public static IQueryable<Seat> GetSeatsOfWagon(int wagonId)
        {
            var seatsOfTrain = ContextKeeper.WagonSeats
                .Where(ws => ws.WagonId == wagonId).Select(ws => ws.SeatId);
            var seats = ContextKeeper.Seats
                .Where(seat => seatsOfTrain.Contains(seat.Id));
            return seats;
        }

        public static IQueryable<Ticket> GetTicketsOfSeat(int seatId)
        {
            return ContextKeeper.Tickets.Where(t => t.SeatId == seatId);
        }

        public static bool SeatAvailibility(int seatId, DateTime depDate, DateTime arrDate)
        {
            var ticketsOfThisSeat = ContextKeeper.Tickets.Where(t => t.SeatId == seatId).ToList();

            if (ticketsOfThisSeat == null) return true;
            
            var ticketsOfThisDate = new List<Ticket>();
            ticketsOfThisSeat.ForEach(t => 
            {
                if ((t.DepartureDate.Value.Date == depDate.Date) && (t.ArrivalDate.Value.Date == arrDate.Date))
                {
                    ticketsOfThisDate.Add(t);
                }
            });

            if (ticketsOfThisSeat.Count == 0) return true;

            bool isAnyOverlaps = ticketsOfThisDate
                .Any(t =>
            {
                if (((t.ArrivalDate > depDate) && (t.DepartureDate < arrDate))
                    || ((t.DepartureDate.Value.ToString() == arrDate.ToString()) 
                    && (t.ArrivalDate.Value.ToString() == depDate.ToString())))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });
            return !isAnyOverlaps;
        }
    }
}

