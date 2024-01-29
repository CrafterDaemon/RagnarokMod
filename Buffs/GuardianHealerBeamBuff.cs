using System;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Utilities;
using ThoriumMod;

namespace RagnarokMod.Buffs
{
	public class GuardianHealerBeamBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[base.Type] = true;
			Main.buffNoTimeDisplay[base.Type] = true;
		}
		public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
		{
			rare = 3;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			player.endurance+=0.1f;
			ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
			thoriumPlayer.lifeRecovery += 5;
		}
	}
}
