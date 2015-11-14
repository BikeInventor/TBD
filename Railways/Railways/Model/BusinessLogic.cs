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
        /// <summary>
        /// Коэффициенты для рассчёта цены на конкретный тип места
        /// </summary>
        private enum PriceCoefficients
        {
            BERTH = 1,
            COUPE = 1.5,
            LUX = 2,
        }

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
        public static double CalculatePrice(double voyageDistance, WagonType seatType)
        {

            double basePrice = _kilometerPrice * voyageDistance;

            switch (seatType)
            {
                case WagonType.BERTH:
                    {
                        return basePrice * (double)PriceCoefficients.BERTH;
                    }
                case WagonType.COUPE:
                    {
                        return basePrice * (double)PriceCoefficients.COUPE;
                    }
                case WagonType.LUX:
                    {
                        return basePrice * (double)PriceCoefficients.LUX;
                    }
                default:
                    {
                        return basePrice * voyageDistance;
                    }
            }

        }


    }
}
