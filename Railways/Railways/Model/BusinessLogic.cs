using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Railways.Model.Context;

namespace Railways.Model
{
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
    /// Бизнес-логика системы
    /// </summary>
    public class BusinessLogic
    {
        private static double _kilometerPrice;
        /// <summary>
        /// Цена за километр поездки
        /// </summary>
        public static double KilometerPrice
        {
            get { return _kilometerPrice; }
            set { _kilometerPrice = value; }
        }
        /// <summary>
        /// Подсчет цены места заданного типа для конкретного маршрута
        /// </summary>
        /// <param name="trainId"></param>
        /// <param name="seatType"></param>
        /// <returns></returns>
        public static double CalculatePrice(int trainId, WagonType seatType)
        {
            var distance = (double)ContextKeeper.TrainRoutes
                .Where(trainRoute => trainRoute.Id == trainId)
                .Select(trainRoute => trainRoute.Distance).First();
            switch (seatType)
            {
                case WagonType.BERTH:
                    {
                        return _kilometerPrice * distance;
                    }
                case WagonType.COUPE:
                    {
                        return _kilometerPrice * distance * 1.5;
                    }
                case WagonType.LUX:
                    {
                        return _kilometerPrice * distance * 2;
                    }
                default:
                    {
                        return _kilometerPrice * distance;
                    }
            }

        }


    }
}
