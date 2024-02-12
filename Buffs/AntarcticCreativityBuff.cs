using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Utilities;
using ThoriumMod;

namespace RagnarokMod.Buffs
{
	public class AntarcticCreativityBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.debuff[base.Type] = false;
			Main.pvpBuff[base.Type] = true;
			Main.buffNoSave[base.Type] = true;
		}
		public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
		{
			rare = 3;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
			thoriumPlayer.inspirationRegenBonus += 0.15f;
			player.GetAttackSpeed(ThoriumDamageBase<BardDamage>.Instance) += 0.1f;
		}
	}
}
