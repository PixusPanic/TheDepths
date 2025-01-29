using CerebralMod;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Items.Placeable;

namespace TheDepths.ModSupport.CerebralMod.Items.Weapons.Defender
{
    [ExtendsFromMod("CerebralMod"), JITWhenModsEnabled("CerebralMod")]
    internal class WaterGeyserSentry : SentryItem
    {
        public override string LocalizationCategory => "CerebralMod.Items";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 14;
            Item.width = 32;
            Item.height = 32;
            Item.knockBack = 2f;
            Item.value = Terraria.Item.sellPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Blue;
            Item.shoot = ModContent.ProjectileType<Projectiles.Defender.WaterGeyserSentry>();
            yPlace = 9;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<WaterGeyser>(), 1)
            .AddRecipeGroup(RecipeGroupID.IronBar, 3)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
