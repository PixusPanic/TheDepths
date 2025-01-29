using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Items.Armor;
using TheDepths.Items.Accessories;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls;
using static TheDepths.ModSupport.Fargos.Enchantments.NightwoodEnchant;
using FargowiltasSouls.Content.Items.Accessories.Forces;
using FargowiltasSouls.Core.Toggler.Content;
using FargowiltasSouls.Core.Toggler;
using TheDepths.Buffs;
using TheDepths.ModSupport.Fargos.Buffs;
using TheDepths.ModSupport.Fargos.Projectiles;
using FargowiltasSouls.Core.ModPlayers;

namespace TheDepths.ModSupport.Fargos.Enchantments
{
    [ExtendsFromMod("FargowiltasSouls"), JITWhenModsEnabled("FargowiltasSouls")]
    internal class QuartzEnchant : BaseEnchant
    {
        public override string LocalizationCategory => "FargosSouls.Items";

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Green;
            Item.value = 50000;
        }

        public override Color nameColor => new(128, 128, 128);

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<QuartzHood>())
            .AddIngredient(ModContent.ItemType<QuartzWinterCoat>())
            .AddIngredient(ModContent.ItemType<QuartzLeggings>())
            .AddIngredient(ModContent.ItemType<AzuriteRose>())
            .AddIngredient(ModContent.ItemType<NightwoodEnchant>())

            .AddTile(TileID.DemonAltar)
            .Register();
        }

        public override void UpdateInventory(Player player)
        {
            NightwoodEnchant.PassiveEffect(player);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            AddEffects(player, Item);
        }

        public static void AddEffects(Player player, Item item)
        {
            bool IsSoaked = false;

            FargoSoulsPlayer modFargosPlayer = player.FargoSouls();
            DepthsSoulsPlayer modPlayer = player.DepthSouls();
            player.AddEffect<NightwoodEffect>(item);
            player.AddEffect<NightwoodMercuryballs>(item);
            player.AddEffect<QuartzEffect>(item);

            player.GetModPlayer<TheDepthsPlayer>().cSkin = true;
            player.buffImmune[ModContent.BuffType<MercuryPoisoning>()] = true;

            if (player.lavaWet && Worldgen.TheDepthsWorldGen.InDepths(player) || Collision.LavaCollision(player.position, player.width, player.height) && Worldgen.TheDepthsWorldGen.InDepths(player) || player.GetModPlayer<TheDepthsPlayer>().quicksilverWet) IsSoaked = true;
            else if (!player.lavaWet && Worldgen.TheDepthsWorldGen.InDepths(player) || !Collision.LavaCollision(player.position, player.width, player.height) && Worldgen.TheDepthsWorldGen.InDepths(player) || !player.GetModPlayer<TheDepthsPlayer>().quicksilverWet) IsSoaked = false;

            // in quicksilver effects
            if (IsSoaked)
            {
                player.gravity = Player.defaultGravity;
                player.ignoreWater = true;
                player.accFlipper = true;

                player.AddBuff(ModContent.BuffType<QuicksilverWetBuff>(), 600);
            }

            if (modPlayer.QuartzCD > 0)
                modPlayer.QuartzCD--;

            bool triggerFromDebuffs = false;
            if (modFargosPlayer.ForceEffect<QuartzEnchant>())
            {
                for (int i = 0; i < Player.MaxBuffs; i++)
                {
                    int type = player.buffType[i];
                    if (type > 0 && type is not BuffID.PotionSickness or BuffID.ManaSickness or BuffID.WaterCandle && Main.debuff[type])
                        triggerFromDebuffs = true;
                }
            }
            if (triggerFromDebuffs || IsSoaked)
            {
                player.AddEffect<QuartzProcEffect>(item);
            }
        }
    }
    
    [ExtendsFromMod("FargowiltasSouls"), JITWhenModsEnabled("FargowiltasSouls")]
    public class QuartzEffect : AccessoryEffect
    {
        public override Header ToggleHeader => null;
    }

    [ExtendsFromMod("FargowiltasSouls"), JITWhenModsEnabled("FargowiltasSouls")]
    public class QuartzProcEffect : AccessoryEffect
    {
        public override Header ToggleHeader => Header.GetHeader<TerraHeader>();
        public override int ToggleItemType => ModContent.ItemType<QuartzEnchant>();
        public override bool ExtraAttackEffect => true;
        public override void OnHitNPCEither(Player player, NPC target, NPC.HitInfo hitInfo, DamageClass damageClass, int baseDamage, Projectile projectile, Item item)
        {
            if (player.HasEffect<TerraLightningEffect>())
                return;
            if (player.DepthSouls().QuartzCD == 0)
            {
                int damage = baseDamage;
                DepthsSoulsPlayer modPlayer = player.DepthSouls();
                int cap = player.ForceEffect<QuartzProcEffect>() ? 200 : 50;
                damage = Math.Min(damage, FargoSoulsUtil.HighestDamageTypeScaling(player, cap));

                if (player.GetModPlayer<TheDepthsPlayer>().quicksilverWet)
                    damage = (int)(damage * 1.3f);
                if (!player.ForceEffect<QuartzProcEffect>())
                    damage = (int)(damage * 0.875f);

                if (damage > 250)
                    damage = 250;

                Projectile.NewProjectile(GetSource_EffectItem(player), target.Center, Vector2.Zero, ModContent.ProjectileType<MercuryEnchantExplosion>(), damage, 0, player.whoAmI);

                modPlayer.QuartzCD = 50;
            }
        }
    }
}
