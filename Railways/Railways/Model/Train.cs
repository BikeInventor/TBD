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
    
    public partial class Train : Interfaces.IEntity
    {
        public Train()
        {
            this.Train_Wagon = new HashSet<TrainWagon>();
            this.Voyage = new HashSet<Voyage>();
        }
    
        public int Id { get; set; }
        public string TrainNum { get; set; }
    
        public virtual ICollection<TrainWagon> Train_Wagon { get; set; }
        public virtual ICollection<Voyage> Voyage { get; set; }
    }
}
