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
    
    public partial class Ticket : Interfaces.IEntity
    {
        public int Id { get; set; }
        public Nullable<int> SeatId { get; set; }
        public Nullable<int> ClientId { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<int> DepartureRouteId { get; set; }
        public Nullable<int> ArrivalRouteId { get; set; }
        public Nullable<System.DateTime> DepartureDate { get; set; }
        public Nullable<System.DateTime> ArrivalDate { get; set; }
    
        public virtual Seat Seat { get; set; }
        public virtual Client Client { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Route Route { get; set; }
        public virtual Route Route1 { get; set; }
    }
}
