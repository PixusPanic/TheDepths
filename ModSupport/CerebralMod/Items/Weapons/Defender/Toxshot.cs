using CerebralMod;
using CerebralMod.Items.Materials;
using CerebralMod.Items.Weapons.Defender.PreHardmode;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Items.Placeable;

namespace TheDepths.ModSupport.CerebralMod.Items.Weapons.Defender
{
    [ExtendsFromMod("CerebralMod"), JITWhenModsEnabled("CerebralMod")]
    internal class Toxshot : SentryItem
    {
        public override string LocalizationCategory => "CerebralMod.Items";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 30;
            Item.width = 36;
            Item.height = 36;
            Item.knockBack = 4;
            Item.value = Terraria.Item.sellPrice(0, 0, 54, 0);
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<Projectiles.Defender.Toxshot>();
            Item.shootSpeed = 1f;
            isTurret = true;
            yPlace = 18;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<ArqueriteBar>(), 15)
            .AddTile(TileID.Anvils)
            .Register();

            Recipe NightTurret = Recipe.Create(ModContent.ItemType<NightTurret>())
            .AddIngredient(ModContent.ItemType<DemoniteTurret>())
            .AddIngredient(ModContent.ItemType<JungleTurret>())
            .AddIngredient(this)
            .AddIngredient(ModContent.ItemType<NightCrafter>())

            .AddTile(TileID.DemonAltar)
            .Register();

            NightTurret = Recipe.Create(ModContent.ItemType<NightTurret>())
            .AddIngredient(ModContent.ItemType<DemoniteTurret>())
            .AddIngredient(ModContent.ItemType<JungleTurret>())
            .AddIngredient(this)
            .AddIngredient(ModContent.ItemType<NightCrafter>())

            .AddTile(TileID.DemonAltar)
            .Register();
        }
    }
}
