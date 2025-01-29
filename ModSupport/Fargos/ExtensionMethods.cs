using FargowiltasSouls.Core.Globals;
using FargowiltasSouls.Core.ModPlayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TheDepths.ModSupport.Fargos
{
    [ExtendsFromMod("FargowiltasSouls"), JITWhenModsEnabled("FargowiltasSouls")]
    public static partial class ExtensionMethods
    {
        public static DepthsSoulsPlayer DepthSouls(this Player player)
            => player.GetModPlayer<DepthsSoulsPlayer>();

        public static DepthsSoulsGlobalNPC DepthsSouls(this NPC npc)
        {
            return npc.GetGlobalNPC<DepthsSoulsGlobalNPC>();
        }
    }
}
