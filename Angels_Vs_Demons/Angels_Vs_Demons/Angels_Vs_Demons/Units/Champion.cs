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
    /// Champion represents a Arch Demon or Arch Angel. 
    /// </summary>
    class Champion : Unit
    {
        #region Properties


        public int TotalMP
        {
            get { return totalMP; }
            set { totalMP = value; }
        }

        private int totalMP;
        
        public int CurrMP
        {
            get { return currMP; }
            set { currMP = value; }
        }

        private int currMP;


        #endregion

        public Champion(Texture2D loadedTexture)
            : base(loadedTexture)
        {
            CurrHP = 100;
            TotalHP = 100;
            currMP = 50;
            totalMP = 50;
            Armor = armorType.MAGIC;
            Special = new specialType[] { specialType.FLYING, specialType.CHAMPION };
            Movement = 2;
            CurrRecharge = 0;
            TotalRecharge = 1;

            name = "Champion";
        }

        /*
         * 
         */
        /// <summary>
        /// Blood Bolt / Lightning Bolt (Bolt): removes 20 from the victim's (Unit) currHP. Removes 20 MP from 
        /// </summary>
        /// <param name="victim">Unit object targetted by this method.</param>
        public void Bolt(Unit victim)
        {
            victim.CurrHP = victim.applyMitigation(20, attackType.MAGIC);
            CurrMP -= 20;
        }

        /*
         * Demonic Displacement / Angelic Transference
         */
        public void Swap()
        {

        }
        
        /// <summary>
        /// Shackles / Light Net (Snare): adds 2 to the victim's (NonChampion) currRecharge
        /// </summary>
        /// <param name="victim">Unit object targetted by this method</param>
        public void Snare(NonChampion victim)
        {
            victim.CurrRecharge += 2;
            CurrMP -= 20;
        }

        /// <summary>
        /// Demonic Empowerment / Blessing (Imbue):
        /// </summary>
        public void Imbue()
        {

        }

        /// <summary>
        /// Rest: sets currMP to totalMP and sets currRecharge to 5.
        /// </summary>
        public void Rest()
        {
            CurrMP = TotalMP;
            CurrRecharge = 5;
        }
    }
}
