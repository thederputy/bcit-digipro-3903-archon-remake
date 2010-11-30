#region Using Statements
using Angels_Vs_Demons.GameObjects.Units;
using Microsoft.Xna.Framework;
#endregion

namespace Angels_Vs_Demons.BoardObjects
{
    abstract class Spell : Attack
    {

        public Spell(Vector2 newVictimPos, Vector2 newAttackerPos)
            : base(newVictimPos, newAttackerPos)
        {
        }

        /// <summary>
        /// Updates the isExecutable value.
        /// </summary>
        protected override void updateIsExecutable()
        {
            if (victimPos == null || attackerPos == null)
            {
                isExecutable = false;
            }
            else
            {
                isExecutable = true;
            }
        }

        public virtual void Cast(Unit VictimUnit, Champion CastingUnit)
        {

        }
    }
}
