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
    
    public partial class VoyageRoute : Interfaces.IEntity
    {
        public int Id { get; set; }
        public Nullable<int> RouteId { get; set; }
        public Nullable<int> VoyageId { get; set; }
    
        public virtual Route Route { get; set; }
        public virtual Voyage Voyage { get; set; }
    }
}
