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
    /// Represents a Imp or Soldier
    /// </summary>
    class Peon : Units
    {
        public Peon(Texture2D loadedTexture)
            : base(loadedTexture)
        {
            currHP = 40;
            totalHP = 40;
            attackType = attackType.MELEE;
            attackPower = 10;
            armor = armorType.MEDIUM;
            special = new specialType[] { specialType.NONE };
            range = 1;
            movement = 3;
            currRecharge = 0;
            totalRecharge = 1;
        }
    }
}
