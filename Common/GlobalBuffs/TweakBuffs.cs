using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RagnarokMod.Utils;
using ThoriumMod.Buffs.Mount;
using CalamityMod;
using ThoriumMod;
using ThoriumMod.Utilities;
using ThoriumMod.NPCs;
using ThoriumMod.Mounts;
using ThoriumMod.Buffs;
using System;
using System.IO;

namespace RagnarokMod.Common.GlobalBuffs
{
    public class TweakBuffs : GlobalBuff
    {
        public override void Update(int type, Player player, ref int buffIndex)
        {
            ModLoader.TryGetMod("ThoriumMod", out Mod thorium);
            if (player.HasBuff(ModContent.BuffType<GoldenScaleBuff>()))
            {
                player.mount.SetMount(ModContent.MountType<GoldenScaleMount>(), player, false);
                player.buffTime[buffIndex] = 10;
                player.GetThoriumPlayer().transformGoldenScale = true;
                player.GetThoriumPlayer().transformation = true;
                var calamityPlayer = player.Calamity();
                if (!calamityPlayer.ZoneAbyss)
                {
                    if (player.breath <= player.breathMax + 2)
                    {
                        player.breath = player.breathMax + 3;
                    }
                }
            }
            if (player.HasBuff(ModContent.BuffType<DepthDiverAura>()))
            {
                var calamityPlayer = player.Calamity();
                if (!calamityPlayer.ZoneAbyss)
                {
                    if (player.breath <= player.breathMax + 2)
                    {
                        player.breath = player.breathMax + 3;
                    }
                }
                else
                {
                    player.moveSpeed += 0.2f;
                    player.statDefense += 10;
                    if (player.breath < player.breathMax - 25 && player.breath > 5)
                    {
                        Random rnd = new Random();
                        if (rnd.Next(1, 600) == 1)
                        {
                            player.breath = player.breath + 20;
                        }
                    }
                }
            }
        }
    }
}