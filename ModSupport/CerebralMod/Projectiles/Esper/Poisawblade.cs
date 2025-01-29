using CerebralMod;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using TheDepths.Buffs;
using TheDepths.Dusts;

namespace TheDepths.ModSupport.CerebralMod.Projectiles.Esper
{
    [ExtendsFromMod("CerebralMod"), JITWhenModsEnabled("CerebralMod")]
    internal class Poisawblade : BaseSawbladeProj
    {
        public override string LocalizationCategory => "CerebralMod.Projectiles";

        public override void PostAI()
        {
            if (Main.rand.NextBool(3))
            {
                Lighting.AddLight((int)((Projectile.position.X + (float)(Projectile.width / 2)) / 16f), (int)((Projectile.position.Y + (float)(Projectile.height / 2)) / 16f), Projectile.light, Projectile.light *= 0.8f, Projectile.light *= 0.6f);
                int num130 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 2f), Projectile.width, Projectile.height, ModContent.DustType<QuicksilverBubble>(), 0f, 0f, 100, default(Color), 1.8f);
                Dust dust3 = Main.dust[num130];
                dust3.velocity += Projectile.velocity * 0.2f;
                Main.dust[num130].noGravity = true;
            }
        }

        public override void OnHitNPCExtra(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(2))
                target.AddBuff(ModContent.BuffType<MercuryPoisoning>(), 120, false);
        }
    }
}
