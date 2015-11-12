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
    
    public partial class TrainRoute
    {
        public int Id { get; set; }
        public Nullable<int> TrainId { get; set; }
        public Nullable<int> StationId { get; set; }
        public Nullable<System.DateTime> ArrivalTime { get; set; }
        public Nullable<System.DateTime> DepartureTime { get; set; }
        public Nullable<double> Distance { get; set; }
    
        public virtual Station Station { get; set; }
        public virtual Train Train { get; set; }
    }
}
