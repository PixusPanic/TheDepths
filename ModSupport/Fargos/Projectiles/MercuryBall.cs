using Terraria.ModLoader;
using TheDepths.Projectiles;

namespace TheDepths.ModSupport.Fargos.Projectiles
{
    [ExtendsFromMod("FargowiltasSouls"), JITWhenModsEnabled("FargowiltasSouls")]
    internal class MercuryBall : ShadowBall
    {
        public override string LocalizationCategory => "FargosSouls.Projectiles";

    }
}
