using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Angels_Vs_Demons
{
    /// <summary>
    /// Mage represents either a Demon Lord or High Angel
    /// </summary>
    class Mage : Units
    {
        public Mage(Texture2D loadedTexture)
            : base(loadedTexture)
        {
            currHP = 30;
            totalHP = 30;
            attackPower = 30;
            attackType = attackType.MAGIC;
            armor = armorType.MAGIC;
            range = 3;
            special = new specialType[] { specialType.SPLASH, specialType.PROJECTILE };
            movement = 3;
            currRecharge = 0;
            totalRecharge = 4;
        }
    }
}
