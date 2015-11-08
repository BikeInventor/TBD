using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railways.Model.Context
{
    /// <summary>
    /// Контекст билета с репозиторием "TicketSet"
    /// </summary>
    public class TicketContext : ContextBase<Ticket, RailwayDataEntities>
    {
    }
}
