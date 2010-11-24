using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Angels_Vs_Demons.GameObjects;

namespace Angels_Vs_Demons.BoardObjects.Spells
{
    class Heal : Spell
    {
        public Heal(Tile newVictimTile, Tile newAttackerTile)
            : base(newVictimTile, newAttackerTile)
        {
        }
    }
}
