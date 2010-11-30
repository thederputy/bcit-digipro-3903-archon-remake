#region Using Statements
using System;
using Angels_Vs_Demons.Players;
using Microsoft.Xna.Framework.Graphics;
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
            CurrArmor = Armor;
            Range = 3;
            Special = new specialType[] { specialType.SPLASH, specialType.PROJECTILE };
            Movement = 3;
            CurrRecharge = 0;
            totalRecharge = 6;
        }

        #endregion

        /// <summary>
        /// Overridden attack method for the mage as he does splash damage.
        /// </summary>
        /// <param name="victim">the unit we are attacking</param>
        public override void attack(Unit victim)
        {
            victim.CurrHP -= victim.applyMitigation(AttackPower / 2, AttackTypeVal);
            victim.CurrHP -= victim.applyMitigation(AttackPower / 2, attackType.PROJECTILE);
            if (victim.CurrHP < 0)
            {
                victim.CurrHP = 0;
            }
        }

        /// <summary>
        /// Performs a deep clone of the Mage.
        /// </summary>
        /// <returns>A new Mage instance populated with the same data as this Mage.</returns>
        public override Object Clone()
        {
            Mage other = base.Clone() as Mage;
            return other;
        }
    }
}
