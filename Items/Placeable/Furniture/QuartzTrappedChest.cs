using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Items.Placeable.Furniture
{
    public class QuartzTrappedChest : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.TrapSigned[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Terraria.Item.sellPrice(silver: 5);
            Item.createTile = ModContent.TileType<Tiles.Furniture.QuartzTrappedChest>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<QuartzChest>()).AddIngredient(ItemID.Wire, 10).AddTile(TileID.HeavyWorkBench).Register();
        }
    }
}