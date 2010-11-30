#region Using Statements
using System;
using Angels_Vs_Demons.GameObjects.Units;
using Microsoft.Xna.Framework;
#endregion

namespace Angels_Vs_Demons.BoardObjects.Spells
{
    class Teleport : Spell
    {
        public Teleport(Vector2 newVictimPos, Vector2 newAttackerPos)
            : base(newVictimPos, newAttackerPos)
        {
        }

        public override void Cast(Unit VictimUnit, Champion CastingUnit)
        {
            Console.WriteLine("THIS SHOULD NEVER GET CALLED!!!!!!!!!!!!!");
            throw new NotImplementedException();
        }
    }
}
