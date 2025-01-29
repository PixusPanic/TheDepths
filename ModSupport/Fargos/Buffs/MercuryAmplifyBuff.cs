using FargowiltasSouls;
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
    public class MercuryAmplifyBuff : ModBuff
    {
        public override string LocalizationCategory => "FargosSouls.Buffs";
        public override string Texture => "FargowiltasSouls/Content/Buffs/PlaceholderDebuff";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Molten Amplify");
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.DepthsSouls().MercuryAmplify = true;
        }
    }
}
