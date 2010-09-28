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
    class Champion : AllUnits
    {
        public int totalMP;
        public int currMP;

        public Champion(Texture2D loadedTexture)
            : base(loadedTexture)
        {
            CurrHP = 100;
            totalHP = 100;
            currMP = 50;
            totalMP = 50;
            Armor = armorType.MAGIC;
            special = new specialType[] { specialType.FLYING, specialType.CHAMPION };
            movement = 2;
            CurrRecharge = 0;
            totalRecharge = 1;
        }

        /*
         * Blood Bolt / Lightning Bolt
         */
        public void Bolt()
        {

        }

        /*
         * Demonic Displacement / Angelic Transference
         */
        public void Swap()
        {

        }

        /*
         * Shackles / Light Net
         */
        public void Snare()
        {

        }

        /*
         * Demonic Empowerment / Blessing
         */
        public void Imbue()
        {

        }

        /*
         * Rest
         */
        public void Rest()
        {

        }
    }
}
