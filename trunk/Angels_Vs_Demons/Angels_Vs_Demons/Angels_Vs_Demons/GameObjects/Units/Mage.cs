#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Angels_Vs_Demons.Players;
#endregion

namespace Angels_Vs_Demons.GameObjects.Units
{
    /// <summary>
    /// Mage represents either a Demon Lord or High Angel
    /// </summary>
    class Mage : NonChampion
    {
        #region Initialization


        public Mage(Texture2D loadedTexture, Faction factionType, string name, int id)
            : base(loadedTexture, factionType, name, id)
        {
            CurrHP = 30;
            TotalHP = 30;
            AttackPower = 30;
            AttackTypeVal = attackType.MAGIC;
            Armor = armorType.MAGIC;
            Range = 3;
            Special = new specialType[] { specialType.SPLASH, specialType.PROJECTILE };
            Movement = 3;
            CurrRecharge = 0;
            totalRecharge = 4;
        }


        #endregion

        /// <summary>
        /// Overriddev attack methos for the mage as he does splash damage.
        /// </summary>
        /// <param name="victim">the unit we are attacking</param>
        public override void attack(Unit victim)
        {
            victim.CurrHP -= victim.applyMitigation(AttackPower, AttackTypeVal);
        }
    }
}
