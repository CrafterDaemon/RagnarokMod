using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Utilities;
using ThoriumMod;

namespace RagnarokMod.Buffs
{
	public class AntarcticReinforcementDebuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.debuff[base.Type] = true;
			Main.pvpBuff[base.Type] = true;
			Main.buffNoSave[base.Type] = false;
			BuffID.Sets.NurseCannotRemoveDebuff[base.Type] = true;
		}
		public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
		{
			rare = 3;
		}
		public override void Update(Player player, ref int buffIndex)
		{
		}
	}
}
