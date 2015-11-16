using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Railways.Model.Context;
using Railways.Model.ModelBuilder;

namespace Railways.Model
{
    public static class TestDataLoader
    {
        public static void AddTestTrain()
        {
            var train = new Train();
            train.TrainNum = "107C";
            ContextKeeper.Trains.Add(train);

            TrainBuilder.AddWagonToTrain(train.Id, WagonType.COUPE);

            Console.WriteLine(TrainBuilder.GetWagonsCount(train.Id));
        }
    }
}
