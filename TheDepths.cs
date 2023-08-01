using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria.Graphics.Effects;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using TheDepths.Tiles;
using Terraria.ID;
using MonoMod.Utils;
using Terraria.GameContent.Liquid;
using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Light;
using Terraria.Utilities;
using Terraria.ObjectData;
using Terraria.Audio;
using TheDepths.Items.Weapons;
using Terraria.GameContent.Skies;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using TheDepths.Hooks;
using Terraria.DataStructures;
using TheDepths.Dusts;
using Terraria.Map;
using TheDepths.Items;
using Terraria.Graphics.Capture;
using Terraria.GameContent.UI.States;
using System.Linq;
using Terraria.IO;
using Terraria.ModLoader.IO;
using TheDepths.Biomes;
using Terraria.GameContent.Tile_Entities;
using static Terraria.WaterfallManager;
using Terraria.GameContent.Items;
using System.IO;
using Terraria.ModLoader.Config;

namespace TheDepths
{
    public class TheDepths : Mod
    {
        public static Asset<Texture2D>[] texture = new Asset<Texture2D>[14];
        public static Mod mod;
        public static List<int> livingFireBlockList;

        public override void Load()
        {
            mod = this;
            for (int i = 0; i < texture.Length; i++)
                texture[i] = ModContent.Request<Texture2D>("TheDepths/Backgrounds/DepthsUnderworldBG_" + i);
            livingFireBlockList = new List<int> { 336, 340, 341, 342, 343, 344, ModContent.TileType<Tiles.LivingFog>() };
            
            if (!Main.dedServ)
            {
                EquipLoader.AddEquipTexture(this, "TheDepths/Items/Armor/OnyxRobe_Legs", EquipType.Legs, name: "OnyxRobe_Legs");

                //GameShaders.Misc["TheDepths:ChasmeDeath"] = new MiscShaderData(new Ref<Effect>((Effect)ModContent.Request<Effect>("TheDepths/Shaders/ChasmeDeathEffect", ReLogic.Content.AssetRequestMode.ImmediateLoad)), "ChasmeAnimation").UseImage0("Images/Misc/Perlin");
            }

            //IL.Terraria.Liquid.Update += Evaporation;
            //IL_Player.ItemCheck_ManageRightClickFeatures += IL_Player_ItemCheck_ManageRightClickFeatures;
            Terraria.IL_Player.UpdateBiomes += NoHeap;

            //Terraria.IL_Main.DrawUnderworldBackgroudLayer += ILMainDrawUnderworldBackground;
            On_Main.DrawUnderworldBackgroudLayer += On_Main_DrawUnderworldBackgroudLayer;

            Terraria.On_Main.UpdateAudio_DecideOnTOWMusic += Main_UpdateAudio_DecideOnTOWMusic;

            Terraria.Graphics.Light.On_TileLightScanner.ApplyLiquidLight += On_TileLightScanner_ApplyLiquidLight;
            Terraria.Graphics.Light.On_TileLightScanner.ApplyHellLight += TileLightScanner_ApplyHellLight;
            On_WaterfallManager.AddLight += On_WaterfallManager_AddLight;

            
            Terraria.On_WaterfallManager.DrawWaterfall_int_int_int_float_Vector2_Rectangle_Color_SpriteEffects += On_WaterfallManager_DrawWaterfall_int_int_int_float_Vector2_Rectangle_Color_SpriteEffects;
            Terraria.On_WaterfallManager.StylizeColor += On_WaterfallManager_StylizeColor;
            On_Main.DoUpdate += On_Main_DoUpdate;

            On_Liquid.GetLiquidMergeTypes += On_Liquid_GetLiquidMergeTypes;
            On_Player.PlaceThing_Tiles_CheckLavaBlocking += On_Player_PlaceThing_Tiles_CheckLavaBlocking;
            
            Terraria.GameContent.UI.Elements.On_UIGenProgressBar.DrawSelf += On_UIGenProgressBar_DrawSelf;
            Terraria.GameContent.UI.States.IL_UIWorldCreation.BuildPage += DepthsSelectionMenu.ILBuildPage;
            Terraria.GameContent.UI.States.IL_UIWorldCreation.MakeInfoMenu += DepthsSelectionMenu.ILMakeInfoMenu;
            Terraria.GameContent.UI.States.IL_UIWorldCreation.ShowOptionDescription +=
                DepthsSelectionMenu.ILShowOptionDescription;
            Terraria.GameContent.UI.States.On_UIWorldSelect.UpdateWorldsList += On_UIWorldSelect_UpdateWorldsList;

            Terraria.GameContent.UI.States.On_UIWorldCreation.SetDefaultOptions += DepthsSelectionMenu.OnSetDefaultOptions;
            On_Player.ItemCheck_CatchCritters += On_Player_ItemCheck_CatchCritters;
            Terraria.On_Dust.NewDust += Dust_NewDust;
            On_TELogicSensor.GetState += On_TELogicSensor_GetState;
        }

