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
        private Route _depRoute;
        private Route _arRoute;
        public String TrainNumber { get; private set; }
        public String DepartureStation { get; private set; }
        public String ArrivalStation { get; private set; }
        public String DepartureTime { get; private set; }
        public String ArrivalTime { get; private set; }
        public String TripDuration { get; private set; }
        public Double BerthPrice { get; private set; }
        public Double CoupePrice { get; private set; }
        public Double LuxPrice { get; private set; }
        public Boolean isBerthAvailible { get; private set; }
        public Boolean isCoupeAvailible { get; private set; }
        public Boolean isLuxAvailible { get; private set; }

        /// <summary>
        /// Класс, предоставляющий информацию о конкретной поездке
        /// </summary>
        /// <param name="voyageId"></param>
        /// <param name="depRouteId"></param>
        /// <param name="arRouteId"></param>
        public TripInfo(int voyageId, int depRouteId, int arRouteId)
        {
            this._voyage = ContextKeeper.Voyages.First(v => v.Id == voyageId);
            this._depRoute = ContextKeeper.Routes.First(r => r.Id == depRouteId);
            this._arRoute = ContextKeeper.Routes.First(r => r.Id == arRouteId);

            this.TrainNumber = _voyage.Train.TrainNum;
            this.DepartureStation = _depRoute.Station.StationName;
            this.ArrivalStation = _depRoute.Station.StationName;

            this.ArrivalTime = Utils.FormatDateOffset(_voyage.DepartureDateTime, _arRoute.ArrivalTimeOffset);
            this.DepartureTime = Utils.FormatDateOffset(_voyage.DepartureDateTime, _depRoute.ArrivalTimeOffset);

            CalculatePrice();
        }

        private void CalculatePrice()
        {
            int berthCount = 0;
            int coupeCount = 0;
            int luxCount = 0;

            var wagonsOfTrain = TrainBuilder.GetWagonsOfTrain(_voyage.Train.Id);

            wagonsOfTrain.ToList().ForEach(w =>
            {
                var seatsOfWagon = TrainBuilder.GetSeatsOfWagon(w.Id);

                seatsOfWagon.ToList().ForEach(seat => 
                {

                    //var ticketsOfSeat = TrainBuilder.GetTicketsOfSeat(seat.Id).Where(t => );
                     
                    //ticketsOfSeat.ToList().ForEach(ticket => );
                });
                
            });
          
        }

        /// <summary>
        /// Построение объекта класса, отвечающего за отображение информации о конкретной поездке
        /// </summary>
        /// <param name="voyageId">Поездка</param>
        /// <param name="depRouteId">начальная точка маршрута</param>
        /// <param name="arRouteId">конечная точка маршрута</param>
        /// <returns></returns>
        public static TripInfo BuildTripInfo(int voyageId, int depRouteId, int arRouteId)
        {
            return new TripInfo(voyageId,depRouteId, arRouteId);
        }
    }
}







/*
               var seatIds = ContextKeeper.Seats.All()
                .Join(wagons,
                    s => s.Wagon_Seat,
                    w => w.Wagon_Seat,
                    (s, w) => s.Id
                ).ToList();


            seatIds.ForEach(seatId =>
            {
                var allTicketsWithCurrentSeat = ContextKeeper.Tickets
                    .Where(ticket => ticket.SeatId == seatId);

            });
*/