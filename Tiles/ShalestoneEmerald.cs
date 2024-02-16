using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheDepths.Dusts;

namespace TheDepths.Tiles
{
	public class ShalestoneEmerald : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			DustType = ModContent.DustType<ShaleDust>();
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(11, 147, 34), name);
			Main.tileMerge[Type][ModContent.TileType<ShaleBlock>()] = true;
			Main.tileMerge[Type][ModContent.TileType<Shalestone>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneDiamond>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneAmethyst>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneRuby>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneSapphire>()] = true;
			Main.tileMerge[Type][ModContent.TileType<ShalestoneTopaz>()] = true;
			Main.tileMerge[Type][ModContent.TileType<OnyxShalestone>()] = true;

			HitSound = SoundID.Tink;
			MinPick = 65;
			RegisterItemDrop(ItemID.Emerald);
			Main.tileSpelunker[Type] = true;
		}

		public override bool CanDrop(int i, int j)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemID.Emerald);
			return false;
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
}