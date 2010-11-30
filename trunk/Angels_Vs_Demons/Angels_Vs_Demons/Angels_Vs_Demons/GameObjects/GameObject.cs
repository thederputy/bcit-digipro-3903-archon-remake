#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Angels_Vs_Demons.GameObjects
{
    class GameObject : ICloneable
    {
        public Texture2D sprite;
        public Vector2 position;
        public float direction;
        public Rectangle rect;

        public GameObject(Texture2D loadedTexture)
        {
            direction = 0.0f;
            position = Vector2.Zero;
            sprite = loadedTexture;
        }

        /// <summary>
        /// Performs a deep clone of the GameObject.
        /// </summary>
        /// <returns>A new GameObject instance populated with the same data as this GameObject.</returns>
        public virtual Object Clone()
        {
            GameObject other = this.MemberwiseClone() as GameObject;
            other.position = new Vector2(this.position.X, this.position.Y);
            other.rect = new Rectangle(this.rect.X, this.rect.Y, this.rect.Width, this.rect.Height);
            return other;
        }
    }
}
