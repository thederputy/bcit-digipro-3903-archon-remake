#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Angels_Vs_Demons.GameObjects.Units;
using Angels_Vs_Demons.Screens.ScreenManagers;
using Angels_Vs_Demons.Screens.GameplayScreens;
#endregion

namespace Angels_Vs_Demons.Screens
{
    class UnitDisplayWindow : GameScreen
    {
        ContentManager content;
        SpriteBatch spritebatch;
        #region Fields
        SpriteFont font;
        Unit displayedUnit;
        Vector2 textPosition;
        Vector2 textOrigin;
        Color titleColor;
        float titleScale;
        String Name, HP, Recharge, Armor;

        #endregion

        #region Properties

        public UnitDisplayWindow(ContentManager Content)
        {
            content = Content;
            font = content.Load<SpriteFont>("MenuFont");

            textPosition = new Vector2(0, 0);
            textOrigin = new Vector2(0, 0);
            titleColor = new Color(0, 0, 0, 100);
            titleScale = 0.75f;

        }

        public Unit DisplayedUnit
        {
            get { return displayedUnit; }
            set { displayedUnit = value; }
        }

        #endregion

        #region Initialization

        public UnitDisplayWindow()
        {
            
            
        }

        #endregion

        #region Draw and Update

        /// <summary>
        /// Draws display window.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, Unit currentTileUnit)
        {
            //SpriteFont font = ScreenManager.Font;
            if (currentTileUnit != null)
            {
                displayedUnit = currentTileUnit;
                Name = "Name: " + currentTileUnit.Name;
                HP = "HP: " + currentTileUnit.CurrHP;
                Recharge = "Recharge: " + currentTileUnit.CurrRecharge;
                Armor = "Armor: " + currentTileUnit.Armor;
                    
            }
            else
            {
                Name = "Name: ";
                HP = "HP: ";
                Recharge = "Recharge: ";
                Armor = "Armor: ";
            }
            
            spritebatch = spriteBatch;

            Vector2 position = new Vector2(100, 350);

            spritebatch.Begin();

            textPosition.Y = 0;
            textOrigin.Y = 0;

            spritebatch.DrawString(font, Name, textPosition, titleColor, 0,
                                   textOrigin, titleScale, SpriteEffects.None, 0);

            textPosition.Y += font.LineSpacing;

            spritebatch.DrawString(font, HP, textPosition, titleColor, 0,
                                   textOrigin, titleScale, SpriteEffects.None, 0);

            textPosition.Y += font.LineSpacing;

            spritebatch.DrawString(font, Recharge, textPosition, titleColor, 0,
                                   textOrigin, titleScale, SpriteEffects.None, 0);

            textPosition.Y += font.LineSpacing;

            spritebatch.DrawString(font, Armor, textPosition, titleColor, 0,
                                   textOrigin, titleScale, SpriteEffects.None, 0);

            spritebatch.End();

        #endregion
        }
    }
}
