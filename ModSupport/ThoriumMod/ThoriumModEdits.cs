using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Worldgen;

namespace TheDepths.ModSupport.ThoriumMod
{
    [ExtendsFromMod("ThoriumMod"), JITWhenModsEnabled("ThoriumMod")]
    internal class ThoriumModNPCEdits : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            SpawnNPC_ThoriumNPC(spawnInfo, 0, out bool blockSpawn);
            if (blockSpawn)
            {
                if (pool.ContainsKey(0))
                {
                    pool[0] = 0f;
                }
            }
        }

        private static int SpawnNPC_TryFindingProperGroundTileType(int spawnTileType, int x, int y)
        {
            if (!NPC.IsValidSpawningGroundTile(x, y))
            {
                for (int i = y + 1; i < y + 30; i++)
                {
                    if (NPC.IsValidSpawningGroundTile(x, i))
                    {
                        return Main.tile[x, i].TileType;
                    }
                }
            }
            return spawnTileType;
        }

        public static float SpawnNPC_ThoriumNPC(NPCSpawnInfo spawnInfo, int npcType, out bool blockSpawn)
        {
            blockSpawn = false;
            int x = spawnInfo.SpawnTileX;
            int y = spawnInfo.SpawnTileY;
            Player player = spawnInfo.Player;
            int spawnTile = spawnInfo.SpawnTileType;
            int tileType = Main.tile[x, y].TileType; //num56
            tileType = SpawnNPC_TryFindingProperGroundTileType(tileType, x, y);
            int wall = Main.tile[x, y - 1].WallType; //num58
            int num89 = (int)(player.position.X + (float)(player.width / 2)) / 16;
            int num100 = (int)(player.position.Y + (float)(player.height / 2)) / 16;
            if (Main.tile[x, y - 2].WallType == 244 || Main.tile[x, y].WallType == 244)
            {
                wall = 244;
            }
            int maxValue = 65;
            if (Main.remixWorld && (double)(player.position.Y / 16f) < Main.worldSurface && (player.ZoneCorrupt || player.ZoneCrimson))
            {
                maxValue = 25;
            }
            //called surface yet gets above the ROCK layer not surface, this includes the dirt layer too
            bool surface = (double)y <= Main.rockLayer; //num9
                                                        //true surface and its additional conditions
            bool surface2 = (double)y <= Main.worldSurface; //num13
            bool dirtLayer = (double)y >= Main.rockLayer; //num14
            bool raining = Main.cloudAlpha > 0f; //num17
            bool oceanBottom = ((x < WorldGen.oceanDistance || x > Main.maxTilesX - WorldGen.oceanDistance) && Main.tileSand[tileType] && (double)y < Main.rockLayer) || (spawnTile == TileID.Sand && WorldGen.oceanDepths(x, y)); //num15
            bool beach = (double)y <= Main.worldSurface && (x < WorldGen.beachDistance || x > Main.maxTilesX - WorldGen.beachDistance); //num16
            //int range = 10;

            // spawning
            if (ModLoader.TryGetMod("ThoriumMod", out Mod Thorium))
            {
                if (Main.hardMode && (spawnInfo.Player.ZoneUnderworldHeight && Worldgen.TheDepthsWorldGen.InDepths(spawnInfo.Player) && !Main.remixWorld) || Main.hardMode && (spawnInfo.Player.ZoneUnderworldHeight && !Worldgen.TheDepthsWorldGen.InDepths(spawnInfo.Player) && (spawnInfo.SpawnTileX < Main.maxTilesX * 0.38 + 50.0 || spawnInfo.SpawnTileX > Main.maxTilesX * 0.62) && Main.remixWorld))
                {
                    if (Thorium.TryFind("BoneFlayer", out ModNPC BoneFlayer) && npcType == BoneFlayer.Type)
                    {
                        blockSpawn = true;
                    }
                    else if (Thorium.TryFind("UnderworldPot1", out ModNPC UnderworldPot1) && npcType == UnderworldPot1.Type)
                    {
                        blockSpawn = true;
                    }
                    else if (Thorium.TryFind("UnderworldPot2", out ModNPC UnderworldPot2) && npcType == UnderworldPot2.Type)
                    {
                        blockSpawn = true;
                    }
                    else if (Thorium.TryFind("InfernalHound", out ModNPC InfernalHound) && npcType == InfernalHound.Type)
                    {
                        blockSpawn = true;
                    }
                    else if (Thorium.TryFind("MoltenMortar", out ModNPC MoltenMortar) && npcType == MoltenMortar.Type)
                    {
                        blockSpawn = true;
                    }
                    else if (Thorium.TryFind("HellBringerMimic", out ModNPC HellBringerMimic) && npcType == HellBringerMimic.Type)
                    {
                        blockSpawn = true;
                    }
                }
            }
            
            return 0f;
        }

        public override void ModifyGlobalLoot(GlobalLoot globalLoot)
        {
            globalLoot.Add(ItemDropRule.ByCondition(new SoulofFluorite(), ModContent.ItemType<Items.SoulofFluorite>(), 5));
            //globalLoot.Remove(ItemDropRule.ByCondition(new SoulofPlightCondition(), ModContent.ItemType<SoulofPlight>(), 5));
            //globalLoot.Add(ItemDropRule.ByCondition(new SoulofPlightUnderworld(), ModContent.ItemType<SoulofPlight>(), 5));
        }
    }

    [ExtendsFromMod("ThoriumMod"), JITWhenModsEnabled("ThoriumMod")]
    internal class SoulofFluorite : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        bool IItemDropRuleCondition.CanDrop(DropAttemptInfo info)
        {
            if (info.IsInSimulation)
            {
                return false;
            }
            if (info.player.position.Y / 16f > Main.UnderworldLayer && Main.hardMode && TheDepthsWorldGen.isWorldDepths && !TheDepthsWorldGen.DrunkDepthsLeft && !TheDepthsWorldGen.DrunkDepthsRight)
            {
                return Conditions.SoulOfWhateverConditionCanDrop(info);
            }
            return false;
        }

        bool IItemDropRuleCondition.CanShowItemDropInUI()
        {
            return true;
        }

        string IProvideItemConditionDescription.GetConditionDescription()
        {
            return null;
        } 
    }

    /*internal class SoulofPlightUnderworld : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        bool IItemDropRuleCondition.CanDrop(DropAttemptInfo info)
        {
            if (info.IsInSimulation)
            {
                return false;
            }
            if (info.player.position.Y / 16f > Main.UnderworldLayer && Main.hardMode && !TheDepthsWorldGen.isWorldDepths && !TheDepthsWorldGen.DrunkDepthsLeft && !TheDepthsWorldGen.DrunkDepthsRight)
            {
                return Conditions.SoulOfWhateverConditionCanDrop(info);
            }
            return false;
        }

        bool IItemDropRuleCondition.CanShowItemDropInUI()
        {
            return true;
        }

        string IProvideItemConditionDescription.GetConditionDescription()
        {
            return Language.GetTextValue("Bestiary_ItemDropConditions.LivingFlames");
        }
    }*/

    [ExtendsFromMod("ThoriumMod"), JITWhenModsEnabled("ThoriumMod")]
    internal class ThoriumModILEdits : ModSystem
    {
        private static Mod Thorium => ModLoader.GetMod("ThoriumMod");
        private static Assembly ThoriumAssembly = Thorium.GetType().Assembly;
        private static Type plight = null;
        private static MethodInfo changePlightCond = null;
        private static ILHook plightHook = null;

        public override void OnModLoad()
        {
            bool loadCaught = false;
            while (!loadCaught)
            {
                if (Thorium != null)
                {
                    foreach (Type type in ThoriumAssembly.GetTypes())
                    {
                        if (type.Name == "SoulofPlightCondition")
                        {
                            plight = type;
                        }   
                    }

                    if (plight != null)
                    {
                        changePlightCond = plight.GetMethod("CanDrop", BindingFlags.Public | BindingFlags.Instance);
                        plightHook = new ILHook(changePlightCond, NewSoulofPlightCondition);
                        plightHook.Apply();
                    } 
                    loadCaught = true;
                    break;
                }
            }
        }

        public override void OnModUnload()
        {
            if (Thorium != null)
            {
                plightHook.Dispose();
            }
        }

        private void NewSoulofPlightCondition(ILContext il)
        {
            try {
                ILCursor c = new(il);
                ILLabel LabelLocation = c.DefineLabel();
                //ModContent.GetInstance<TheDepths>().Logger.Debug("c is equal to: " + c + "LabelLocation is equal to: " + LabelLocation);

                c.GotoNext(MoveType.After,
                    i => i.MatchLdarg1(),
                    i => i.MatchLdfld("Terraria.GameContent.ItemDropRules.DropAttemptInfo", "player"),
                    i => i.MatchLdflda("Terraria.Entity", "position"),
                    i => i.MatchLdfld("Microsoft.Xna.Framework.Vector2", "Y"),
                    i => i.MatchLdcR4(16),
                    i => i.MatchDiv(),
                    i => i.MatchCall("Terraria.Main", "get_UnderworldLayer"),
                    i => i.MatchConvR4(),
                    i => i.MatchBleUn(out _));
            
                c.MarkLabel(LabelLocation);
                if (LabelLocation == null) {
                    ModContent.GetInstance<TheDepths>().Logger.Debug("Soul of Plight ILLabel could not be found");
                    return;
                }

                c.Index = 0;

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

                /* Code should look like this:
                 * public bool CanDrop(DropAttemptInfo info)
		    {
			    return !info.IsInSimulation && info.player.position.Y / 16f > (float)Main.UnderworldLayer && !TheDepthsWorldGen.isWorldDepths && Conditions.SoulOfWhateverConditionCanDrop(info);
		    }*/
            }
            catch (Exception)
            {
            }
        }
    }
}
