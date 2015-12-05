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
    public static class ContextKeeper
    {
        private static RailwayDataModelContainer _database;

        public static RailwayDataModelContainer DataBase
        {
            get
            {
                return _database;
            }
        }

        public static ClientContext Clients {get; private set;}
        public static EmployeeContext Employees { get; private set; }
        public static StationContext Stations { get; private set; }
        public static SeatContext Seats { get; private set; }
        public static TicketContext Tickets { get; private set; }
        public static TrainWagonContext TrainWagons { get; private set; }
        public static TrainContext Trains { get; private set; }
        public static RouteContext Routes { get; private set; }
        public static VoyageContext Voyages { get; private set; }
        public static VoyageRouteContext VoyageRoutes { get; private set; }
        public static WagonContext Wagons { get; private set; }
        public static WagonSeatContext WagonSeats { get; private set; }

        public static Task Initialize()
        {
            return Task.Run(() =>
            {
                System.Threading.Thread.Sleep(2000);

                _database = new RailwayDataModelContainer();

                Clients = new ClientContext();
                Employees = new EmployeeContext();
                Stations = new StationContext();
                Seats = new SeatContext();
                Tickets = new TicketContext();
                TrainWagons = new TrainWagonContext();
                Trains = new TrainContext();
                Routes = new RouteContext();
                VoyageRoutes = new VoyageRouteContext();
                Voyages = new VoyageContext();
                Wagons = new WagonContext();
                WagonSeats = new WagonSeatContext();


                Clients.Repository = _database.ClientSet;
                Employees.Repository = _database.EmployeeSet;
                Stations.Repository = _database.StationSet;
                Seats.Repository = _database.SeatSet;
                Tickets.Repository = _database.TicketSet;
                TrainWagons.Repository = _database.TrainWagonSet;
                Trains.Repository = _database.TrainSet;
                VoyageRoutes.Repository = _database.VoyageRouteSet;
                Routes.Repository = _database.RouteSet;
                Voyages.Repository = _database.VoyageSet;
                Wagons.Repository = _database.WagonSet;
                WagonSeats.Repository = _database.WagonSeatSet;
            });
            
        }
    }
}
