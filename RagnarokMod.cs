using RagnarokMod.Buffs;
using RagnarokMod.Utils;
using System.IO;
using Terraria;
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
            }
        }

        public override void Load()
		{
			
		}

        public override void Unload()
        {
			PlayerHelper.Unload();
        }
    }
}