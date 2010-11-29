#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Angels_Vs_Demons.GameObjects;
using Angels_Vs_Demons.GameObjects.Units;
#endregion

namespace Angels_Vs_Demons.BoardObjects.Spells
{
    class Buff : Spell
    {
        public Buff(Vector2 newVictimPos, Vector2 newAttackerPos)
            : base(newVictimPos, newAttackerPos)
        {
        }
        public override void Cast(Unit VictimUnit, Champion CastingUnit)
        {
            CastingUnit.CurrMP -= (int)SpellValues.spellCost.BUFF;
            VictimUnit.Armor = armorType.IMBUED;
        }
    }
}
