using FargowiltasSouls;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Content.Items.Accessories.Forces;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.ModPlayers;
using FargowiltasSouls.Core.Toggler;
using FargowiltasSouls.Core.Toggler.Content;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Buffs;
using TheDepths.Items;
using TheDepths.Items.Armor;
using TheDepths.Items.Placeable;
using TheDepths.ModSupport.Fargos.Projectiles;
using TheDepths.Projectiles;

namespace TheDepths.ModSupport.Fargos.Enchantments
{
    [ExtendsFromMod("FargowiltasSouls"), JITWhenModsEnabled("FargowiltasSouls")]
    internal class NightwoodEnchant : BaseEnchant
    {
        public override string LocalizationCategory => "FargosSouls.Items";

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Blue;
            Item.value = 10000;
        }

        public override Color nameColor => new(64, 64, 64);

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<NightwoodHelmet>())
            .AddIngredient(ModContent.ItemType<NightwoodBreastplate>())
            .AddIngredient(ModContent.ItemType<NightwoodGreaves>())
            .AddIngredient(ModContent.ItemType<ShadowShrub>())
            .AddIngredient(ModContent.ItemType<Ciamito>())
            .AddIngredient(ModContent.ItemType<Geode>(), 5)

            .AddTile(TileID.DemonAltar)
            .Register();
        }

        public static void PassiveEffect(Player player)
        {
            //player.DepthSouls().fireNoDamage = true;
        }
        public override void UpdateInventory(Player player) => PassiveEffect(player);
        public override void UpdateVanity(Player player) => PassiveEffect(player);
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PassiveEffect(player);
            player.AddEffect<NightwoodEffect>(Item);
            player.AddEffect<NightwoodMercuryballs>(Item);
        }

        public class NightwoodEffect : AccessoryEffect
        {
            public override Header ToggleHeader => null;

            public override void PostUpdateEquips(Player player)
            {
                NightwoodEnchant.PassiveEffect(player);
                player.buffImmune[ModContent.BuffType<MercuryBoiling>()] = true;
                player.GetModPlayer<TheDepthsPlayer>().NightwoodBuff = true;
            }
        }
        public class NightwoodMercuryballs : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<TerraHeader>();
            public override int ToggleItemType => ModContent.ItemType<NightwoodEnchant>();
            public override bool ExtraAttackEffect => true;
            public override void PostUpdateEquips(Player player)
            {
                DepthsSoulsPlayer modPlayer = player.DepthSouls();
                if (modPlayer.NightwoodCD > 0)
                    modPlayer.NightwoodCD--;
            }
            public override void TryAdditionalAttacks(Player player, int damage, DamageClass damageType)
            {
                if (player.HasEffect<TerraLightningEffect>())
                    return;
                FargoSoulsPlayer modFargosPlayer = player.FargoSouls();
                DepthsSoulsPlayer modPlayer = player.DepthSouls();
                bool debuffed = false;
                for (int i = 0; i < Player.MaxBuffs; i++)
                {
                    int type = player.buffType[i];
                    if (type > 0 && type is not BuffID.PotionSickness or BuffID.ManaSickness or BuffID.WaterCandle && Main.debuff[type])
                        debuffed = true;
                }
                if (modPlayer.NightwoodCD <= 0 && (debuffed || player.HasEffect<QuartzProcEffect>()))
                {
                    modPlayer.NightwoodCD = modFargosPlayer.ForceEffect<NightwoodEnchant>() ? 20 : player.HasEffect<QuartzProcEffect>() ? 25 : 35;

                    int cap = 60;
                    int effectItemType = EffectItem(player).type;
                    int nightwood = ModContent.ItemType<NightwoodEnchant>();
                    int quicksilver = ModContent.ItemType<QuartzEnchant>();
                    if (!player.ForceEffect<NightwoodMercuryballs>() && (effectItemType == nightwood || effectItemType == quicksilver))
                        cap = 30;

                    int mercuryballDamage = damage;
                    Vector2 vel = Vector2.Normalize(Main.MouseWorld - player.Center) * 17f;
                    vel = vel.RotatedByRandom(Math.PI / 10);
                    if (!modFargosPlayer.TerrariaSoul)
                        mercuryballDamage = Math.Min(mercuryballDamage, FargoSoulsUtil.HighestDamageTypeScaling(player, cap));

                    if (player.whoAmI == Main.myPlayer)
                        Projectile.NewProjectile(GetSource_EffectItem(player), player.Center, vel, ModContent.ProjectileType<MercuryBall>(), mercuryballDamage, 1, Main.myPlayer);
                }
            }
        }
    }
}
