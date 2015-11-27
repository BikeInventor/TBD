using Railways.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railways.Model.ModelBuilder
{
    public static class VoyageBuilder
    {
        /// <summary>
        /// Поиск рейса заданного поезда
        /// </summary>
        /// <param name="trainId"></param>
        /// <returns></returns>
        public static Voyage GetVoyageOfTrain(int trainId)
        {
            var voyageOfTrain = ContextKeeper.Voyages
                .Where(voyage => voyage.TrainId == trainId)
                .FirstOrDefault();

            return voyageOfTrain as Voyage;
        }

        /// <summary>
        /// Связывание рейса с поездом
        /// </summary>
        /// <param name="voyageId"></param>
        /// <param name="trainId"></param>
        public static void ConnectVoyageToTrain(int voyageId, int trainId)
        {
            var voyage = ContextKeeper.Voyages.First(v => v.Id == voyageId);
            voyage.TrainId = trainId;
        }
        /// <summary>
        /// Получение всех точек маршрута заданного рейса
        /// </summary>
        /// <param name="voyageId"></param>
        /// <returns></returns>
        public static IQueryable<Route> GetRoutesOfVoyage(int voyageId)
        {
            var routesOfVoyage = ContextKeeper.VoyageRoutes
                .Where(vr => vr.VoyageId == voyageId).Select(vr => vr.RouteId);
            var routes = ContextKeeper.Routes
                .Where(route => routesOfVoyage.Contains(route.Id));
            return routes;
        }
        /// <summary>
        /// Добавление точки маршрута к заданному рейсу
        /// </summary>
        /// <param name="voyageId"></param>
        public static void AddRouteToVoyage(int voyageId, int routeId)
        {
            var routeOfVoyage = new VoyageRoute();
            routeOfVoyage.VoyageId = voyageId;
            routeOfVoyage.RouteId = routeId;
            ContextKeeper.VoyageRoutes.Add(routeOfVoyage);
        }
        /// <summary>
        /// Удаление последней точки маршрута заданного рейса
        /// </summary>
        /// <param name="voyageId"></param>
        public static void DeleteLastRouteOfVoyage(int voyageId)
        {
            var routes = ContextKeeper.VoyageRoutes
                   .Where(vr => vr.VoyageId == voyageId)
                   .Select(vr => vr.RouteId);
            var lastRouteId = routes.Max();
            var lastVoyageRoute = ContextKeeper.VoyageRoutes.First(vr => vr.RouteId == lastRouteId);
            var lastRoute = ContextKeeper.Routes.First(r => r.Id== lastRouteId);
            ContextKeeper.VoyageRoutes.Remove(lastVoyageRoute);
            ContextKeeper.Routes.Remove(lastRoute); 
        }

    }
}
