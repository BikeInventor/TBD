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
    
    public partial class Ticket
    {
        public Ticket()
        {
            this.Voyage = new HashSet<Voyage>();
        }
    
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int TrainId { get; set; }
        public int WagonId { get; set; }
        public int SeatId { get; set; }
        public double Price { get; set; }
    
        public virtual Client Client { get; set; }
        public virtual ICollection<Voyage> Voyage { get; set; }
    }
}