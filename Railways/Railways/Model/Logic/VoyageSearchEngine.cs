using Railways.Model.Context;
using Railways.Model.ModelBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railways.Model.Logic
{
    public static class VoyageSearchEngine
    {

        private static List<TripInfo> suitableTrains = new List<TripInfo>();

        public static Task<List<TripInfo>> FindVoyagesAsync(String depStationName, String arrStationName, DateTime depDate)
        {
           return Task.Run(() =>
            {
                suitableTrains.Clear();

                //Ищем id станций отправления и прибытия по их названиям
                var depStation = ContextKeeper.Stations
                    .Where(station => station.StationName == depStationName)
                    .FirstOrDefault();
                var arrStation = ContextKeeper.Stations
                    .Where(station => station.StationName == arrStationName)
                    .FirstOrDefault();

                //Ищем id всех точек маршрута со станций отправления
                var depRoutesIds = ContextKeeper.Routes
                    .Where(route => route.StationId == depStation.Id)
                    .Select(dr => dr.Id)
                    .ToList();

                //Ищем id всех точек маршрута со станций прибытия
                var arrRoutesIds = ContextKeeper.Routes
                    .Where(route => route.StationId == arrStation.Id)
                    .Select(ar => ar.Id)
                    .ToList();

                //Ищем рейсы, в которых есть точки прибытия со станцией отправления
                var depVoyages = ContextKeeper.VoyageRoutes
                    .Where(vr => depRoutesIds.Contains((int)vr.RouteId))
                    .Select(vr => vr.VoyageId)
                    .ToList();

                //Ищем рейсы, в которых есть точки прибытия со станцией прибытия
                var arrVoyages = ContextKeeper.VoyageRoutes
                    .Where(av => arrRoutesIds.Contains((int)av.RouteId))
                    .Select(av => av.VoyageId)
                    .ToList();

                //Ищем рейсы, где есть обе точки
                var bothVoyagesIds = ContextKeeper.Voyages
                    .Where(voyage => (depVoyages.Contains(voyage.Id) && arrVoyages.Contains(voyage.Id)))
                    .Select(v => v.Id)
                    .ToList();

                //Отсеиваем рейсы, у которых станции идут в неправильном порядке (ст. прибытия раньше ст. отправления)
                //И рейсы с неподходящей датой
                var rightOrderVoyageIds = bothVoyagesIds.Where(vId =>
                {
                    // Ищем все маршруты для рейса c id vId
                    var routeIds = ContextKeeper.VoyageRoutes
                        .Where(vr => vr.VoyageId == vId)
                        .Select(vr => vr.RouteId);

                    // Ищем точку маршрута со станцией отправления
                    var arrRoute = ContextKeeper.Routes
                        .Where(r => r.StationId == arrStation.Id)
                        .Where(r => routeIds.Contains(r.Id))
                        .First();

                    // Ищем точку маршрута со станцией прибытия
                    var depRoute = ContextKeeper.Routes
                        .Where(r => r.StationId == depStation.Id)
                        .Where(r => routeIds.Contains(r.Id))
                        .First();

                    // Рейс подходит, если маршут отправления стоит раньше прибытия
                    if (depRoute.Id > arrRoute.Id) return false;

                    // Ищем раницу (в днях) между желаемой датой отправки и датой отправки в текущем проверяемом рейсе
                    var dateDifference = depDate.Date.Subtract(depRoute.DepartureTimeOffset.Value.Date).Days;

                    // Достаем саму поездку по ее Id
                    var voyage = ContextKeeper.Voyages
                        .Where(v => v.Id == vId)
                        .First();

                    // Рейс подходит, если разница в датах делится на периодичность рейса без остатка
                    if (dateDifference % voyage.Periodicity == 0)
                    {
                        suitableTrains.Add(TripInfo.BuildTripInfo(vId, depRoute.Id, arrRoute.Id, dateDifference));
                        return true;
                    }
                    else return false;
                })
                 .ToList();

                return suitableTrains;
            });
           
        }
    }
}
