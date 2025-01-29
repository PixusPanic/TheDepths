using FargowiltasSouls.Core.ModPlayers;
using FargowiltasSouls;
using Terraria;
using Terraria.ModLoader;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Content.Items.Accessories.Forces;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using TheDepths.ModSupport.Fargos.Enchantments;

namespace TheDepths.ModSupport.Fargos
{
    [ExtendsFromMod("FargowiltasSouls"), JITWhenModsEnabled("FargowiltasSouls")]
    public class DepthsSoulsGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public bool MercuryAmplify;

        public override void ResetEffects(NPC npc)
        {
            MercuryAmplify = false;
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[Main.myPlayer];
            FargoSoulsPlayer fargoSoulsPlayer = player.FargoSouls();

            if (MercuryAmplify)
            {
                float num2 = 1.2f;
                if (player.HasEffect<NatureEffect>())
                {
                    num2 = 1.15f;
                }
                else if (fargoSoulsPlayer.ForceEffect<MercuryEnchant>())
                {
                    num2 = 1.3f;
                }

                modifiers.FinalDamage *= num2;
            }
        }
    }
}
