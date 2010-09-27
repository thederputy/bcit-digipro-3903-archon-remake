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
    /// Types of armor units can have. All units have 1 armor type.
    /// </summary>
    public enum armorType
    {
        HEAVY,
        MEDIUM,
        LIGHT,
        MAGIC,
        IMBUED
    }

    /// <summary>
    /// Types of special properties units can have. All units have one or more special properties. 
    /// NONE indicates no special properties on that unit. 
    /// </summary>
    public enum specialType
    {
        NONE,
        FLYING,
        SPLASH,
        PROJECTILE,
        HULKING,
        CHAMPION
    }

    /// <summary>
    /// AllUnits represents all unit objects.
    /// </summary>
    abstract class AllUnits : GameObject
    {
        public int totalHP;
        public int currHP;
        public int movement;
        public armorType armor;
        public specialType[] special;
        public int totalRecharge;
        public int currRecharge;

        protected AllUnits(Texture2D loadedTexture)
            : base(loadedTexture)
        {

        }
    }
}
