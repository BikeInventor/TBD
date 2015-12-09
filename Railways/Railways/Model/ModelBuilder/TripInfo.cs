using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Railways.Model.Context;
using Railways.Model.Logic;
using Railways.Model.ModelBuilder;

namespace Railways.Model.ModelBuilder
{
    public class TripInfo
    {
        private Voyage _voyage;
        public Route DepRoute { get; set; }
        public Route ArrRoute { get; set; }
        public int TrainId { get; private set; }
        public String TrainNumber { get; private set; }
        public String DepartureStation { get; private set; }
        public String ArrivalStation { get; private set; }
        public DateTime DepartureTime { get; private set; }
        public DateTime ArrivalTime { get; private set; }
        public String TripDuration { get; private set; }
        public Double BerthPrice { get; private set; }
        public Double CoupePrice { get; private set; }
        public Double LuxPrice { get; private set; }
        public int BerthCount { get; private set; }
        public int CoupeCount { get; private set; }
        public int LuxCount { get; private set; }
        public int DateOffset { get; private set; }
        public String FreeSeatsCount { get; private set; }
        public String Cost { get; private set; }
        public String WagonTypeTable { get; private set; }

        /// <summary>
        /// Класс, предоставляющий информацию о конкретной поездке
        /// </summary>
        public TripInfo(int voyageId, int depRouteId, int arRouteId, int dateOffset)
        {
            this._voyage = ContextKeeper.Voyages.First(v => v.Id == voyageId);
            this.DepRoute = ContextKeeper.Routes.First(r => r.Id == depRouteId);
            this.ArrRoute = ContextKeeper.Routes.First(r => r.Id == arRouteId);

            this.TrainId = _voyage.TrainId.Value;

            this.TrainNumber = _voyage.Train.TrainNum;
            this.DepartureStation = DepRoute.Station.StationName;
            this.ArrivalStation = ArrRoute.Station.StationName;

            this.DepartureTime = ContextKeeper.Routes
                .Where(r => r.Id == depRouteId)
                .Select(r => r.DepartureTimeOffset.Value)
                .First()
                .AddDays(DateOffset);

            this.ArrivalTime = ContextKeeper.Routes
                .Where(r => r.Id == arRouteId)
                .Select(r => r.ArrivalTimeOffset.Value)
                .First();

            this.ArrivalTime = this.ArrivalTime.AddDays(dateOffset);
            this.DepartureTime = this.DepartureTime.AddDays(dateOffset);

            SetEachTypeSeatsCount();
            CalculatePrice();
            SetWagonTypeTableFields();
        }

        /// <summary>
        /// Подсчёт количества мест в вагонах каждого типа
        /// </summary>
        private void SetEachTypeSeatsCount()
        {
            var berth = new List<WagonSeatsSet>();
            var coupe = new List<WagonSeatsSet>();
            var lux = new List<WagonSeatsSet>();

            var wagonsOfTrain = TrainBuilder.GetWagonsOfTrain(_voyage.Train.Id).ToList();
            wagonsOfTrain.ForEach(wagon =>
            {
                var currentWag = new WagonSeatsSet(wagon.Id, DepartureTime, ArrivalTime);

                    var wagonType = (WagonType)ContextKeeper.Wagons.Where(w => w.Id == wagon.Id).Select(w => w.WagonType.Value).First();
                    switch (wagonType)
                    {
                        case WagonType.BERTH:
                            {
                                BerthCount += FreeSeatsOfWagonCount(currentWag);
                                break;
                            }
                        case WagonType.COUPE:
                            {
                                CoupeCount += FreeSeatsOfWagonCount(currentWag);
                                break;
                            }
                        case WagonType.LUX:
                            {
                                LuxCount += FreeSeatsOfWagonCount(currentWag);
                                break;
                            }
                    }
            });        
        }

        /// <summary>
        /// Расчёт цены на места в зависимости от их типа
        /// </summary>
        private void CalculatePrice()
        {
            var tripDistance = CalculateTripDistance();
            this.BerthPrice = BusinessLogic.CalculatePrice(tripDistance, WagonType.BERTH);
            this.CoupePrice = BusinessLogic.CalculatePrice(tripDistance, WagonType.COUPE);
            this.LuxPrice = BusinessLogic.CalculatePrice(tripDistance, WagonType.LUX);
        }

        private double CalculateTripDistance()
        {
            return this.ArrRoute.Distance.Value - this.DepRoute.Distance.Value;
        }

        private int FreeSeatsOfWagonCount(WagonSeatsSet wagonSeats)
        {
            int freeSeatsCount = 0;
            wagonSeats.Seats.ForEach(freeSeat =>
            {
                if (freeSeat) freeSeatsCount++;
            });
            return freeSeatsCount;
        }

        /// <summary>
        /// Построение объекта класса, отвечающего за отображение информации о конкретной поездке
        /// </summary>
        /// <param name="voyageId">Поездка</param>
        /// <param name="depRouteId">начальная точка маршрута</param>
        /// <param name="arRouteId">конечная точка маршрута</param>
        /// <returns></returns>
        public static TripInfo BuildTripInfo(int voyageId, int depRouteId, int arRouteId, int dateOffset)
        {
            return new TripInfo(voyageId,depRouteId, arRouteId, dateOffset);
        }

        private void SetWagonTypeTableFields()
        {
            WagonTypeTable += "Плацкарт\nКупе\nЛюкс";
            Cost += BerthPrice.ToString() + " РУБ.\n" +
                CoupePrice.ToString() + " РУБ.\n" +
                LuxPrice.ToString() + " РУБ.";
            FreeSeatsCount += BerthCount.ToString() + "\n" +
                CoupeCount.ToString() + "\n" +
                LuxCount.ToString();
        }
    }
}
