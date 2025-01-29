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
    internal class MercurySpikes : SentryItem
    {
        public override string LocalizationCategory => "CerebralMod.Items";

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 24;
            Item.width = 32;
            Item.height = 32;
            Item.value = Terraria.Item.sellPrice(0, 0, 54, 0);
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<Projectiles.Defender.MercurySpikes>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ArqueriteBar>(), 15)
                .AddTile(TileID.Anvils)
                .Register();

            Recipe NightSpikes = Recipe.Create(ModContent.ItemType<NightSpikes>())
                .AddIngredient(ModContent.ItemType<DemoniteSpikes>())
                .AddIngredient(ModContent.ItemType<JungleSpikes>())
                .AddIngredient(ModContent.ItemType<DungeonSpikes>())
                .AddIngredient(this)
                .AddIngredient(ModContent.ItemType<NightCrafter>())

                .AddTile(TileID.DemonAltar)
                .Register();

            NightSpikes = Recipe.Create(ModContent.ItemType<NightSpikes>())
                .AddIngredient(ModContent.ItemType<CrimtaneSpikes>())
                .AddIngredient(ModContent.ItemType<JungleSpikes>())
                .AddIngredient(ModContent.ItemType<DungeonSpikes>())
                .AddIngredient(this)
                .AddIngredient(ModContent.ItemType<NightCrafter>())

                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}
