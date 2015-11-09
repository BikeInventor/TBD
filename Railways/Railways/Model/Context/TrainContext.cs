﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railways.Model.Context
{
    /// <summary>
    /// Контекст поезда с репозиторием "TrainSet"
    /// </summary>
    public class TrainContext : ContextBase<Train, RailwayDataEntities>
    {
    }
}