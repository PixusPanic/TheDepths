using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.ModSupport.ThoriumMod.Items
{
    [ExtendsFromMod("ThoriumMod"), JITWhenModsEnabled("ThoriumMod")]
    internal class SoulofFluoriteinaBottle : ModItem
    {
        public override string LocalizationCategory => "ThoriumMod.Items";

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 28;
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = ItemRarityID.Orange;
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.SoulofFluoriteinaBottle>());
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Bottle);
            recipe.AddIngredient(null, "SoulofFluorite");
            recipe.Register();
        }
    }
}
