﻿using SRogue.Core.Common.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRogue.Core.Entities.Interfaces
{
    public interface IControllable
    {
        void Interact(IInteractable target);
    }
}
