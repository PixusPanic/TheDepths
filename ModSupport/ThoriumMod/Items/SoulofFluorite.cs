using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.ModSupport.ThoriumMod.Items
{
    [ExtendsFromMod("ThoriumMod"), JITWhenModsEnabled("ThoriumMod")]
    internal class SoulofFluorite : ModItem
    {
        public override string LocalizationCategory => "ThoriumMod.Items";

        public override void SetStaticDefaults()
        {
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 4));
            ItemID.Sets.ItemIconPulse[Item.type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = Item.CommonMaxStack;
            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.rare = ItemRarityID.Orange;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 0) * 1f;
        }

        public override void PostUpdate()
        {
            float lightLevel = Main.rand.Next(90, 111) * 0.01f;
            lightLevel *= Main.essScale;
            Lighting.AddLight(Item.Center, new Vector3(0.3f * lightLevel, 0.2f * lightLevel, 0.08f * lightLevel));
        }
    }
}
