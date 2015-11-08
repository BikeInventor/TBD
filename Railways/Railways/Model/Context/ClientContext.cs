using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railways.Model.Context
{
    /// <summary>
    /// Контекст клиента с репозиторием "ClientSet"
    /// </summary>
    public class ClientContext : ContextBase<Client, RailwayDataEntities>
    {
    }
}
