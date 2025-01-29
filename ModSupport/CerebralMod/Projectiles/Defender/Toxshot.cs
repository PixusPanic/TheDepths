using CerebralMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Buffs;
using TheDepths.Dusts;

namespace TheDepths.ModSupport.CerebralMod.Projectiles.Defender
{
    [ExtendsFromMod("CerebralMod"), JITWhenModsEnabled("CerebralMod")]
    internal class Toxshot : BaseTurret
    {
        public override string LocalizationCategory => "CerebralMod.Projectiles";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 36;
            Projectile.height = 36;
            fireRate = 60;
            projVel = 16f;
            turretBase = $"{nameof(TheDepths)}/ModSupport/CerebralMod/Projectiles/Defender/ToxshotBase";
        }

        public override void Fire(EntitySource_ItemUse_WithAmmo source, Vector2 vector2, int target)
        {
            Vector2 velocity = vector2 * projVel;
            Vector2 center = Projectile.Center;
            SoundEngine.PlaySound(SoundID.Item8, Projectile.position);
            Terraria.Projectile.NewProjectile(source, center, velocity, ModContent.ProjectileType<ToxshotProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
        }
    }

    [ExtendsFromMod("CerebralMod"), JITWhenModsEnabled("CerebralMod")]
    internal class ToxshotProj : ModProjectile
    {
        public override string LocalizationCategory => "CerebralMod.Projectiles";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.SentryShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.alpha = 100;
            Projectile.light = 0.2f;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.tileCollide = true;
            Projectile.scale = 0.9f;
            Projectile.localNPCHitCooldown = -1;
            Projectile.usesLocalNPCImmunity = true;
            //projectile.extraUpdates = 1;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(2))
                target.AddBuff(ModContent.BuffType<MercuryPoisoning>(), 180, false);
        }

        public override void AI()
        {
            if (Projectile.ai[1] == 0f)
                Projectile.ai[1] = 1f;
            Projectile.rotation += (float)Projectile.direction * 0.8f;
            int num3;
            for (int num258 = 0; num258 < 2; num258 = num3 + 1)
            {
                int num259 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<QuicksilverBubble>(), 0f, 0f, 100, default(Color), 1f);
                Main.dust[num259].noGravity = true;
                num3 = num258;
            }
            return;
        }
    }
}
