//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Railways.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Train
    {
        public Train()
        {
            this.TrainComposition = new HashSet<TrainComposition>();
            this.TrainRoute = new HashSet<TrainRoute>();
            this.Voyage = new HashSet<Voyage>();
        }
    
        public int Id { get; set; }
        public int TrainNum { get; set; }
        public System.TimeSpan DepartureTime { get; set; }
        public int DepartureStationID { get; set; }
        public System.TimeSpan ArrivalTime { get; set; }
        public int ArrivalStationID { get; set; }
    
        public virtual Station Station { get; set; }
        public virtual ICollection<TrainComposition> TrainComposition { get; set; }
        public virtual ICollection<TrainRoute> TrainRoute { get; set; }
        public virtual ICollection<Voyage> Voyage { get; set; }
    }
}
