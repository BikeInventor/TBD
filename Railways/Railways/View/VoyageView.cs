using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Railways.Model.Context;
using Railways.Model;
using System.Linq.Expressions;

namespace Railways.View
{
    /// <summary>
    /// Класс, являющийся представлением информации о конкретной поездке
    /// </summary>
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
        public double LuxPrice{get;private set;}
        public double CoupePrice { get; private set; }
        public double BerthPrice { get; private set; }

        public VoyageView(int _voyageId) 
        {
            VoyageId = _voyageId;
            this.InitializeProperties();
        }
        /// <summary>
        /// Инициализация всех свойств представления информации о поездке и маршруте
        /// </summary>
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
            SetPrice(train.Id);
        }
        /// <summary>
        /// Установка цен на места заданного типа для данного поезда
        /// </summary>
        /// <param name="trainId"></param>
        private void SetPrice(int trainId)
        {
            this.BerthPrice=BusinessLogic.CalculatePrice(trainId, WagonType.BERTH);
            this.CoupePrice = BusinessLogic.CalculatePrice(trainId, WagonType.BERTH);
            this.LuxPrice = BusinessLogic.CalculatePrice(trainId, WagonType.BERTH);
        }

        /// <summary>
        /// Формирование VoyageView по заданному id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static VoyageView FormView(int id)
        {
            return new VoyageView(id);
        }

        /// <summary>
        /// Проверка наличия в поезде свободных мест всех типов (плацкарт, купе, люкс)
        /// </summary>
        /// <param name="trainId"></param>
        private void SetSeatsAvailibility(int trainId)
        {
            this.IsLuxSeatsAvailable = SeatsOfTypeAvailable(trainId, WagonType.LUX);
            this.IsCoupeSeatsAvailable = SeatsOfTypeAvailable(trainId, WagonType.COUPE);
            this.IsBerthSeatsAvailable = SeatsOfTypeAvailable(trainId, WagonType.BERTH);
        }

        /// <summary>
        /// Метод опредеяет наличие мест заданного типа в составе поезда
        /// </summary>
        /// <param name="wagons"></param>
        /// <param name="wagonType"></param>
        /// <returns></returns>
        private bool SeatsOfTypeAvailable(int trainId, WagonType wagonType)
        {
            var isFreeSeatsAvailible = false;

            var wagonsOfGivenTypeIds = ContextKeeper.Wagons
                .Where(wagon => ContextKeeper.TrainCompositions
                .Where(trainComp => trainComp.TrainId == trainId)
                .Select(trainComp => trainComp.WagonId)
                .Contains(wagon.Id))
                .Where(wagon => (int)wagon.WagonType == (int)wagonType)
                .Select(wagon => wagon.Id)
                .ToList();

            wagonsOfGivenTypeIds.ForEach(wagId => 
            {
                isFreeSeatsAvailible = ContextKeeper.Seats
                    .Where(seat => seat.Sold == false)
                    .Where(seat => ContextKeeper.WagonSeats
                    .Where(_seat => _seat.WagonId == wagId)
                    .Select(_seat => _seat.SeatId)
                    .Contains(seat.Id))
                    .Any();
                if (isFreeSeatsAvailible) return;
            });

            return isFreeSeatsAvailible;
        }
       
        public void TestOutput()
        {
            Console.WriteLine("Train number: {0}", this.TrainNum);
            Console.WriteLine("Departure Station: {0}", this.DepartureDate);
            Console.WriteLine("Arrival Station: {0}", this.ArrivalStation);
            Console.WriteLine("Departure Date: {0}", this.DepartureDate);
            Console.WriteLine("Arrival Date: {0}", this.ArrivalStation);
            Console.WriteLine("Berth seat availability: {0}", this.IsBerthSeatsAvailable);
            Console.WriteLine("Coupe seat availability: {0}", this.IsCoupeSeatsAvailable);
            Console.WriteLine("Lux seat availability: {0}", this.IsLuxSeatsAvailable);
            Console.WriteLine("Berth seat price: {0}", this.BerthPrice);
            Console.WriteLine("Coupe seat price: {0}", this.CoupePrice);
            Console.WriteLine("Lux seat price: {0}", this.LuxPrice);
        }
    }
}
