﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRogue.Core.Entities.Interfaces
{
    public interface IDamageble : IKillable
    {
        float Health { get; set; }
        float HealthMax { get; set; }
        void Damage(float pure);
    }
}
