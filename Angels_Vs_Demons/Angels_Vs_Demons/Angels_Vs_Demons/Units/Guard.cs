#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Angels_Vs_Demons
{
    /// <summary>
    /// Guard represents either a Blood Guard or Angelic Guard
    /// </summary>
    class Guard : Units
    {
        public Guard(Texture2D loadedTexture)
            : base(loadedTexture)
        {
            currHP = 50;
            totalHP = 50;
            attackPower = 25;
            attackType = attackType.MELEE;
            armor = armorType.HEAVY;
            range = 2;
            special = new specialType[]{specialType.HULKING};
            movement = 3;
            currRecharge = 0;
            totalRecharge = 3;
        }
    }
}