        public override void Unload()
        {
            livingFireBlockList = null;
            //IL.Terraria.Liquid.Update -= Evaporation;
            //IL_Player.ItemCheck_ManageRightClickFeatures -= IL_Player_ItemCheck_ManageRightClickFeatures;
            Terraria.IL_Player.UpdateBiomes -= NoHeap;

            //Terraria.IL_Main.DrawUnderworldBackgroudLayer -= ILMainDrawUnderworldBackground;
            On_Main.DrawUnderworldBackgroudLayer -= On_Main_DrawUnderworldBackgroudLayer;

            Terraria.On_Main.UpdateAudio_DecideOnTOWMusic -= Main_UpdateAudio_DecideOnTOWMusic;

            Terraria.Graphics.Light.On_TileLightScanner.ApplyLiquidLight -= On_TileLightScanner_ApplyLiquidLight;
            Terraria.Graphics.Light.On_TileLightScanner.ApplyHellLight -= TileLightScanner_ApplyHellLight;
            On_WaterfallManager.AddLight -= On_WaterfallManager_AddLight;

            Terraria.On_WaterfallManager.DrawWaterfall_int_int_int_float_Vector2_Rectangle_Color_SpriteEffects -= On_WaterfallManager_DrawWaterfall_int_int_int_float_Vector2_Rectangle_Color_SpriteEffects;
            Terraria.On_WaterfallManager.StylizeColor -= On_WaterfallManager_StylizeColor;
            On_Main.DoUpdate -= On_Main_DoUpdate;
            
            On_Liquid.GetLiquidMergeTypes -= On_Liquid_GetLiquidMergeTypes;
            On_Player.PlaceThing_Tiles_CheckLavaBlocking -= On_Player_PlaceThing_Tiles_CheckLavaBlocking;
            
            Terraria.GameContent.UI.Elements.On_UIGenProgressBar.DrawSelf -= On_UIGenProgressBar_DrawSelf;
            Terraria.GameContent.UI.States.On_UIWorldSelect.UpdateWorldsList -= On_UIWorldSelect_UpdateWorldsList;

            On_Player.ItemCheck_CatchCritters -= On_Player_ItemCheck_CatchCritters;
            Terraria.On_Dust.NewDust -= Dust_NewDust;
            On_TELogicSensor.GetState -= On_TELogicSensor_GetState;
        }

        #region QuicksilverfallGlowmaskRemover
        private Color On_WaterfallManager_StylizeColor(On_WaterfallManager.orig_StylizeColor orig, float alpha, int maxSteps, int waterfallType, int y, int s, Tile tileCache, Color aColor)
        {
            float num = (float)(int)aColor.R * alpha;
            float num2 = (float)(int)aColor.G * alpha;
            float num3 = (float)(int)aColor.B * alpha;
            float num4 = (float)(int)aColor.A * alpha;
            if (Worldgen.TheDepthsWorldGen.InDepths)
            {
                if (waterfallType == 1)
                {
                    if (num < 0 * alpha)
                    {
                        num = 0 * alpha;
                    }
                    if (num2 < 0 * alpha)
                    {
                        num2 = 0 * alpha;
                    }
                    if (num3 < 0 * alpha)
                    {
                        num3 = 0 * alpha;
                    }
                }
            }
            else
            {
                orig.Invoke(alpha, maxSteps, waterfallType, y, s, tileCache, aColor);
                if (waterfallType == 1)
                {
                    if (num < 190f * alpha)
                    {
                        num = 190f * alpha;
                    }
                    if (num2 < 190f * alpha)
                    {
                        num2 = 190f * alpha;
                    }
                    if (num3 < 190f * alpha)
                    {
                        num3 = 190f * alpha;
                    }
                }
            }
            aColor = new Color((int)num, (int)num2, (int)num3, (int)num4);
            return aColor;
        }
        #endregion

        #region LavaSensorDetour
        private bool On_TELogicSensor_GetState(On_TELogicSensor.orig_GetState orig, int x, int y, TELogicSensor.LogicCheckType type, TELogicSensor instance)
        {
            if (type == TELogicSensor.LogicCheckType.Lava && (Worldgen.TheDepthsWorldGen.depthsorHell && !Main.drunkWorld || (Worldgen.TheDepthsWorldGen.DrunkDepthsLeft && Math.Abs(x) < Main.maxTilesX / 2 || Worldgen.TheDepthsWorldGen.DrunkDepthsRight && Math.Abs(x) > Main.maxTilesX / 2) && Main.drunkWorld))
            {
                return false;
            }
            return orig.Invoke(x, y, type, instance);
        }
        #endregion

