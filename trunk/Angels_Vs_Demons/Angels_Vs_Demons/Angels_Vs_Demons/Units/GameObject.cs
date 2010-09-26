#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace Angels_Vs_Demons
{
    class GameObject
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
    }
}
