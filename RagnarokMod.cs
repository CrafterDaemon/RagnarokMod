using RagnarokMod.Buffs;
using RagnarokMod.Items;
using RagnarokMod.Sounds;
using RagnarokMod.Utils;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RagnarokMod
{
    public class RagnarokMod : Mod
    {
        internal static RagnarokMod mod;

        public RagnarokMod()
        {
            mod = this;
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            byte packetType = reader.ReadByte();
            switch (packetType)
            {
                case 0:
                    byte targetIndex = reader.ReadByte();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        // Forward to target client
                        ModPacket packet = GetPacket();
                        packet.Write((byte)0);
                        packet.Write(targetIndex);
                        packet.Send(targetIndex);
                    }
                    else
                    {
                        // We are the target, apply the buff
                        Main.player[targetIndex].AddBuff(ModContent.BuffType<LeviathanHeartBubble>(), 5 * 60);
                    }
                    break;
                case 1:
                    byte playerID = reader.ReadByte();
                    bool start = reader.ReadBoolean();
                    Player player = Main.player[playerID];
                    var ragnarokplayer = player.GetModPlayer<RagnarokModPlayer>();
                    if (start)
                    {
                        if (!SoundEngine.TryGetActiveSound(ragnarokplayer.riffSlot, out var sound) || !sound.IsPlaying)
                        {
                            ragnarokplayer.riffSlot = SoundEngine.PlaySound(
                                RagnarokModSounds.fretsriff,
                                player.Center
                            );
                            ragnarokplayer.riffPlaying = true;
                        }
                    }
                    else
                    {
                        if (SoundEngine.TryGetActiveSound(ragnarokplayer.riffSlot, out var sound))
                        {
                            sound.Stop();
                        }
                        ragnarokplayer.riffPlaying = false;
                    }
                    break;
                case 2:
                    byte myplayer = reader.ReadByte();
                    Player player1 = Main.player[myplayer];
                    bool hasFavorited = false;
                    foreach (Item item in player1.inventory)
                    {
                        if (item.type == ItemID.None)
                            continue;
                        if (item.type == ModContent.ItemType<PrimalTerror>() && item.favorited) hasFavorited = true;
                    }
                    if (player1.HeldItem.type == ModContent.ItemType<PrimalTerror>() || hasFavorited)
                        SoundEngine.PlaySound(new SoundStyle("CalamityMod/Sounds/Custom/Scare"));
                    break;
            }

        }

        public override void Load()
        {
            EmpowermentHelper.Load();
        }

        public override void Unload()
        {
            EmpowermentHelper.Unload();
            PlayerHelper.Unload();
        }
    }
}