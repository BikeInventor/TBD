using Railways.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railways.Model.Logic
{
    public static class VoyageSearchEngine
    {
        public static void FindVoyages(String depStationName, String arrStationName, DateTime depDate)
        {
            //var voyagesForThisDate = ContextKeeper.Voyages.Where(v => v.DepartureDateTime == depDate);
            var depStation = ContextKeeper.Stations.Where(station => station.StationName == depStationName).FirstOrDefault();
            var arrStation = ContextKeeper.Stations.Where(station => station.StationName == arrStationName).FirstOrDefault();

            var depRoutes = ContextKeeper.Routes.Where(route => route.StationId == depStation.Id);
            var depRoutesIds = depRoutes.Select(dr => dr.Id).ToList();

            var arrRoutes = ContextKeeper.Routes.Where(route => route.StationId == arrStation.Id);
            var arrRoutesIds = arrRoutes.Select(ar => ar.Id).ToList();
            
            var depVoyages = ContextKeeper.VoyageRoutes
                .Where(vr => depRoutesIds.Contains((int)vr.RouteId))
                .Select(vr => vr.VoyageId)
                .ToList();

            var arrVoyages = ContextKeeper.VoyageRoutes
                .Where(av => arrRoutesIds.Contains((int)av.RouteId))
                .Select(av => av.VoyageId)
                .ToList();

            var bothVoyages = ContextKeeper.Voyages
                .Where(voyage => (depVoyages.Contains(voyage.Id) && arrVoyages.Contains(voyage.Id)));

            var bothVoyagesIds = bothVoyages.Select(v => v.Id).ToList();

            var rightOrderVoyageIds = bothVoyagesIds.Where(vId =>
            {
                 var voyageRoutesOfVoyage = ContextKeeper.VoyageRoutes.Where(vr => vr.VoyageId == vId);
                 var routeIds = voyageRoutesOfVoyage.Select(vr => vr.RouteId);

                 int arrRouteId = ContextKeeper.Routes
                     .Where(r => r.StationId == arrStation.Id)
                     .Where(r => routeIds.Contains(r.Id))
                     .Select(r => r.Id)
                     .First();

                 int depRouteId = ContextKeeper.Routes
                     .Where(r => r.StationId == depStation.Id)
                     .Where(r => routeIds.Contains(r.Id))
                     .Select(r => r.Id)
                     .First();

                 if (depRouteId < arrRouteId) return true;
                 else return false;
             })
             .ToList();

            var goodTrainsIds = ContextKeeper.Voyages.Where(v => rightOrderVoyageIds.Contains(v.Id)).Select(v => v.TrainId).ToList();

            var goodTrains = ContextKeeper.Trains.Where(t => goodTrainsIds.Contains(t.Id)).ToList();

            Console.WriteLine("ПОДОШЛИ ВОЯДЖИ ПОЕЗДОВ:");
            goodTrains.ForEach(t => Console.WriteLine(t.TrainNum));

        }
    }
}
