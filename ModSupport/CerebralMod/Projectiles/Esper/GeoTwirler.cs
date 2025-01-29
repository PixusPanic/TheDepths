using CerebralMod;
using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using TheDepths.Buffs;
using TheDepths.Dusts;

namespace TheDepths.ModSupport.CerebralMod.Projectiles.Esper
{
    [ExtendsFromMod("CerebralMod"), JITWhenModsEnabled("CerebralMod")]
    internal class GeoTwirler : BaseTwirlerProj
    {
        public override string LocalizationCategory => "CerebralMod.Projectiles";

        private static float geoTorch = 1f;
        public override void SetDefaults()
        {
            base.SetDefaults();
            twirlerDust = ModContent.DustType<GeodeTorchDust>();
            dustR = 0.8f;
            dustG = 0.8f;
            dustB = 0.5f;
        }

        public override void PostAI()
        {
            if (Projectile.velocity.X != 0)
                dir = Math.Sign(Projectile.velocity.X);
            Projectile.rotation -= dir * 6;
            if (!Projectile.wet && !Projectile.lavaWet)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, twirlerDust, 0f, 0f, 100, default(Color), 1f);
                Lighting.AddLight((int)((Projectile.position.X + (float)(Projectile.width / 2)) / 16f), (int)((Projectile.position.Y + (float)(Projectile.height / 2)) / 16f), dustR * geoTorch + 1f * (1f - geoTorch), dustG, dustB * geoTorch + 0.5f * (1f - geoTorch));
            }
        }

        public override void OnHitNPCExtra(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int chance = Main.rand.Next(20);
            if (chance == 19) target.AddBuff(ModContent.BuffType<FreezingWater>(), 180, false);
            else if (chance >= 14) target.AddBuff(ModContent.BuffType<FreezingWater>(), 90, false);
            else if (chance >= 9) target.AddBuff(ModContent.BuffType<FreezingWater>(), 30, false);
        }
    }
}
