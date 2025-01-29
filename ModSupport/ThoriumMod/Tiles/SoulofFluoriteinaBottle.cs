using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace TheDepths.ModSupport.ThoriumMod.Tiles
{
    [ExtendsFromMod("ThoriumMod"), JITWhenModsEnabled("ThoriumMod")]
    internal class SoulofFluoriteinaBottle : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.GetTileData(TileID.SoulBottles, 0));
            TileObjectData.addTile(Type);
            AnimationFrameHeight = 36;
            AddMapEntry(new Color(255, 180, 180), ModContent.GetInstance<Items.SoulofFluoriteinaBottle>().DisplayName);
            AdjTiles = [TileID.Torches];
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 0;
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            Tile tile = Main.tile[i, j];
            TileObjectData data = TileObjectData.GetTileData(tile);
            int x = i - tile.TileFrameX / 18 % data.Width;
            int topLeftY = j - tile.TileFrameY / 18 % data.Height;
            if (WorldGen.IsBelowANonHammeredPlatform(x, topLeftY))
            {
                offsetY -= 8;
            }
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            if (++frameCounter >= 6)
            {
                frameCounter = 0;
                if (++frame >= 4)
                {
                    frame = 0;
                }
            }
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.6f;
            g = 0.6f;
            b = 0.6f;
        }
    }
}
