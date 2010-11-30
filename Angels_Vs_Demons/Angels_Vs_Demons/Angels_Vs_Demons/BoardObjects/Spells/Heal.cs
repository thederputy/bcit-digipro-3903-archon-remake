#region Using Statements
using Angels_Vs_Demons.GameObjects.Units;
using Microsoft.Xna.Framework;
#endregion

namespace Angels_Vs_Demons.BoardObjects.Spells
{
    class Heal : Spell
    {
        public Heal(Vector2 newVictimPos, Vector2 newAttackerPos)
            : base(newVictimPos, newAttackerPos)
        {
        }
        public override void Cast(Unit VictimUnit, Champion CastingUnit)
        {
            CastingUnit.CurrMP -= (int)SpellValues.spellCost.HEAL;
            VictimUnit.CurrHP = VictimUnit.TotalHP;
        }
    }
}
