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
    /// Archer represents either a Skeleton Archer or Chosen One.
    /// </summary>
    class Archer : Units
    {
        public Archer(Texture2D loadedTexture)
            : base(loadedTexture)
        {
            currHP = 30;
            totalHP = 30;
            attackPower = 20;
            attackType = attackType.PROJECTILE;
            armor = armorType.LIGHT;
            range = 4;
            special = new specialType[] {specialType.PROJECTILE};
            movement = 2;
            currRecharge = 2;
            totalRecharge = 2;
        }
    }
}
