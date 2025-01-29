using CerebralMod;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using TheDepths.Projectiles;

namespace TheDepths.ModSupport.CerebralMod.Projectiles.Defender
{
    [ExtendsFromMod("CerebralMod"), JITWhenModsEnabled("CerebralMod")]
    internal class WaterGeyserSentry : SentryProjectile
    {
        public override string LocalizationCategory => "CerebralMod.Projectiles";

        public override string Texture => "CerebralMod/Projectiles/Defender/PreHardmode/GeyserSentry";

        public override void SetStaticDefaults()
        {
            //ProjectileID.Sets.IsADD2Turret[Projectile.type] = true;
            ProjectileID.Sets.DontAttachHideToAlpha[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 32;
            Projectile.height = 18;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.tileCollide = true;
            Projectile.hide = true;
            Projectile.netImportant = true;
            Projectile.penetrate = -1;
            Projectile.sentry = true;
            Projectile.ignoreWater = true;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override void AI()
        {
            base.AI();
            float sentrySpeed = Main.player[Projectile.owner].GetModPlayer<CMPlayer>().sentrySpeed;

            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= 120f / sentrySpeed)
            {
                Projectile.ai[0] = 0f;
                if (Projectile.owner == Main.myPlayer)
                {
                    SoundEngine.PlaySound(SoundID.Item34, Projectile.position);
                    Vector2 vector = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), vector.X, vector.Y, 0, -8, ModContent.ProjectileType<WaterGeyserSentryProj>(), (int)(Projectile.damage), Projectile.knockBack, Main.player[Projectile.owner].whoAmI);
                }
            }
        }

        public override bool? CanHitNPC(NPC npc)
        {
            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCs.Add(index);
        }
    }

    [ExtendsFromMod("CerebralMod"), JITWhenModsEnabled("CerebralMod")]
    internal class WaterGeyserSentryProj : WaterGeyser
    {
        public override string LocalizationCategory => "CerebralMod.Projectiles";

        public override string Texture => "TheDepths/Projectiles/WaterGeyser";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.hostile = false;
            Projectile.trap = false;
        }
    }
}
