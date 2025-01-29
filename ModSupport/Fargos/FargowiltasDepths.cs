using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using System;
using System.Reflection;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using TheDepths.Worldgen;
using TheDepths.Items.Placeable;
using TheDepths.Items;
using Terraria.Localization;

namespace TheDepths.ModSupport.Fargos
{
    [ExtendsFromMod("Fargowiltas"), JITWhenModsEnabled("Fargowiltas")]
    internal class FargowiltasILEdits : ModSystem
    {
        private static Mod Fargos => ModLoader.GetMod("Fargowiltas");
        private static Assembly FargosAssembly = Fargos.GetType().Assembly;
        private static Type lumberjack = null;
        private static MethodInfo addDepths = null;
        private static ILHook lumberHook = null;

        Player player = Main.LocalPlayer;
        int itemType;

        string quote = "";
        string LumberChat(string key, params object[] args) => Language.GetTextValue($"Mods.TheDepths.Fargowiltas.NPCs.LumberJack.Chat.{key}", args);

        public override void OnModLoad()
        {
            bool loadCaught = false;
            while (!loadCaught)
            {
                if (Fargos != null)
                {
                    foreach (Type type in FargosAssembly.GetTypes())
                    {
                        if (type.Name == "LumberJack")
                        {
                            lumberjack = type;
                        }
                    }

                    if (lumberjack != null)
                    {
                        addDepths = lumberjack.GetMethod("OnChatButtonClicked", BindingFlags.Public | BindingFlags.Instance);
                        //lumberHook = new ILHook(addDepths, DepthsLumberjack);
                        //lumberHook.Apply();
                    }
                    loadCaught = true;
                    break;
                }
            }
        }

        public override void OnModUnload()
        {
            if (Fargos != null)
            {
                //lumberHook.Dispose();
            }
        }

        private void DepthsLumberjack(ILContext il)
        {
            try
            {
                ILCursor c = new(il);
                ILLabel LabelLocation = c.DefineLabel();
                //ModContent.GetInstance<TheDepths>().Logger.Debug("c is equal to: " + c + "LabelLocation is equal to: " + LabelLocation);

                c.GotoNext(MoveType.After,
                    i => i.MatchLdarg0(),
                    i => i.MatchLdfld("Fargowiltas.NPCs.LumberJack", "dayOver"),
                    i => i.MatchBrfalse(out _),
                    i => i.MatchLdarg0(),
                    i => i.MatchLdfld("Fargowiltas.NPCs.LumberJack", "nightOver"),
                    i => i.MatchBrfalse(out _));

                c.GotoNext(MoveType.Before,
                    i => i.MatchLdloc0(),
                    i => i.MatchCall("Terraria.Player", "get_ZoneRockLayerHeight"),
                    i => i.MatchBrtrue(out _),
                    i => i.MatchLdloc0(),
                    i => i.MatchCall("Terraria.Player", "get_ZoneDirtLayerHeight"),
                    i => i.MatchBrfalse(out _));

                c.MarkLabel(LabelLocation);
                if (LabelLocation == null)
                {
                    ModContent.GetInstance<TheDepths>().Logger.Debug("Lumberjack biome ILLabel could not be found");
                    return;
                }

                c.Index = 0;

                c.GotoNext(MoveType.After,
                    i => i.MatchLdloc0(),
                    i => i.MatchCall("Terraria.Player", "get_ZoneUnderworldHeight"),
                    i => i.MatchBrfalse(out _));

                //c.EmitLdloc(0); //flag
                // Push an if statement, saying if the world core is Hell
                c.EmitDelegate(() =>
                {
                    /*if (flag == TheDepthsWorldGen.isWorldDepths)
                    {
                        flag = false;
                    }
                    return flag;*/
                    return TheDepthsWorldGen.isWorldDepths;
                });
                // If this is true, go to LabelLocation
                c.EmitBrtrue(LabelLocation);

                c.GotoNext(MoveType.Before,
                    i => i.MatchLdloc0(),
                    i => i.MatchCall("Terraria.Player", "get_ZoneRockLayerHeight"),
                    i => i.MatchBrtrue(out _),
                    i => i.MatchLdloc0(),
                    i => i.MatchCall("Terraria.Player", "get_ZoneDirtLayerHeight"),
                    i => i.MatchBrfalse(out _));

                c.EmitDelegate(() =>
                {
                    if (player.ZoneUnderworldHeight && TheDepthsWorldGen.isWorldDepths)
                    {
                        quote = LumberChat("Depths");
                        for (int i = 0; i < 5; i++)
                        {
                            if (Main.rand.NextBool(3)) player.QuickSpawnItem(player.GetSource_OpenItem(ModContent.ItemType<PetrifiedWood>()), ModContent.ItemType<PetrifiedWood>(), 50);
                            else player.QuickSpawnItem(player.GetSource_OpenItem(ModContent.ItemType<NightWood>()), ModContent.ItemType<NightWood>(), 50);
                            itemType = Main.rand.Next([ModContent.ItemType<AlbinoRat>(), ModContent.ItemType<QuartzCrawler>(), ModContent.ItemType<EnchantedNightmareWorm>()]);
                            player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType);
                            itemType = Main.rand.Next([ModContent.ItemType<BlackOlive>(), ModContent.ItemType<Ciamito>()]);
                            player.QuickSpawnItem(player.GetSource_OpenItem(itemType), itemType);
                        }
                    }
                    return;
                });
            }
            catch (Exception)
            {
            }
        }
    }
}
