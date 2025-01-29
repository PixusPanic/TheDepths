using CerebralMod.Projectiles.Defender.PreHardmode;
using Terraria;
using Terraria.ModLoader;
using TheDepths.Buffs;

namespace TheDepths.ModSupport.CerebralMod.Projectiles.Defender
{
    [ExtendsFromMod("CerebralMod"), JITWhenModsEnabled("CerebralMod")]
    internal class MercurySpikes : CopperSpikes
    {
        public override string LocalizationCategory => "CerebralMod.Projectiles";

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(3))
                target.AddBuff(ModContent.BuffType<MercuryPoisoning>(), 60, false);
        }
    }
}
