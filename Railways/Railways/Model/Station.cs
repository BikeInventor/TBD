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
    
    public partial class Station
    {
        public Station()
        {
            this.Train = new HashSet<Train>();
            this.TrainRoute = new HashSet<TrainRoute>();
        }
    
        public int Id { get; set; }
        public string StationName { get; set; }
    
        public virtual ICollection<Train> Train { get; set; }
        public virtual ICollection<TrainRoute> TrainRoute { get; set; }
    }
}