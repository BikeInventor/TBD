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
        /// Множители для рассчёта цены на конкретный тип места
        /// </summary>
        private enum PriceMultiplyers
        {
            BERTH = 2,
            COUPE = 3,
            LUX = 4,
        }
        /// <summary>
        /// Коэффициент для рассчёта цены на конретный тип места
        /// </summary>
        private static double priceCoefficient = 0.5;

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

            double basePrice = _kilometerPrice * voyageDistance * priceCoefficient;

            switch (seatType)
            {
                case WagonType.BERTH:
                    {
                        return basePrice * (double)PriceMultiplyers.BERTH;
                    }
                case WagonType.COUPE:
                    {
                        return basePrice * (double)PriceMultiplyers.COUPE;
                    }
                case WagonType.LUX:
                    {
                        return basePrice * (double)PriceMultiplyers.LUX;
                    }
                default:
                    {
                        return basePrice;
                    }
            }

        }


    }
}
