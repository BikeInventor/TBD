using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Railways.Model.Context;

namespace Railways.View
{
    class VoyageView
    {
        public int VoyageId { get; private set; }
        public string TrainNum { get; private set; }
        public string DepartureStation { get; private set; }
        public string ArrivalStation { get; private set; }
        public DateTime DepartureDate { get; private set; }
        public DateTime ArrivalDate { get; private set; }
        public int? Periodicity { get; private set; }
        public bool IsLuxSeatsAvailabe { get; private set; }
        public bool IsCoupeSeatsAvailable { get; private set; }     
        public bool IsBerthSeatsAvailable { get; private set; }
        public float LuxPrice{get;private set;}
        public float CoupePrice { get; private set; }
        public float BerthPrice { get; private set; }

        public VoyageView(int _voyageId) 
        {
            VoyageId = _voyageId;
        }

        private void InitializeProperties()
        {
            var Voyage=ContextKeeper.Voyages.FindBy(voyage=> voyage.Id==VoyageId).First();
            var Train=ContextKeeper.Trains.FindBy(train => train.Id == Voyage.TrainId).First();
            TrainNum = ContextKeeper.Trains.First(train => train.Id == Voyage.TrainId).TrainNum;
            DepartureStation = ContextKeeper.Stations.First(station => station.Id == Train.DepartureStationID).StationName;
            ArrivalStation = ContextKeeper.Stations.First(station => station.Id == Train.ArrivalStationID).StationName;
            Periodicity = Train.Periodicity;
        }

        public static VoyageView VoyageView.FormView(int id)
        {
            return new VoyageView(id);
        }


    }
}
