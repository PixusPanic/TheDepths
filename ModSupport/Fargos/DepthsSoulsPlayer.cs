using FargowiltasSouls.Core.ModPlayers;
using Terraria.ModLoader;

namespace TheDepths.ModSupport.Fargos
{
    [ExtendsFromMod("FargowiltasSouls"), JITWhenModsEnabled("FargowiltasSouls")]
    public partial class DepthsSoulsPlayer : ModPlayer
    {
        public int NightwoodCD;

        public int QuartzCD;

        public bool QuicksilverWet = false;
    }
}
