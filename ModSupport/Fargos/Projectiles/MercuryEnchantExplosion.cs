using FargowiltasSouls.Content.Buffs.Souls;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Content.Projectiles.Souls;
using FargowiltasSouls;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using FargowiltasSouls.Core.AccessoryEffectSystem;

namespace TheDepths.ModSupport.Fargos.Projectiles
{
    [ExtendsFromMod("FargowiltasSouls"), JITWhenModsEnabled("FargowiltasSouls")]
    internal class MercuryEnchantExplosion : ModProjectile
    {
        public override string LocalizationCategory => "FargosSouls.Projectiles";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Explosion");
            Main.projFrames[Type] = 6;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        private int fpf = 4;
        public override void SetDefaults()
        {
            Projectile.width = 300;
            Projectile.height = 300;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = Main.projFrames[Type] * fpf;
            Projectile.tileCollide = false;
            Projectile.light = 0.75f;
            Projectile.ignoreWater = true;
            //Projectile.extraUpdates = 1;
            AIType = ProjectileID.Bullet;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.FargoSouls().DeletionImmuneRank = 2;

            Projectile.scale = 1f;

        }
        public bool SourceIsTerra = false;
        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);

            if (source is EntitySource_Parent parent && parent.Entity is Projectile parentProj && parentProj.type == ModContent.ProjectileType<TerraLightning>())
            {
                SourceIsTerra = true;
            }
        }
        public override void AI()
        {
            if (++Projectile.frameCounter > fpf)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
            }

            if (Projectile.frame >= Main.projFrames[Type])
            {
                Projectile.Kill();
            }
            Projectile.velocity = Vector2.Zero;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (SourceIsTerra)
            {
                target.AddBuff(BuffID.Electrified, 60 * 5);
                if (Projectile.owner.IsWithinBounds(Main.maxProjectiles) && Main.player[Projectile.owner].HasEffect<LeadEffect>())
                    target.AddBuff(ModContent.BuffType<LeadPoisonBuff>(), 60 * 5);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            FargoSoulsUtil.GenericProjectileDraw(Projectile, lightColor);
            return false;
        }
    }
}
