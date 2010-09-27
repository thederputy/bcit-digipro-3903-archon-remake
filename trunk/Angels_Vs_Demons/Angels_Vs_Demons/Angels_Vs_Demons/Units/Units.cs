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
    /// Types of attacks. All non-champion units have an attack type. Attack type interacts with AllUnits.armorType.
    /// </summary>
    public enum attackType
    {
        MELEE,
        PROJECTILE,
        MAGIC
    }

    /// <summary>
    /// Units represents all non-champion units.
    /// </summary>
    abstract class Units : AllUnits
    {
        public int attackPower;
        public attackType attackType;
        public int range;

        protected Units(Texture2D loadedTexture)
            : base(loadedTexture)
        {

        }
    }
}
