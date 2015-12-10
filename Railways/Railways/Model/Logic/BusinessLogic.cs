﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Railways.Model.Context;
using Railways.Model.ModelBuilder;
namespace Railways.Model.Logic
{


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
        private static double priceCoefficient = 0.2;

        /// <summary>
        /// Цена на верхнее место в процентах от цены на нижнее
        /// </summary>
        private static double upperSeatCoefficient = 0.95;

        private static double _kilometerPrice = 4;
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

        /// <summary>
        /// Установка цены на верхнее место в купе
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public static double SetUpperSeatPrice(double price)
        {
            return price * upperSeatCoefficient;
        }

        /// <summary>
        /// Установка цены на нижнее место в купе
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public static double SetLowerSeatPrice(double price)
        {
            return price / upperSeatCoefficient;
        }


    }
}
