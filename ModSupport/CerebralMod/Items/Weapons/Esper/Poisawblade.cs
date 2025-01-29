using CerebralMod;
using CerebralMod.Items.Materials;
using CerebralMod.Items.Weapons.Esper.PreHardmode;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Items.Placeable;

namespace TheDepths.ModSupport.CerebralMod.Items.Weapons.Esper
{
    [ExtendsFromMod("CerebralMod"), JITWhenModsEnabled("CerebralMod")]
    internal class Poisawblade : ECItem
    {
        public override string LocalizationCategory => "CerebralMod.Items";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 24;
            Item.width = 26;
            Item.height = 26;
            Item.knockBack = 4f;
            Item.value = Terraria.Item.sellPrice(0, 0, 54, 0);
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<Projectiles.Esper.Poisawblade>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<ArqueriteBar>(), 20)
            .AddTile(TileID.Anvils)
            .Register();
            
            Recipe NightSawblade = Recipe.Create(ModContent.ItemType<NightSawblade>())
            .AddIngredient(ModContent.ItemType<DemoniteSawblade>())
            .AddIngredient(ModContent.ItemType<JungleSawblade>())
            .AddIngredient(ModContent.ItemType<DungeonSawblade>())
            .AddIngredient(this)
            .AddIngredient(ModContent.ItemType<NightCrafter>())

            .AddTile(TileID.DemonAltar)
            .Register();

            NightSawblade = Recipe.Create(ModContent.ItemType<NightSawblade>())
            .AddIngredient(ModContent.ItemType<CrimtaneSawblade>())
            .AddIngredient(ModContent.ItemType<JungleSawblade>())
            .AddIngredient(ModContent.ItemType<DungeonSawblade>())
            .AddIngredient(this)
            .AddIngredient(ModContent.ItemType<NightCrafter>())

            .AddTile(TileID.DemonAltar)
            .Register();
        }
    }
}
