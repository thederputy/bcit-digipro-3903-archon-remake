#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
#endregion

namespace Angels_Vs_Demons.Players
{
    class HumanPlayer : Player
    {
        private Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public HumanPlayer(Faction faction)
            :base(faction)
        {
        }
    }
}
