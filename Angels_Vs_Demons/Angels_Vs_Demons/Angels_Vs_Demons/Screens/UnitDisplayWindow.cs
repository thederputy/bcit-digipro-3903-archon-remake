using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Angels_Vs_Demons
{
    class UnitDisplayWindow : GameplayScreen
    {
        #region Fields
        Vector2 size;
        List<String> fields = new List<String>();
        string menuTitle;
        SpriteFont font;

        #endregion

        #region Properties
        public int HP
        {
            set { HP = value; }
        }
        #endregion

        #region Initialization

        public UnitDisplayWindow()
        {
            size.X = ScreenManager.screenWidth;
            size.Y = 200;

            //font = content.Load<SpriteFont>("DisplayFont");
            String Name = "Unit: ";
            String HP = "HP: ";
            String Recharge = "Recharge: ";
            String Armour = "Armour: ";
            String AP = "AP: ";
            String MOV = "MOV: ";

            fields.Add(Name);
            fields.Add(HP);
            fields.Add(Recharge);
            fields.Add(Armour);
            fields.Add(AP);
            fields.Add(MOV);
            
        }

        #endregion

        #region Draw and Update

        /// <summary>
        /// Draws display window.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.AliceBlue, 0, 0);
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            Vector2 position = new Vector2(100, 350);

            spriteBatch.Begin();

            // Draw the unit display window.
            Vector2 titlePosition = new Vector2(426, 80);
            Vector2 titleOrigin = new Vector2(10, 500);
            Color titleColor = new Color(192, 192, 192, TransitionAlpha);
            float titleScale = 1.25f;

            spriteBatch.DrawString(font, menuTitle, titlePosition, titleColor, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();

        #endregion
        }
    }
}
