using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TheDepths.ModSupport.Fargos.Buffs
{
    [ExtendsFromMod("FargowiltasSouls"), JITWhenModsEnabled("FargowiltasSouls")]
    public class QuicksilverWetBuff : ModBuff
    {
        public override string LocalizationCategory => "FargosSouls.Buffs";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Lava Wet");
            // Description.SetDefault("You are dripping lava");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //player.DepthSouls().QuicksilverWet = true;
        }
    }
}
