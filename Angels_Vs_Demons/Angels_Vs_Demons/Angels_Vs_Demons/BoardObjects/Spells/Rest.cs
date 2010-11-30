#region Using Statements
using Angels_Vs_Demons.GameObjects.Units;
using Microsoft.Xna.Framework;
#endregion

namespace Angels_Vs_Demons.BoardObjects.Spells
{
    class Rest : Spell
    {
        public Rest(Vector2 newVictimPos, Vector2 newAttackerPos)
            : base(newVictimPos, newAttackerPos)
        {
        }
        public override void Cast(Unit VictimUnit, Champion CastingUnit)
        {
            CastingUnit.CurrMP = CastingUnit.TotalMP;
            CastingUnit.CurrRecharge = 5;
        }
    }
}
