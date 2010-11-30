#region Using Statements
using System;
using Angels_Vs_Demons.Players;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Angels_Vs_Demons.GameObjects.Units
{
    /// <summary>
    /// Guard represents either a Blood Guard or Angelic Guard
    /// </summary>
    class Guard : NonChampion
    {
        #region Initialization


        public Guard(Texture2D loadedTexture, Faction factionType, string name, int id)
            : base(loadedTexture, factionType, name, id)
        {
            CurrHP = 50;
            TotalHP = 50;
            AttackPower = 25;
            AttackTypeVal = attackType.MELEE;
            Armor = armorType.HEAVY;
            CurrArmor = Armor;
            Range = 2;
            Special = new specialType[]{specialType.HULKING};
            Movement = 2;
            CurrRecharge = 0;
            totalRecharge = 4;
        }


        #endregion

        /// <summary>
        /// Performs a deep clone of the Guard.
        /// </summary>
        /// <returns>A new Guard instance populated with the same data as this Guard.</returns>
        public override Object Clone()
        {
            Guard other = base.Clone() as Guard;
            return other;
        }

        /// <summary>
        /// Calls base applyMitigation (see: Unit.applyMitigation) and applies hulking property
        /// (damage in excess of 20 is ignored).
        /// </summary>
        /// <param name="attackerAP">beginning attack power.</param>
        /// <param name="attackerType">attack type to apply mitigation for.</param>
        /// <returns>the altered attack power.</returns>
        public override int applyMitigation(int attackerAP, attackType attackerType)
        {
            int effectiveAP;

            effectiveAP = base.applyMitigation(attackerAP, attackerType);

            if (effectiveAP > 20)
            {
                effectiveAP = 20;
            }

            return effectiveAP;
        }
    }
}
