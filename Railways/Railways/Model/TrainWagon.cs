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
    
    public partial class TrainWagon : Interfaces.IEntity
    {
        public int Id { get; set; }
        public Nullable<int> TrainId { get; set; }
        public Nullable<int> WagonId { get; set; }
    
        public virtual Train Train { get; set; }
        public virtual Wagon Wagon { get; set; }
    }
}
