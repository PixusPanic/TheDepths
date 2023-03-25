using Terraria;
using TheDepths.Items.Placeable;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheDepths.Items.Placeable
{
	public class ShaleBricks : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
			if (ModContent.GetInstance<TheDepthsClientConfig>().SlateConfig)
			{
				DisplayName.SetDefault("Slate Bricks");
			}
			else
            {
				DisplayName.SetDefault("Shale Bricks");
			}
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.ShaleBricks>();
			Item.width = 12;
			Item.height = 12;
			Item.rare = ItemRarityID.White;
		}
		
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.ShaleBlock>(), 2);
			recipe.AddTile(TileID.Furnaces);
			recipe.Register();
		}
	}
}
