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
        #region Initialization


        public Archer(Texture2D loadedTexture)
            : base(loadedTexture)
        {
            CurrHP = 30;
            TotalHP = 30;
            AttackPower = 20;
            AttackTypeVal = attackType.PROJECTILE;
            Armor = armorType.LIGHT;
            Range = 4;
            Special = new specialType[] {specialType.PROJECTILE};
            Movement = 2;
            CurrRecharge = 2;
            TotalRecharge = 2;
        }


        #endregion

        public override void attack(AllUnits victim)
        {
            victim.CurrHP = victim.applyMitigation(AttackPower, AttackTypeVal);
        }
    }
}
