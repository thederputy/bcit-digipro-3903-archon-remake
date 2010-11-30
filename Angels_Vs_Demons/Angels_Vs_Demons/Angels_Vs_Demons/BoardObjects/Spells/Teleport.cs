#region Using Statements
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
    }
}
