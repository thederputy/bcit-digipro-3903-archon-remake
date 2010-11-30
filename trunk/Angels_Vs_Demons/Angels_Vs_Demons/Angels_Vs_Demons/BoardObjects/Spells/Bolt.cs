#region Using Statements
using Angels_Vs_Demons.GameObjects.Units;
using Microsoft.Xna.Framework;
#endregion

namespace Angels_Vs_Demons.BoardObjects.Spells
{
    class Bolt : Spell
    {
        public Bolt(Vector2 newVictimPos, Vector2 newAttackerPos)
            : base(newVictimPos, newAttackerPos)
        {

        }
        public override void Cast(Unit VictimUnit, Champion CastingUnit)
        {
            CastingUnit.CurrMP -= (int)SpellValues.spellCost.BOLT;
            VictimUnit.CurrHP -= 20;
            if (VictimUnit.CurrHP < 0)
            {
                VictimUnit.CurrHP = 0;
            }
        }
    }
}
