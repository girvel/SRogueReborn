﻿using SRogue.Core.Common;
using SRogue.Core.Common.TickEvents;
using SRogue.Core.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRogue.Core.Entities.Concrete.Tiles
{
    public class SpikeTrap : Tile
    {
        protected virtual bool Active { get; set; }

        public override void OnStep(IUnit unit)
        {
            Active = true;
            GameManager.Current.OnTickEndEvents.Add(new EventSpikeTrapDamage(unit));
        }

        public override char Texture
        {
            get
            {
                if (Active)
                    return Assets.SpikeTrap_Active;

                return base.Texture;
            }
        }
    }
}
