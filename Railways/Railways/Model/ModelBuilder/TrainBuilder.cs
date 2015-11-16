using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Railways.Model.Context;

namespace Railways.Model.ModelBuilder
{
    public enum WagonSeatsAmount
    {
        BERTH = 54,
        COUPE = 36,
        LUX = 18,
    }
    public static class TrainBuilder
    {
        /// <summary>
        /// Добавление вагона к поезду
        /// </summary>
        /// <param name="trainId"></param>
        /// <param name="wagonType"></param>
        public static void AddWagonToTrain(int trainId, WagonType wagonType)
        {
            var wagon = new Wagon();
            wagon.WagonType = (int)wagonType;
            wagon.SeatsAmount = GetSeatsAmount(wagonType);
            ContextKeeper.Wagons.Add(wagon);

            var trainComposition = new TrainComposition();
            trainComposition.TrainId = trainId;
            trainComposition.WagonNum = GetWagonNumber(trainId);
            trainComposition.WagonId = wagon.Id;
            ContextKeeper.TrainCompositions.Add(trainComposition);
        }

        /// <summary>
        /// Получение очередного номера вагона заданного поезда
        /// </summary>
        /// <param name="trainId"></param>
        /// <returns></returns>
        private static int GetWagonNumber(int trainId)
        {
            var lastWagonNumber =
                ContextKeeper.TrainCompositions
                .Where(composition => composition.TrainId == trainId)
                .Select(composition => composition.WagonNum)
                .Max();

            if (lastWagonNumber == null)
            {
                return 0;
            }
            else
            {
                return (int)lastWagonNumber++;
            }
        }

        /// <summary>
        /// Получение количества мест в вагоне по типу вагона
        /// </summary>
        /// <param name="wagonType"></param>
        /// <returns></returns>
        private static int GetSeatsAmount(WagonType wagonType)
        {
            switch (wagonType)
            {
                case WagonType.BERTH:
                    {
                        return (int)WagonSeatsAmount.BERTH;
                    }
                case WagonType.COUPE:
                    {
                        return (int)WagonSeatsAmount.COUPE;
                    }
                case WagonType.LUX:
                    {
                        return (int)WagonSeatsAmount.LUX;
                    }
            }
            return (int)WagonSeatsAmount.BERTH;
        }
    }
}
