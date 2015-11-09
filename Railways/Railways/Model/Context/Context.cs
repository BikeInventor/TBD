using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railways.Model.Context
{
    /// <summary>
    /// Класс, хранящий различные контексты базы данных
    /// </summary>
    public static class Context
    {
        private static RailwayDataEntities _database;
        public static ClientContext Clients {get; private set;}
        public static EmployeeContext Employees { get; private set; }
        public static StationContext Stations { get; private set; }
        public static SeatContext Seats { get; private set; }
        public static TicketContext Tickets { get; private set; }
        public static TrainCompositionContext TrainCompositions { get; private set; }
        public static TrainContext Trains { get; private set; }
        public static TrainRouteContext TrainRoutes { get; private set; }
        public static VoyageContext Voyages { get; private set; }
        public static WagonContext Wagons { get; private set; }
        public static WagonSeatContext WagonSeats { get; private set; }

        public static void Initialize()
        {
            _database = new RailwayDataEntities();

            Clients = new ClientContext();
            Employees = new EmployeeContext();
            Stations = new StationContext();
            Seats = new SeatContext();
            Tickets = new TicketContext();
            TrainCompositions = new TrainCompositionContext();
            Trains = new TrainContext();
            TrainRoutes = new TrainRouteContext();
            Voyages = new VoyageContext();
            Wagons = new WagonContext();
            WagonSeats = new WagonSeatContext();

            Clients.Repository = _database.Client;
            Employees.Repository = _database.Employee;
            Stations.Repository = _database.Station;
            Seats.Repository = _database.Seat;
            Tickets.Repository = _database.Ticket;
            TrainCompositions.Repository = _database.TrainComposition;
            Trains.Repository = _database.Train;
            TrainRoutes.Repository = _database.TrainRoute;
            Voyages.Repository = _database.Voyage;
            Wagons.Repository = _database.Wagon;
            WagonSeats.Repository = _database.WagonSeat;
        }
    }
}
