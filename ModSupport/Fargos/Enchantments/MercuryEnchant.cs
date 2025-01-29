using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Items.Armor;
using TheDepths.Items.Placeable;
using Microsoft.Xna.Framework;
using FargowiltasSouls.Content.Items.Accessories.Forces;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler.Content;
using FargowiltasSouls;
using Terraria;
using FargowiltasSouls.Core.Toggler;
using TheDepths.Buffs;
using TheDepths.ModSupport.Fargos.Projectiles;
using TheDepths.ModSupport.Fargos.Buffs;

namespace TheDepths.ModSupport.Fargos.Enchantments
{
    [ExtendsFromMod("FargowiltasSouls"), JITWhenModsEnabled("FargowiltasSouls")]
    internal class MercuryEnchant : BaseEnchant
    {
        public override string LocalizationCategory => "FargosSouls.Items";

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Orange;
            Item.value = 50000;
        }

        public override Color nameColor => new(192, 192, 192);

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<MercuryHelmet>())
            .AddIngredient(ModContent.ItemType<MercuryChestplate>())
            .AddIngredient(ModContent.ItemType<MercuryGreaves>())
            .AddIngredient(ModContent.ItemType<Items.Weapons.Terminex>())
            .AddIngredient(ModContent.ItemType<FlowingQuicksilver>())

            .AddTile(TileID.DemonAltar)
            .Register();
        }

        public class MercuryEffect : AccessoryEffect
        {
            public override Header ToggleHeader => Header.GetHeader<NatureHeader>();
            public override int ToggleItemType => ModContent.ItemType<MercuryEnchant>();
            public static float AuraSize(Player player)
            {
                if (player.HasEffect<NatureEffect>())
                {
                    return ShadewoodEffect.Range(player, true);
                }
                if (player.FargoSouls().ForceEffect<MercuryEnchant>())
                    return 150 * 1.1f;
                return 150;

            }
            public override void PostUpdateEquips(Player player)
            {
                if (player.whoAmI == Main.myPlayer)
                {
                    bool nature = player.HasEffect<NatureEffect>();

                    //player.inferno = true;
                    int visualProj = ModContent.ProjectileType<MercuryAuraProj>();
                    if (player.ownedProjectileCounts[visualProj] <= 0)
                    {
                        Projectile.NewProjectile(GetSource_EffectItem(player), player.Center, Vector2.Zero, visualProj, 0, 0, Main.myPlayer);
                    }
                    if (!nature)
                        Lighting.AddLight((int)(player.Center.X / 16f), (int)(player.Center.Y / 16f), 0.65f, 0.4f, 0.1f);

                    int buff = ModContent.BuffType<MercuryPoisoning>();
                    float distance = AuraSize(player);
                    int baseDamage = player.FargoSouls().ForceEffect<MercuryEnchant>() ? 40 : 20;

                    int damage = FargoSoulsUtil.HighestDamageTypeScaling(player, baseDamage);

                    if (player.whoAmI == Main.myPlayer)
                    {
                        bool healed = false;

                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            NPC npc = Main.npc[i];
                            if (npc.active && !npc.friendly && !npc.dontTakeDamage && !(npc.damage == 0 && npc.lifeMax == 5)) //critters
                            {
                                if (Vector2.Distance(player.Center, FargoSoulsUtil.ClosestPointInHitbox(npc.Hitbox, player.Center)) <= distance)
                                {
                                    int dmgRate = 30;//60;

                                    if (!nature)
                                    {
                                        if (player.FindBuffIndex(ModContent.BuffType<MercuryContagion>()) == -1)
                                            player.AddBuff(ModContent.BuffType<MercuryContagion>(), 10);

                                        if (npc.FindBuffIndex(buff) == -1)
                                            npc.AddBuff(buff, 120);

                                        /*if (player.infernoCounter % dmgRate == 0)
                                            player.ApplyDamageToNPC(npc, damage, 0f, 0, false);*/
                                    }
                                    else
                                    {
                                        baseDamage = 50;
                                        int time = player.FargoSouls().TimeSinceHurt;
                                        float minTime = 60 * 4;
                                        if (time > minTime)
                                        {
                                            float maxBonus = 4; // at 16s
                                            float bonus = MathHelper.Clamp(time / (60 * 4), 1, maxBonus);
                                            baseDamage = (int)(baseDamage * bonus);
                                            damage = FargoSoulsUtil.HighestDamageTypeScaling(player, baseDamage);

                                            /*if (player.infernoCounter % dmgRate == 0)
                                            {
                                                player.ApplyDamageToNPC(npc, damage, 0f, 0, false);
                                                if (player.HasEffect<CrimsonEffect>() && !healed)
                                                {
                                                    healed = true;
                                                    player.FargoSouls().HealPlayer(damage / 80);
                                                }
                                            }*/
                                        }
                                    }

                                    int MercuryDebuff = ModContent.BuffType<MercuryAmplifyBuff>();
                                    if (npc.FindBuffIndex(MercuryDebuff) == -1)
                                        npc.AddBuff(MercuryDebuff, 10);


                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
