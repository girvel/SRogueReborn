﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRogue.Core.Common.Items.Bases
{
    public class ArmorBase : ItemBase
    {
        public int Armor { get; set; }

        public int MagicResist { get; set; }

        public override string Name
        {
            get
            {
                if (isEmpty)
                    return base.Name;

                return base.Name + " ({0} arm, {1} res)".FormatWith(Armor, MagicResist);   
            }
        }

        public ArmorBase(int armor, int resist, ItemType slot)
            : base()
        {
            switch (Material)
            {
                case ItemMaterial.Wooden:
                case ItemMaterial.Iron:
                case ItemMaterial.Steel:
                    Armor = armor + (int)Material + (int)Quality;
                    MagicResist = 0;
                    break;
                case ItemMaterial.Glass:
                case ItemMaterial.Golden:
                    Armor = 0;
                    MagicResist = resist + (int)Material + (int)Quality;
                    break;
                case ItemMaterial.Diamond:
                case ItemMaterial.Torium:
                    Armor = armor + (int)Material + (int)Quality;
                    MagicResist = resist + (int)Material + (int)Quality;
                    break;
                default:
                    break;
            }
            Slot = slot;
        }
    }
}
