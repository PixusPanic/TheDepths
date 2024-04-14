using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using System.Collections.Generic;
using System;
using ReLogic.Content;
using Terraria.Graphics.Effects;

namespace TheDepths
{
    public class TheDepthsMenuTheme : ModMenu
	{
		public override string DisplayName => "Depths";
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Depths");

		public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("TheDepths/Assets/Title");

		public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor) //Taken from SLR 
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(default, default, SamplerState.PointClamp, default, default, default, Main.UIScaleMatrix);
			if (!Main.starGame)
			{
				for (int k = 4; k >= 0; k--)
				{
					Texture2D tex = ModContent.Request<Texture2D>("TheDepths/Backgrounds/DepthsUnderworldBG_" + k).Value;

					int realWidth = (k == 1 ? tex.Width / 2 : tex.Width);

					Rectangle? sourceRect = null;
					if (k == 1)
					{
						sourceRect = tex.Frame(2, 2, 1, 1);
					}

					float heightRatio = Main.screenHeight / (float)Main.screenWidth;
					int width = (int)(realWidth * heightRatio) * 2;
					var pos = new Vector2((int)(Main.screenPosition.X * 0.08f * -(k - 5)) % width, 0);

					Color color = Color.White;

					for (int h = 0; h < Main.screenWidth + width; h += width)
						Main.spriteBatch.Draw(tex, new Rectangle(h - (int)pos.X, (int)pos.Y, width, Main.screenHeight), sourceRect, color, 0, default, 0, 0);
				}
			}
			return true;
		}
	}
}