        #region WorldUIOverlay
        private void On_UIWorldSelect_UpdateWorldsList(Terraria.GameContent.UI.States.On_UIWorldSelect.orig_UpdateWorldsList orig, Terraria.GameContent.UI.States.UIWorldSelect self)
        {
            orig.Invoke(self);
            UIList WorldList = (UIList)typeof(UIWorldSelect).GetField("_worldList", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self);
			
			TheDepthsClientConfig config = ModContent.GetInstance<TheDepthsClientConfig>();
            Dictionary<string, TheDepthsClientConfig.WorldDataValues> tempDict = config.GetWorldData();
            foreach (var item in WorldList)
            {
                if (item is UIWorldListItem)
                {
                    UIElement WorldIcon = (UIElement)typeof(UIWorldListItem).GetField("_worldIcon", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(item);
                    WorldFileData Data = (WorldFileData)typeof(AWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(item);

                    var path = Path.ChangeExtension(Data.Path, ".twld");

                    /*if (!tempDict.ContainsKey(path))
                    {
                        byte[] buf = FileUtilities.ReadAllBytes(path, Data.IsCloudSave);
                        var stream = new MemoryStream(buf);
                        var tag = TagIO.FromStream(stream);
                        bool containsMod = false;

                        if (tag.ContainsKey("modData"))
                        {
                            foreach (TagCompound modDataTag in tag.GetList<TagCompound>("modData").Skip(2))
                            {
                                if (modDataTag.Get<string>("mod") == ModContent.GetInstance<TheDepthsClientConfig>().Mod.Name)
                                {
                                    TagCompound dataTag = modDataTag.Get<TagCompound>("data");
                                    TheDepthsClientConfig.WorldDataValues worldData;

                                    worldData.depths = dataTag.Get<bool>("IsDepths");
                                    worldData.depthsLeft = dataTag.Get<bool>("DepthsIsOnTheLeft");
                                    worldData.depthsRight = dataTag.Get<bool>("DepthsIsOnTheRight");
                                    tempDict[path] = worldData;

                                    containsMod = true;

                                    break;
                                }
                            }

                            if (!containsMod)
                            {
                                TheDepthsClientConfig.WorldDataValues worldData;

                                worldData.depths = false;
                                worldData.depthsLeft = false;
                                worldData.depthsRight = false;
                                tempDict[path] = worldData;
                            }

                            config.SetWorldData(tempDict);
                            TheDepthsClientConfig.Save(config);
                        }
                    }*/

                    #region UnopenedWorldIcon
                    if (!tempDict.ContainsKey(path))
                    {
                        if (!Data.DrunkWorld && !Data.DefeatedMoonlord)
                        {
                            UIElement worldIcon = WorldIcon;
                            UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconUnderworld"))
                            {
                                Top = new StyleDimension(-10f, 0f),
                                Left = new StyleDimension(-6f, 0f),
                                IgnoresMouseInteraction = true
                            };
                            worldIcon.Append(element);
                        }
                        else if (!Data.DrunkWorld && Data.DefeatedMoonlord)
                        {
                            UIElement worldIcon = WorldIcon;
                            UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconUnderworld"))
                            {
                                Top = new StyleDimension(-10f, 0f),
                                Left = new StyleDimension(-7f, 0f),
                                IgnoresMouseInteraction = true
                            };
                            worldIcon.Append(element);
                        }
                        else if (Data.DrunkWorld)
                        {
                            UIElement worldIcon = WorldIcon;
                            UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDrunk"))
                            {
                                Top = new StyleDimension(-10f, 0f),
                                Left = new StyleDimension(-6f, 0f),
                                IgnoresMouseInteraction = true
                            };
                            worldIcon.Append(element);
                        }
                        else if (Data.DrunkWorld && Data.DefeatedMoonlord)
                        {
                            UIElement worldIcon = WorldIcon;
                            UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDrunk"))
                            {
                                Top = new StyleDimension(-10f, 0f),
                                Left = new StyleDimension(-7f, 0f),
                                IgnoresMouseInteraction = true
                            };
                            worldIcon.Append(element);
                        }
                        continue;
                    }
                    #endregion

                    #region RegularSeedIcon
                    if (tempDict[path].depths && !Data.RemixWorld && !Data.DrunkWorld && !Data.DefeatedMoonlord)
                    {
                        UIElement worldIcon = WorldIcon;
                        UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDepths"))
                        {
                            Top = new StyleDimension(-10f, 0f),
                            Left = new StyleDimension(-6f, 0f),
                            IgnoresMouseInteraction = true
                        };
                        worldIcon.Append(element);
                    }
                    if (tempDict[path].depths && !Data.RemixWorld && !Data.DrunkWorld && Data.DefeatedMoonlord)
                    {
                        UIElement worldIcon = WorldIcon;
                        UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDepths"))
                        {
                            Top = new StyleDimension(-10f, 0f),
                            Left = new StyleDimension(-7f, 0f),
                            IgnoresMouseInteraction = true
                        };
                        worldIcon.Append(element);
                    }
                    else if (!tempDict[path].depths && !Data.DrunkWorld && !Data.DefeatedMoonlord)
                    {
                        UIElement worldIcon = WorldIcon;
                        UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconUnderworld"))
                        {
                            Top = new StyleDimension(-10f, 0f),
                            Left = new StyleDimension(-6f, 0f),
                            IgnoresMouseInteraction = true
                        };
                        worldIcon.Append(element);
                    }
                    else if (!tempDict[path].depths && !Data.DrunkWorld && Data.DefeatedMoonlord)
                    {
                        UIElement worldIcon = WorldIcon;
                        UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconUnderworld"))
                        {
                            Top = new StyleDimension(-10f, 0f),
                            Left = new StyleDimension(-7f, 0f),
                            IgnoresMouseInteraction = true
                        };
                        worldIcon.Append(element);
                    }
                    #endregion

                    #region DrunkSeedIcon
                    else if (Data.DrunkWorld && !Data.DefeatedMoonlord && (tempDict[path].depthsLeft && tempDict[path].depthsRight || !tempDict[path].depthsLeft && !tempDict[path].depthsRight))
                    {
                        UIElement worldIcon = WorldIcon;
                        UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDrunk"))
                        {
                            Top = new StyleDimension(-10f, 0f),
                            Left = new StyleDimension(-6f, 0f),
                            IgnoresMouseInteraction = true
                        };
                        worldIcon.Append(element);
                    }
                    else if (Data.DrunkWorld && Data.DefeatedMoonlord && (tempDict[path].depthsLeft && tempDict[path].depthsRight || !tempDict[path].depthsLeft && !tempDict[path].depthsRight))
                    {
                        UIElement worldIcon = WorldIcon;
                        UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDrunk"))
                        {
                            Top = new StyleDimension(-10f, 0f),
                            Left = new StyleDimension(-7f, 0f),
                            IgnoresMouseInteraction = true
                        };
                        worldIcon.Append(element);
                    }

                    else if (Data.DrunkWorld && !Data.DefeatedMoonlord && tempDict[path].depthsLeft && !tempDict[path].depthsRight)
                    {
                        UIElement worldIcon = WorldIcon;
                        UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDrunkLeft"))
                        {
                            Top = new StyleDimension(-10f, 0f),
                            Left = new StyleDimension(-6f, 0f),
                            IgnoresMouseInteraction = true
                        };
                        worldIcon.Append(element);
                    }
                    else if (Data.DrunkWorld && Data.DefeatedMoonlord && tempDict[path].depthsLeft && !tempDict[path].depthsRight)
                    {
                        UIElement worldIcon = WorldIcon;
                        UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDrunkLeft"))
                        {
                            Top = new StyleDimension(-10f, 0f),
                            Left = new StyleDimension(-7f, 0f),
                            IgnoresMouseInteraction = true
                        };
                        worldIcon.Append(element);
                    }

                    else if (Data.DrunkWorld && !Data.DefeatedMoonlord && !tempDict[path].depthsLeft && tempDict[path].depthsRight)
                    {
                        UIElement worldIcon = WorldIcon;
                        UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDrunkRight"))
                        {
                            Top = new StyleDimension(-10f, 0f),
                            Left = new StyleDimension(-6f, 0f),
                            IgnoresMouseInteraction = true
                        };
                        worldIcon.Append(element);
                    }
                    else if (Data.DrunkWorld && Data.DefeatedMoonlord && !tempDict[path].depthsLeft && tempDict[path].depthsRight)
                    {
                        UIElement worldIcon = WorldIcon;
                        UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDrunkRight"))
                        {
                            Top = new StyleDimension(-10f, 0f),
                            Left = new StyleDimension(-7f, 0f),
                            IgnoresMouseInteraction = true
                        };
                        worldIcon.Append(element);
                    }

                    #endregion

                    #region RemixSeedIcon
                    else if (tempDict[path].depths && Data.HasCrimson && Data.RemixWorld && !Data.IsHardMode)
                    {
                        UIElement worldIcon = WorldIcon;
                        UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDepthsRemixCrimson"))
                        {
                            Top = new StyleDimension(-10f, 0f),
                            Left = new StyleDimension(-6f, 0f),
                            IgnoresMouseInteraction = true
                        };
                        worldIcon.Append(element);
                    }
                    else if (tempDict[path].depths && Data.HasCrimson && Data.RemixWorld && Data.IsHardMode)
                    {
                        UIElement worldIcon = WorldIcon;
                        UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDepthsRemixCrimsonHallow"))
                        {
                            Top = new StyleDimension(-10f, 0f),
                            Left = new StyleDimension(-6f, 0f),
                            IgnoresMouseInteraction = true
                        };
                        worldIcon.Append(element);
                    }
                    else if (tempDict[path].depths && Data.HasCorruption && Data.RemixWorld && !Data.IsHardMode)
                    {
                        UIElement worldIcon = WorldIcon;
                        UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDepthsRemixCorruption"))
                        {
                            Top = new StyleDimension(-10f, 0f),
                            Left = new StyleDimension(-6f, 0f),
                            IgnoresMouseInteraction = true
                        };
                        worldIcon.Append(element);
                    }
                    else if (tempDict[path].depths && Data.HasCorruption && Data.RemixWorld && Data.IsHardMode)
                    {
                        UIElement worldIcon = WorldIcon;
                        UIImage element = new UIImage(ModContent.Request<Texture2D>("TheDepths/Assets/WorldIcon/IconDepthsRemixCorruptionHallow"))
                        {
                            Top = new StyleDimension(-10f, 0f),
                            Left = new StyleDimension(-6f, 0f),
                            IgnoresMouseInteraction = true
                        };
                        worldIcon.Append(element);
                    }
                    #endregion
                }
            }
        }
        #endregion

        #region LavaTextureDetour
        private void On_Main_DoUpdate(On_Main.orig_DoUpdate orig, Main self, ref GameTime gameTime)
        {
            orig.Invoke(self, ref gameTime);
            if (!Main.dedServ)
            {
                if (Worldgen.TheDepthsWorldGen.InDepths)
                {
                    LiquidRenderer.Instance._liquidTextures[1] = ModContent.Request<Texture2D>("TheDepths/Assets/Lava/Quicksilver", (AssetRequestMode)1);
                    int[] liquidAssetRegularNum = new int[14] { 0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
                    foreach (int i in liquidAssetRegularNum)
                    {
                        LiquidRenderer.Instance._liquidTextures[i] = Main.Assets.Request<Texture2D>("Images/Misc/water_" + i, (AssetRequestMode)1);
                    }
                }
                else
                {
                    for (int i = 0; i < 15; i++)
                    {
                        LiquidRenderer.Instance._liquidTextures[i] = Main.Assets.Request<Texture2D>("Images/Misc/water_" + i, (AssetRequestMode)1);
                    }
                }
            }
        }
        #endregion

        #region ShellphoneILEdit
        /*private void IL_Player_ItemCheck_ManageRightClickFeatures(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (!c.TryGotoNext(MoveType.After, i => i.MatchStloc(out _), i => i.MatchBr(out _), i => i.MatchLdcI4(ItemID.ShellphoneHell)))
            {
                throw new ILPatchFailureException(ModContent.GetInstance<TheDepths>(), il, null);
            }

            c.EmitDelegate<Func<int, int>>(static shellphone => {
                if (Worldgen.TheDepthsWorldGen.depthsorHell)
                    shellphone = ModContent.ItemType<ShellPhoneDepths>();
                return shellphone;
            });

            MonoModHooks.DumpIL(ModContent.GetInstance<TheDepths>(), il);
        }*/
        #endregion

        #region nightmareflamebootsdetour(Doesn'tWork)
        private void On_Player_SpawnFastRunParticles(On_Player.orig_SpawnFastRunParticles orig, Player self)
        {
            orig.Invoke(self);
            int nump2 = 0;
            if (self.gravDir == -1f)
            {
                nump2 -= self.height;
            }
            if (self.GetModPlayer<TheDepthsPlayer>().nFlare == true)
            {
                int num8 = Dust.NewDust(new Vector2(self.position.X - 4f, self.position.Y + (float)self.height + (float)nump2), Main.LocalPlayer.width + 8, 4, ModContent.DustType<ShadowflameEmber>(), (0f - self.velocity.X) * 0.5f, self.velocity.Y * 0.5f, 50, default(Color), 2f);
                Main.dust[num8].velocity.X = Main.dust[num8].velocity.X * 0.2f;
                Main.dust[num8].velocity.Y = -1.5f - Main.rand.NextFloat() * 0.5f;
                Main.dust[num8].fadeIn = 0.5f;
                Main.dust[num8].noGravity = true;
                Main.dust[num8].shader = GameShaders.Armor.GetSecondaryShader(self.cShoe, self);
            }
        }
        #endregion

        #region MercuryBugCatchingPunishmentDetour
        private Rectangle On_Player_ItemCheck_CatchCritters(On_Player.orig_ItemCheck_CatchCritters orig, Player self, Item sItem, Rectangle itemRectangle)
        {
            orig.Invoke(self, sItem, itemRectangle);
            bool flag = sItem.type == ModContent.ItemType<Items.QuicksilverproofBugNet>() || sItem.type == 4821;
            for (int i = 0; i < 200; i++)
            {
                if (!Main.npc[i].active || Main.npc[i].catchItem <= 0)
                {
                    continue;
                }
                Rectangle value = new Rectangle((int)Main.npc[i].position.X, (int)Main.npc[i].position.Y, Main.npc[i].width, Main.npc[i].height);
                if (!itemRectangle.Intersects(value))
                {
                    continue;
                }
                if (!flag && ItemID.Sets.IsLavaBait[Main.npc[i].catchItem])
                {
                    if (Main.myPlayer == Main.LocalPlayer.whoAmI/* && Player.Hurt(PlayerDeathReason.ByNPC(i), 1, (Main.npc[i].Center.X < Main.LocalPlayer.Center.X) ? 1 : (-1), pvp: false, quiet: false, -1, false, 3f) > 0.0*/ && !Main.LocalPlayer.dead)
                    {
                        if (Main.npc[i].type == ModContent.NPCType<NPCs.EnchantedNightmareWorm>() || Main.npc[i].type == ModContent.NPCType<NPCs.AlbinoRat>() || Main.npc[i].type == ModContent.NPCType<NPCs.QuartzCrawler>())
                        {
                            Main.LocalPlayer.AddBuff(ModContent.BuffType<Buffs.MercuryBoiling>(), 300, quiet: false);
                            Main.LocalPlayer.ClearBuff(BuffID.OnFire);
                        }
                    }
                }
            }
            return itemRectangle;
        }
        #endregion

        #region OuterLowerDepthsProgressBar
        private void On_UIGenProgressBar_DrawSelf(Terraria.GameContent.UI.Elements.On_UIGenProgressBar.orig_DrawSelf orig, Terraria.GameContent.UI.Elements.UIGenProgressBar self, SpriteBatch spriteBatch)
        {
            orig.Invoke(self, spriteBatch);
            Asset<Texture2D> OuterCorrupt = (Asset<Texture2D>)typeof(UIGenProgressBar).GetField("_texOuterCorrupt", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self);
            Asset<Texture2D> OuterCrimson = (Asset<Texture2D>)typeof(UIGenProgressBar).GetField("_texOuterCrimson", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self);
            Asset<Texture2D> OuterLower = (Asset<Texture2D>)typeof(UIGenProgressBar).GetField("_texOuterLower", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self);
            if (OuterCorrupt.IsLoaded && OuterCrimson.IsLoaded && OuterLower.IsLoaded)
            {
                bool flag2 = Worldgen.TheDepthsWorldGen.depthsorHell;
                if (WorldGen.drunkWorldGen && Main.rand.NextBool(2))
                {
                    flag2 = !flag2;
                }
                Color color = default(Color);
                color.PackedValue = 4290947159u;
                Rectangle r = self.GetDimensions().ToRectangle();
                r.X -= 8;
                spriteBatch.Draw(flag2 ? ModContent.Request<Texture2D>("TheDepths/Assets/Loading/Depths_Outer_Lower").Value : OuterLower.Value, r.TopLeft() + new Vector2(44f, 60f), Color.White);
            }
        }
        #endregion

        #region DepthsBackgroundDetour
        private void On_Main_DrawUnderworldBackgroudLayer(On_Main.orig_DrawUnderworldBackgroudLayer orig, bool flat, Vector2 screenOffset, float pushUp, int layerTextureIndex)
        {
            orig.Invoke(flat, screenOffset, pushUp, layerTextureIndex);
            if (Worldgen.TheDepthsWorldGen.InDepths)
            {
                int num = Main.underworldBG[layerTextureIndex];
                var assets = new Asset<Texture2D>[TextureAssets.Underworld.Length];
                for (int i = 0; i < TextureAssets.Underworld.Length; i++)
                {
                    assets[i] = ModContent.Request<Texture2D>("TheDepths/Backgrounds/DepthsUnderworldBG_" + i);
                }
                Asset<Texture2D> asset = assets[num];
                Texture2D value = asset.Value;
                Vector2 vec = new Vector2(value.Width, value.Height) * 0.5f;
                float num2 = (flat ? 1f : ((float)(layerTextureIndex * 2) + 3f));
                Vector2 vector = new Vector2(1f / num2);
                Microsoft.Xna.Framework.Rectangle value2 = new Microsoft.Xna.Framework.Rectangle(0, 0, value.Width, value.Height);
                float num3 = 1.3f;
                Vector2 zero = Vector2.Zero;
                int num4 = 0;
                switch (num)
                {
                    case 1:
                        {
                            int num9 = (int)(Main.GlobalTimeWrappedHourly * 8f) % 4;
                            value2 = new Microsoft.Xna.Framework.Rectangle((num9 >> 1) * (value.Width >> 1), num9 % 2 * (value.Height >> 1), value.Width >> 1, value.Height >> 1);
                            vec *= 0.5f;
                            zero.Y += 175f;
                            break;
                        }
                    case 2:
                        zero.Y += 100f;
                        break;
                    case 3:
                        zero.Y += 75f;
                        break;
                    case 4:
                        num3 = 0.5f;
                        zero.Y -= 0f;
                        break;
                    case 5:
                        zero.Y += num4;
                        break;
                    case 6:
                        {
                            int num8 = (int)(Main.GlobalTimeWrappedHourly * 8f) % 4;
                            value2 = new Microsoft.Xna.Framework.Rectangle(num8 % 2 * (value.Width >> 1), (num8 >> 1) * (value.Height >> 1), value.Width >> 1, value.Height >> 1);
                            vec *= 0.5f;
                            zero.Y += num4;
                            zero.Y += -60f;
                            break;
                        }
                    case 7:
                        {
                            int num7 = (int)(Main.GlobalTimeWrappedHourly * 8f) % 4;
                            value2 = new Microsoft.Xna.Framework.Rectangle(num7 % 2 * (value.Width >> 1), (num7 >> 1) * (value.Height >> 1), value.Width >> 1, value.Height >> 1);
                            vec *= 0.5f;
                            zero.Y += num4;
                            zero.X -= 400f;
                            zero.Y += 90f;
                            break;
                        }
                    case 8:
                        {
                            int num6 = (int)(Main.GlobalTimeWrappedHourly * 8f) % 4;
                            value2 = new Microsoft.Xna.Framework.Rectangle(num6 % 2 * (value.Width >> 1), (num6 >> 1) * (value.Height >> 1), value.Width >> 1, value.Height >> 1);
                            vec *= 0.5f;
                            zero.Y += num4;
                            zero.Y += 90f;
                            break;
                        }
                    case 9:
                        zero.Y += num4;
                        zero.Y -= 30f;
                        break;
                    case 10:
                        zero.Y += 250f * num2;
                        break;
                    case 11:
                        zero.Y += 100f * num2;
                        break;
                    case 12:
                        zero.Y += 20f * num2;
                        break;
                    case 13:
                        {
                            zero.Y += 20f * num2;
                            int num5 = (int)(Main.GlobalTimeWrappedHourly * 8f) % 4;
                            value2 = new Microsoft.Xna.Framework.Rectangle(num5 % 2 * (value.Width >> 1), (num5 >> 1) * (value.Height >> 1), value.Width >> 1, value.Height >> 1);
                            vec *= 0.5f;
                            break;
                        }
                }
                if (flat)
                {
                    num3 *= 1.5f;
                }
                vec *= num3;
                SkyManager.Instance.DrawToDepth(Main.spriteBatch, 1f / vector.X);
                if (flat)
                {
                    zero.Y += (float)(TextureAssets.Underworld[0].Height() >> 1) * 1.3f - vec.Y;
                }
                zero.Y -= pushUp;
                float num10 = num3 * (float)value2.Width;
                int num11 = (int)((float)(int)(screenOffset.X * vector.X - vec.X + zero.X - (float)(Main.screenWidth >> 1)) / num10);
                vec = vec.Floor();
                int num12 = (int)Math.Ceiling((float)Main.screenWidth / num10);
                int num13 = (int)(num3 * ((float)(value2.Width - 1) / vector.X));
                Vector2 vector2 = (new Vector2((num11 - 2) * num13, (float)Main.UnderworldLayer * 16f) + vec - screenOffset) * vector + screenOffset - Main.screenPosition - vec + zero;
                vector2 = vector2.Floor();
                while (vector2.X + num10 < 0f)
                {
                    num11++;
                    vector2.X += num10;
                }
                for (int i = num11 - 2; i <= num11 + 4 + num12; i++)
                {
                    Main.spriteBatch.Draw(value, vector2, value2, Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, num3, SpriteEffects.None, 0f);
                    if (layerTextureIndex == 0)
                    {
                        int num14 = (int)(vector2.Y + (float)value2.Height * num3);
                        Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, new Microsoft.Xna.Framework.Rectangle((int)vector2.X, num14, (int)((float)value2.Width * num3), Math.Max(0, Main.screenHeight - num14)), new Microsoft.Xna.Framework.Color(0, 0, 0));
                    }
                    vector2.X += num10;
                }
            }
        }
        #endregion

        #region QuicksilverTilePlacement
        private bool On_Player_PlaceThing_Tiles_CheckLavaBlocking(On_Player.orig_PlaceThing_Tiles_CheckLavaBlocking orig, Player self)
        {
            orig.Invoke(self);
            bool result = false;
            if (Worldgen.TheDepthsWorldGen.InDepths)
            {
                result = false;
            }
            return result;
        }
        #endregion

        #region DepthsSilverfallLightRemover
        private void On_WaterfallManager_AddLight(On_WaterfallManager.orig_AddLight orig, int waterfallType, int x, int y)
        {
            if (Worldgen.TheDepthsWorldGen.InDepths)
            {
            }
            else
            {
                orig.Invoke(waterfallType, x, y);
                if (waterfallType == 1)
                {
                    float r = 0f;
                    float g = 0f;
                    float b = 0f;
                    Lighting.AddLight(x, y, r, g, b);
                }
            }
        }
        #endregion

        #region QuicksilverCombinations
        private void On_Liquid_GetLiquidMergeTypes(On_Liquid.orig_GetLiquidMergeTypes orig, int thisLiquidType, out int liquidMergeTileType, out int liquidMergeType, bool waterNearby, bool lavaNearby, bool honeyNearby, bool shimmerNearby)
        {
            orig.Invoke(thisLiquidType, out liquidMergeTileType, out liquidMergeType, waterNearby, lavaNearby, honeyNearby, shimmerNearby);
            if (Worldgen.TheDepthsWorldGen.InDepths)
            {
                liquidMergeTileType = ModContent.TileType<Tiles.Quartz>();
                liquidMergeType = thisLiquidType;
                if (thisLiquidType != 0 && waterNearby)
                {
                    switch (thisLiquidType)
                    {
                        case 1:
                            liquidMergeTileType = ModContent.TileType<Tiles.Quartz>();
                            break;
                        case 2:
                            liquidMergeTileType = 229;
                            break;
                        case 3:
                            liquidMergeTileType = 659;
                            break;
                    }
                    liquidMergeType = 0;
                }
                if (thisLiquidType != 1 && lavaNearby)
                {
                    switch (thisLiquidType)
                    {
                        case 0:
                            liquidMergeTileType = ModContent.TileType<Tiles.Quartz>();
                            break;
                        case 2:
                            liquidMergeTileType = ModContent.TileType<Tiles.GlitterBlock>();
                            break;
                        case 3:
                            liquidMergeTileType = 659;
                            break;
                    }
                    liquidMergeType = 1;
                }
                if (thisLiquidType != 2 && honeyNearby)
                {
                    switch (thisLiquidType)
                    {
                        case 0:
                            liquidMergeTileType = 229;
                            break;
                        case 1:
                            liquidMergeTileType = ModContent.TileType<Tiles.GlitterBlock>();
                            break;
                        case 3:
                            liquidMergeTileType = 659;
                            break;
                    }
                    liquidMergeType = 2;
                }
                if (thisLiquidType != 3 && shimmerNearby)
                {
                    switch (thisLiquidType)
                    {
                        case 0:
                            liquidMergeTileType = 659;
                            break;
                        case 1:
                            liquidMergeTileType = 659;
                            break;
                        case 2:
                            liquidMergeTileType = 659;
                            break;
                    }
                    liquidMergeType = 3;
                }
            }
        }
        #endregion

        #region SilverfallTextureDetour
        private void On_WaterfallManager_DrawWaterfall_int_int_int_float_Vector2_Rectangle_Color_SpriteEffects(On_WaterfallManager.orig_DrawWaterfall_int_int_int_float_Vector2_Rectangle_Color_SpriteEffects orig, WaterfallManager self, int waterfallType, int x, int y, float opacity, Vector2 position, Rectangle sourceRect, Color color, SpriteEffects effects)
        {
            orig.Invoke(self, waterfallType, x, y, opacity, position, sourceRect, color, effects);
            if (Worldgen.TheDepthsWorldGen.InDepths)
            {
                if (waterfallType == 1)
                {
                    Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("TheDepths/Assets/Lava/Quicksilver_Silverfall", (AssetRequestMode)1), position, sourceRect, color, 0f, default(Vector2), 1f, effects, 0f);
                }
            }
        }
        #endregion

        #region TemperaryLavabubbleRemoval
        private int Dust_NewDust(Terraria.On_Dust.orig_NewDust orig, Vector2 Position, int Width, int Height, int Type, float SpeedX, float SpeedY, int Alpha, Color newColor, float Scale)
        {
            int index = orig.Invoke(Position, Width, Height, Type, SpeedX, SpeedY, Alpha, newColor, Scale);
            if (Type == DustID.Lava && Worldgen.TheDepthsWorldGen.InDepths)
            {
                Main.dust[index].active = false;
            }
            return index;
        }
        #endregion

        #region OtherworldlyMusicDetour
        private void Main_UpdateAudio_DecideOnTOWMusic(Terraria.On_Main.orig_UpdateAudio_DecideOnTOWMusic orig, Main self)
        {
            orig.Invoke(self);
            Player player = Main.CurrentPlayer;
            if (player.InModBiome(ModContent.GetInstance<DepthsBiome>()))
            {
                Main.newMusic = MusicLoader.GetMusicSlot(mod, "Sounds/Music/DepthsOtherworldly"); //Otherworldly Eerie
            }
        }
        #endregion

        #region DepthsNoSkyLightDetour
        private void TileLightScanner_ApplyHellLight(Terraria.Graphics.Light.On_TileLightScanner.orig_ApplyHellLight orig, TileLightScanner self, Tile tile, int x, int y, ref Vector3 lightColor)
        {
            orig.Invoke(self, tile, x, y, ref lightColor);
            if (Worldgen.TheDepthsWorldGen.InDepths && ModContent.GetInstance<TheDepthsClientConfig>().DepthsLightingConfig)
            {
                if ((!tile.HasTile || !Main.tileNoSunLight[tile.TileType] || ((tile.Slope != 0 || tile.IsHalfBlock) && Main.tile[x, y - 1].LiquidAmount == 0 && Main.tile[x, y + 1].LiquidAmount == 0 && Main.tile[x - 1, y].LiquidAmount == 0 && Main.tile[x + 1, y].LiquidAmount == 0)) && (Main.wallLight[tile.WallType] || tile.WallType == 73 || tile.WallType == 227) && tile.LiquidAmount < 200 && (!tile.IsHalfBlock || Main.tile[x, y - 1].LiquidAmount < 200))
                {
                    lightColor = Vector3.Zero;
                }
                if ((!tile.HasTile || tile.IsHalfBlock || !Main.tileNoSunLight[tile.TileType]) && tile.LiquidAmount < byte.MaxValue)
                {
                    lightColor = Vector3.Zero;
                }
                lightColor = Vector3.Zero;
            }
        }
        #endregion

        #region QuicksilverNoLightDetour
        private void On_TileLightScanner_ApplyLiquidLight(On_TileLightScanner.orig_ApplyLiquidLight orig, TileLightScanner self, Tile tile, ref Vector3 lightColor)
        {
            orig.Invoke(self, tile, ref lightColor);
            if (tile.LiquidType == 1 && Worldgen.TheDepthsWorldGen.InDepths)
                lightColor = Vector3.Zero;
        }
        #endregion

        #region DepthsNoHeatDistortionILEdit
        private void NoHeap(ILContext il)
        {
            var c = new ILCursor(il);
            try
            {
                c.GotoNext(MoveType.After,
                    i => i.MatchLdstr("HeatDistortion"),
                    i => i.MatchLdsfld<Main>("UseHeatDistortion"));

                c.EmitDelegate((bool useHeatDistortion) =>
                {
                    if (Worldgen.TheDepthsWorldGen.depthsorHell)
                    {
                        return false;
                    }
                    return useHeatDistortion;
                });
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
        }
        #endregion

        #region NoEvaporationILEdit
        private void Evaporation(ILContext il)
        {
            var c = new ILCursor(il);
            try
            {
                int b = 0;
                int tile = 0;
                c.GotoNext(MoveType.After,
                    i => i.MatchLdloca(out tile),
                    i => i.MatchCall<Tile>("get_liquid"),
                    i => i.MatchDup(),
                    i => i.MatchLdindU1(),
                    i => i.MatchLdloc(out b),
                    i => i.MatchSub(),
                    i => i.MatchConvU1(),
                    i => i.MatchStindI1());

                c.Index -= 3;
                c.Emit(OpCodes.Ldloca_S, (byte)tile);
                c.EmitDelegate((byte num, ref Tile liqTile) =>
                {
                    if (Worldgen.TheDepthsWorldGen.depthsorHell && liqTile.LiquidType == LiquidID.Water /*&& ModContent.GetInstance<TheDepthsServerConfig>().DepthsNoWaterVapor*/)
                    {
                        return (byte)0;
                    }
                    return num;
                });
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
        }
        #endregion

        #region UnderworldTextureILEdit
        /*private void ILMainDrawUnderworldBackground(ILContext il)
        {
            ILCursor c = new(il);

            int asset = 0, texture = 0;
            c.GotoNext(i => i.MatchLdloc(out asset),
                i => i.OpCode == OpCodes.Callvirt,
                i => i.MatchStloc(out texture));

            c.Emit(OpCodes.Ldloc, asset);
            c.Emit(OpCodes.Ldloc, 0);
            c.EmitDelegate<Func<Asset<Texture2D>, int, Asset<Texture2D>>>((asset, index) =>
            {
                if (Worldgen.TheDepthsWorldGen.depthsorHell)
                    return TheDepths.texture[index];
                return asset;
            });
            c.Emit(OpCodes.Stloc, asset);
        }*/
        #endregion
    }
}