using CerebralMod;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Items.Placeable;

namespace TheDepths.ModSupport.CerebralMod.Items.Weapons.Esper
{
    [ExtendsFromMod("CerebralMod"), JITWhenModsEnabled("CerebralMod")]
    internal class GeoTwirler : ECItem
    {
        public override string LocalizationCategory => "CerebralMod.Items";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 15;
            Item.width = 26;
            Item.height = 30;
            Item.knockBack = 2f;
            Item.value = Terraria.Item.sellPrice(0, 0, 39, 0);
            Item.rare = ItemRarityID.Blue;
            Item.shoot = ModContent.ProjectileType<Projectiles.Esper.GeoTwirler>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<GeoTorch>(), 20)
            .AddIngredient(ModContent.ItemType<Geode>(), 5)
            .AddTile(ModContent.TileType<Tiles.Gemforge>())
            .Register();
        }
    }
}
